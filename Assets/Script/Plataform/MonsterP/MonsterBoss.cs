using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterBoss : MonsterPlataform, IDamageable
{
    protected bool _playerInZoneBoss;
    [SerializeField] private Transform _spawTransform;
    private Vector2 _velocity = Vector2.zero;

    private bool _attackingCooldownSlap;
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
    private float _buffedSpeed = 3f;

    private float _cooldownTime = 20f;
    private float _lastAttackTime = -Mathf.Infinity;

    private Animator anim;
    private bool isAttacking = false;
    private bool isDying = false;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _hpMax = _hp;
        anim = GetComponent<Animator>();
        _enemyHp.SetActive(true);
    }

    private void Update()
    {
        if (isDying) return;

        Move();

        if (_playerInZoneBoss)
        {
            Attack();
        }

        if (_state == 3)
        {
            gameObject.SetActive(false);
        }

        _targetFill = Mathf.Clamp01(_hp / _hpMax);
        _hpImage.fillAmount = Mathf.Lerp(_hpImage.fillAmount, _targetFill, Time.deltaTime * _hpBarSpeed);

        Damage = _damageBoss;
        _playerInZoneBoss = _playerInZone;
    }

    protected override void Attack()
    {
        if (Time.time < _lastAttackTime + _cooldownTime || isAttacking) return;

        float healthPercentage = _hp / _hpMax;
        _cooldownTime = (healthPercentage <= 0.5f) ? 5f : 10f;

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

    public void AttackOne()
    {
        isAttacking = true;
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
            if (bt != null) bt.SetTarget(target);
        }

        StartCoroutine(ResetAttack(1f));
    }

    public void AttackTwo()
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

        yield return new WaitForSeconds(duration);

        _damageBoss = _originalDamage;
        _buffedSpeed = _originalSpeed;
        _isBuffActive = false;
    }

    public void AttackThree()
    {
        bool isLowHealth = _hp <= _hpMax * 0.5f;
        anim.SetTrigger("Attack");

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

        StartCoroutine(ResetAttack(1.5f));
    }

    IEnumerator ResetAttack(float delay)
    {
        yield return new WaitForSeconds(delay);
        isAttacking = false;
    }

    protected override void Move()
    {
        if (isAttacking || isDying) return;

        float speed = _buffedSpeed;
        Vector2 newPosition = rb.position;

        if (_playerInZoneBoss)
        {
            Vector2 playerPos = FindAnyObjectByType<Player>().transform.position;
            Vector2 direction = (playerPos - rb.position).normalized;

            FlipTowards(playerPos);
            _velocity = Vector2.Lerp(_velocity, direction * speed, Time.deltaTime * 2f);
        }
        else
        {
            Vector2 spaw = _spawTransform.position;
            Vector2 direction = (spaw - rb.position).normalized;

            FlipTowards(spaw);
            _velocity = Vector2.Lerp(_velocity, direction * speed, Time.deltaTime * 2f);
        }

        newPosition += _velocity * Time.deltaTime;
        rb.MovePosition(newPosition);
    }

    private void FlipTowards(Vector2 target)
    {
        float dir = target.x - transform.position.x;
        if (Mathf.Abs(dir) > 0.1f)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(dir);
            transform.localScale = scale;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<Player>(out var player))
        {
            if (!_attackingCooldownSlap && !isAttacking)
            {
                FlipTowards(player.transform.position);
                StartCoroutine(PlayAttack());
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<Player>(out var player))
        {
            if (!_attackingCooldownSlap && !isAttacking)
            {
                FlipTowards(player.transform.position);
                StartCoroutine(PlayAttack());
            }
        }
    }

    IEnumerator PlayAttack()
    {
        isAttacking = true;
        anim.SetTrigger("Attack");
        _attackingCooldownSlap = true;

        yield return new WaitForSeconds(0.2f);
        FindAnyObjectByType<Player>().TakeDamage(Damage);

        AnimatorStateInfo info;
        do
        {
            yield return null;
            info = anim.GetCurrentAnimatorStateInfo(0);
        } while (info.IsName("BossAttack") && info.normalizedTime < 1f);

        yield return new WaitForSeconds(1f);
        isAttacking = false;
        _attackingCooldownSlap = false;
    }

    public override void TakeDamage(float Damage)
    {
        if (isDying) return;

        _hp -= Damage;

        if (_hp <= 0)
        {
            isDying = true;

            FindAnyObjectByType<Player>().GainXp(_xp);
            FindAnyObjectByType<Player>().GainCoin(_coin);

            State = 3;

            FindAnyObjectByType<SaveController>().ChangeLightPlayer();
            FindAnyObjectByType<SaveController>().SaveBeforeCombate();
        }
    }
}