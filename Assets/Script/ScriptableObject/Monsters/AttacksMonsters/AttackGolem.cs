using System.Collections;
using UnityEngine;

public class AttackGolem : AttackLogic
{
    private Vector2 _randomDirection = Vector2.zero;
    private float _randomMoveTime = 0f;

    [SerializeField] private GameObject _spikePregab;
    [SerializeField] private Transform _spikeSpaw;
    private bool _attackingCoowldown;

    protected override void AttackOne()
    {
        if (_randomMoveTime <= 0f)
        {
            _randomDirection = Random.insideUnitCircle.normalized;
            _randomMoveTime = Random.Range(1f, 3f);
        }

        transform.position += (Vector3)(_randomDirection * 20f * Time.deltaTime);
        _randomMoveTime -= Time.deltaTime;
    }

    protected override void AttackTwo()
    {
        if (_attackingCoowldown == false)
        {
            int bulletCount = Random.Range(4, 7);

            Vector2 spawnCenter = _spikeSpaw.position;
            Vector2 areaSize = Vector2.one;

            BoxCollider2D box = _spikeSpaw.GetComponent<BoxCollider2D>();
            if (box != null)
            {
                areaSize = box.size;
            }

            for (int i = 0; i < bulletCount; i++)
            {
                float offsetX = Random.Range(-areaSize.x / 2f, areaSize.x / 2f);
                float offsetY = Random.Range(-areaSize.y / 2f, areaSize.y / 2f);
                Vector3 spawnPos = spawnCenter + new Vector2(offsetX, offsetY);

                GameObject bullet = Instantiate(_spikePregab, spawnPos, Quaternion.identity);

                Transform target = FindAnyObjectByType<Player>().transform;
                Vector2 dir = (target.position - bullet.transform.position).normalized;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                bullet.transform.rotation = Quaternion.Euler(0f, 0f, angle);

                BulletTarget bt = bullet.GetComponent<BulletTarget>();
                if (bt != null)
                {
                    bt.SetTarget(target);
                }
            }
            StartCoroutine(AttackDuranting());
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            _randomDirection = Random.insideUnitCircle.normalized;
            _randomMoveTime = Random.Range(1f, 2f);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            FindAnyObjectByType<Player>().TakeDamage(FindAnyObjectByType<Monster>().Strenght);
        }
    }

    IEnumerator AttackDuranting()
    {
        _attackingCoowldown = true;
        yield return new WaitForSeconds(5);
        _attackingCoowldown = false;
    }
}
