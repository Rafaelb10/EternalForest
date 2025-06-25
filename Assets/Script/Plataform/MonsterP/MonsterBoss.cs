using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterBoss : MonsterPlataform, IDamageable
{
    protected bool _playerInZoneBoss;
    [SerializeField] private Transform _spawTransform;
    private bool _back;
    private Vector2 _velocity = Vector2.zero;

    private bool _attackingCoowldownSlap;
    private float _damageBoss = 30;

    private float _hp = 300;
    private float _hpMax;

    private float _xp = 300;
    private float _coin = 150;

    [SerializeField] private GameObject _enemyHp;
    [SerializeField] private Image _hpImage;

    [SerializeField] private float _hpBarSpeed = 3f;
    private float _targetFill = 1f;

    [SerializeField] private GameObject _prefabBat;
    [SerializeField] private GameObject _prefabAngel;

    [SerializeField] private List<Transform> _spawnPointsOneBat;
    [SerializeField] private List<Transform> _spawnPointsTwoAngel;

    [SerializeField] private GameObject _bulletTargetPrefab;

    private bool _isBuffActive = false;
    private float _originalDamage;
    private float _originalSpeed = 3f;

    private float _cooldownTime = 20f;
    private float _lastAttackTime = -Mathf.Infinity;

    private float _buffedSpeed = 3f;

    private void Start()
    {
        _hpMax = _hp;

        _enemyHp.SetActive(true);
    }

    private void Update()
    {
        Move();

        if (_playerInZoneBoss == true)
        {
            Attack();
        }

        _targetFill = Mathf.Clamp01(_hp / _hpMax);
        _hpImage.fillAmount = Mathf.Lerp(_hpImage.fillAmount, _targetFill, Time.deltaTime * _hpBarSpeed);

        Damage = _damageBoss;
        _playerInZoneBoss = _playerInZone;

        if (State == 3)
        {
            gameObject.SetActive(false);
        }
    }

    protected override void Attack()
    {
        if (Time.time < _lastAttackTime + _cooldownTime) return;

        float healthPercentage = _hp / _hpMax;
        _cooldownTime = (healthPercentage <= 0.5f) ? 10f : 20f;

        int randomAttack = Random.Range(1, 4);

        switch (randomAttack)
        {
            case 1:
                AttackOne();
                break;
            case 2:
                AttackTwo();
                break;
            case 3:
                AttackThree();
                break;
        }

        _lastAttackTime = Time.time;
    }

    public void AttackOne() // Disparar Varias esferas que perseguem
    {
        Transform target = FindAnyObjectByType<Player>().transform;

        int bulletCount = 5;
        float spacing = 0.5f; 

        for (int i = 0; i < bulletCount; i++)
        {
            float offset = (i - (bulletCount - 1) / 2f) * spacing;

            Vector3 spawnPosition = transform.position + new Vector3(offset, 0f, 0f);

            GameObject bullet = Instantiate(_bulletTargetPrefab, spawnPosition, Quaternion.identity);

            Vector2 dir = (target.position - bullet.transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            BulletTarget bt = bullet.GetComponent<BulletTarget>();
            if (bt != null)
            {
                bt.SetTarget(target);
            }
        }
    }

    public void AttackTwo() // Aumentar a Velocidade e Dano
    {
        if (_isBuffActive) return;

        float healthPercentage = _hp / _hpMax;
        float duration = (healthPercentage <= 0.5f) ? 20f : 10f;

        StartCoroutine(BuffRoutine(duration));
    }

    IEnumerator BuffRoutine(float duration)
    {
        _isBuffActive = true;

        _originalDamage = _damageBoss;

        _damageBoss += 20f;
        _buffedSpeed = _originalSpeed + 3f;

        Debug.Log("Buff de dano e velocidade aplicado por " + duration + " segundos.");

        yield return new WaitForSeconds(duration);

        _damageBoss = _originalDamage;
        _buffedSpeed = _originalSpeed;

        _isBuffActive = false;

        Debug.Log("Buff encerrado.");
    }

    public void AttackThree() // Chamar Minion
    {
        bool isLowHealth = _hp <= _hpMax * 0.5f;

        int countOne = isLowHealth ? Random.Range(1, _spawnPointsOneBat.Count + 1) : 1;
        int countTwo = isLowHealth ? Random.Range(1, _spawnPointsTwoAngel.Count + 1) : 1;

        List<Transform> availablePointsOne = new List<Transform>(_spawnPointsOneBat);
        List<Transform> availablePointsTwo = new List<Transform>(_spawnPointsTwoAngel);

        for (int i = 0; i < countOne && availablePointsOne.Count > 0; i++)
        {
            int index = Random.Range(0, availablePointsOne.Count);
            Instantiate(_prefabBat, availablePointsOne[index].position, Quaternion.identity);
            availablePointsOne.RemoveAt(index); 
        }

        for (int i = 0; i < countTwo && availablePointsTwo.Count > 0; i++)
        {
            int index = Random.Range(0, availablePointsTwo.Count);
            Instantiate(_prefabAngel, availablePointsTwo[index].position, Quaternion.identity);
            availablePointsTwo.RemoveAt(index);
        }
    }

    protected override void Move()
    {
        float speed = _buffedSpeed;

        if (_playerInZoneBoss == true)
        {
            Vector2 playerPos = FindAnyObjectByType<Player>().transform.position;
            Vector2 directionToPlayer = (playerPos - (Vector2)transform.position).normalized;

            _velocity = Vector2.Lerp(_velocity, directionToPlayer * speed, Time.deltaTime * 2f);
            transform.position += (Vector3)(_velocity * Time.deltaTime);

            Attack();
        }
        else
        {
            Vector2 spaw = _spawTransform.transform.position;
            Vector2 directionToSpaw = (spaw - (Vector2)transform.position).normalized;

            _velocity = Vector2.Lerp(_velocity, directionToSpaw * speed, Time.deltaTime * 2f);
            transform.position += (Vector3)(_velocity * Time.deltaTime);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<Player>(out var player))
        {
            if (_attackingCoowldownSlap == false)
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
            if (_attackingCoowldownSlap == false)
            {
                FindAnyObjectByType<Player>().TakeDamage(Damage);
                StartCoroutine(AttackCooldown());
            }
        }
    }


    IEnumerator AttackCooldown()
    {
        _attackingCoowldownSlap = true;
        yield return new WaitForSeconds(1);
        _attackingCoowldownSlap = false;
    }

    public override void TakeDamage(float Damage)
    {
        _hp = _hp - Damage;

        if (_hp <= 0)
        {
            FindAnyObjectByType<Player>().GainXp(_xp);
            FindAnyObjectByType<Player>().GainCoin(_coin);
            State = 3;
            FindAnyObjectByType<SaveController>().ChangeLightPlayer();
            FindAnyObjectByType<SaveController>().SaveBeforeCombate();
        }
    }

}

