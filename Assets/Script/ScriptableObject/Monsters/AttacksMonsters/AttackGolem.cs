using System.Collections;
using UnityEngine;

public class AttackGolem : AttackLogic
{
    private Rigidbody2D _rb;
    private Vector2 _randomDirection = Vector2.zero;
    private float _randomMoveTime = 0f;

    [SerializeField] private GameObject _spikePrefab;
    [SerializeField] private Transform _spikeSpawn;
    private bool _attackingCooldown;
    private bool _damageCooldown;
    private bool _movementPaused;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    protected override void AttackOne()
    {
        if (_movementPaused) return;

        if (_randomMoveTime <= 0f)
        {
            _randomDirection = Random.insideUnitCircle.normalized;
            _randomMoveTime = Random.Range(1f, 3f);
        }

        _rb.linearVelocity = _randomDirection * 15f;
        _randomMoveTime -= Time.deltaTime;
    }

    protected override void AttackTwo()
    {
        if (_movementPaused) return;

        Transform player = FindAnyObjectByType<Player>()?.transform;
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            _rb.linearVelocity = direction * 4f;
        }

        if (!_attackingCooldown)
        {
            StartCoroutine(SpawnSpikes());
        }
    }

    private IEnumerator SpawnSpikes()
    {
        _attackingCooldown = true;

        int spikeCount = Random.Range(4, 7);
        Vector2 spawnCenter = _spikeSpawn.position;
        Vector2 areaSize = Vector2.one;

        BoxCollider2D box = _spikeSpawn.GetComponent<BoxCollider2D>();
        if (box != null)
        {
            areaSize = box.size;
        }

        for (int i = 0; i < spikeCount; i++)
        {
            float offsetX = Random.Range(-areaSize.x / 2f, areaSize.x / 2f);
            float offsetY = Random.Range(-areaSize.y / 2f, areaSize.y / 2f);
            Vector3 spawnPos = spawnCenter + new Vector2(offsetX, offsetY);

            Instantiate(_spikePrefab, spawnPos, Quaternion.identity);
        }

        yield return new WaitForSeconds(5f);
        _attackingCooldown = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!_damageCooldown)
            {
                Player player = collision.gameObject.GetComponent<Player>();
                if (player != null)
                {
                    player.TakeDamage(FindAnyObjectByType<Monster>().Strenght);
                }

                StartCoroutine(HandlePlayerCollisionPause());
            }
        }
        else
        {
            _randomDirection = Random.insideUnitCircle.normalized;
            _randomMoveTime = Random.Range(1f, 2f);
        }
    }

    private IEnumerator HandlePlayerCollisionPause()
    {
        _movementPaused = true;
        _damageCooldown = true;
        _rb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(0.6f); 
        _movementPaused = false;

        yield return new WaitForSeconds(1.5f); 
        _damageCooldown = false;
    }
}