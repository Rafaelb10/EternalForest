using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class MonsterBat : MonsterPlataform
{
    protected bool _playerInZoneBat;
    [SerializeField] private Transform _spawTransform;
    private bool _back;
    private Vector2 _velocity = Vector2.zero;

    private Vector2 _randomDirection = Vector2.zero;
    private float _randomMoveTime = 0f;

    private bool _attackingCooldownSlap;
    private bool _shootcooldown;
    private float _damagebat = 5;

    private float _hp = 90;
    private float _hpmax;

    private float _xp = 100;
    private float _coin = 55;

    [SerializeField] private GameObject _bulletTargetPrefab;

    private void Start()
    {
        _hpmax = _hp;
    }


    private void Update()
    {
        Move();

        if (State == 3)
        {
            gameObject.SetActive(false);
        }

        _playerInZoneBat = _playerInZone;
        Damage = _damagebat;
    }

    protected override void Attack() 
    {
        if (_shootcooldown == false)
        {
            GameObject bullet = Instantiate(_bulletTargetPrefab, gameObject.transform.position, Quaternion.identity);
            Transform target = FindAnyObjectByType<Player>().transform;

            Vector2 dir = (target.position - bullet.transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            BulletTarget bt = bullet.GetComponent<BulletTarget>();
            if (bt != null)
            {
                bt.SetTarget(target);
            }

            StartCoroutine(FireCooldown());
        }
    }
    protected override void Move() 
    { 
        if (_playerInZoneBat == true)
        {
            Vector2 playerPos = FindAnyObjectByType<Player>().transform.position;
            Vector2 directionToPlayer = (playerPos - (Vector2)transform.position).normalized;

            _velocity = Vector2.Lerp(_velocity, directionToPlayer * 3f, Time.deltaTime * 2f);
            transform.position += (Vector3)(_velocity * Time.deltaTime);

            Attack();
        }
        else
        {
            if (_back == false)
            {
                if (_randomMoveTime <= 0f)
                {
                    _randomDirection = Random.insideUnitCircle.normalized;
                    _randomMoveTime = Random.Range(1f, 3f);
                }

                transform.position += (Vector3)(_randomDirection * 3f * Time.deltaTime);
                _randomMoveTime -= Time.deltaTime;

                StartCoroutine(GotoSpaw());
            }
            else
            {
                Vector2 spaw = _spawTransform.transform.position;
                Vector2 directionToSpaw = (spaw - (Vector2)transform.position).normalized;

                _velocity = Vector2.Lerp(_velocity, directionToSpaw * 3f, Time.deltaTime * 2f);
                transform.position += (Vector3)(_velocity * Time.deltaTime);
            }
        }
    }

    IEnumerator GotoSpaw() 
    {
        yield return new WaitForSeconds(10);
        _back = true;
        yield return new WaitForSeconds(10);
        _back = false;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<Player>(out var player))
        {
            if (_attackingCooldownSlap == false)
            {
                FindAnyObjectByType<Player>().TakeDamage(Damage);
                StartCoroutine(AttackCooldown());
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<Player>(out var player))
        {
            if (_attackingCooldownSlap == false)
            {
                FindAnyObjectByType<Player>().TakeDamage(Damage);
                StartCoroutine(AttackCooldown());
            }
        }
    }

    IEnumerator AttackCooldown()
    {
        _attackingCooldownSlap = true;
        yield return new WaitForSeconds(1);
        _attackingCooldownSlap = false;
    }

    IEnumerator FireCooldown()
    {
        _shootcooldown = true;
        yield return new WaitForSeconds(5);
        _shootcooldown = false;
    }

    public void TakeDamage(float Damage)
    {
        _hp = _hp - Damage;

        if (_hp <= 0)
        {
            FindAnyObjectByType<Player>().GainXp(_xp);
            FindAnyObjectByType<Player>().GainCoin(_coin);
            State = 3;
            FindAnyObjectByType<SaveController>().SaveBeforeCombate();
        }
    }
}
