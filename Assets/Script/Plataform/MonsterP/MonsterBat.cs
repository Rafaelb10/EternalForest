using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    private float _damagebat = 10;

    private float _hp = 90;
    private float _hpMax;

    private float _xp = 100;
    private float _coin = 55;

    [SerializeField] private GameObject _enemyHp;
    [SerializeField] private Image _hpImage;

    [SerializeField] private float _hpBarSpeed = 3f;
    private float _targetFill = 1f;

    [SerializeField] private GameObject _bulletTargetPrefab;

    private Animator anim;
    private bool isAttacking = false;

    private Rigidbody2D rb;

    private void Start()
    {
        _hpMax = _hp;
        anim = GetComponent<Animator>();
        _enemyHp.SetActive(true);
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _targetFill = Mathf.Clamp01(_hp / _hpMax);
        _hpImage.fillAmount = Mathf.Lerp(_hpImage.fillAmount, _targetFill, Time.deltaTime * _hpBarSpeed);

        if (State == 3)
        {
            gameObject.SetActive(false);
        }

        _playerInZoneBat = _playerInZone;
        Damage = _damagebat;
    }

    private void FixedUpdate()
    {
        Move();
    }

    protected override void Attack()
    {
        if (_shootcooldown == false && !isAttacking)
        {
            anim.SetTrigger("Attack");
            StartCoroutine(ShootWithDelay(0.3f));
        }
    }

    IEnumerator ShootWithDelay(float delay)
    {
        isAttacking = true;
        yield return new WaitForSeconds(delay);

        GameObject bullet = Instantiate(_bulletTargetPrefab, transform.position, Quaternion.identity);
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
        yield return new WaitForSeconds(0.7f);
        isAttacking = false;
    }

    protected override void Move()
    {
        if (isAttacking)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = Vector2.zero;

        if (_playerInZoneBat)
        {
            Vector2 playerPos = FindAnyObjectByType<Player>().transform.position;
            direction = (playerPos - rb.position).normalized;
            FlipTowards(playerPos);
            Attack();
        }
        else
        {
            if (!_back)
            {
                if (_randomMoveTime <= 0f)
                {
                    _randomDirection = Random.insideUnitCircle.normalized;
                    _randomMoveTime = Random.Range(1f, 3f);
                }

                FlipTowards(transform.position + (Vector3)_randomDirection);
                direction = _randomDirection;
                _randomMoveTime -= Time.fixedDeltaTime;

                StartCoroutine(GotoSpaw());
            }
            else
            {
                Vector2 spaw = _spawTransform.position;
                direction = (spaw - rb.position).normalized;
                FlipTowards(spaw);
            }
        }

        rb.linearVelocity = direction * 3f;
    }

    IEnumerator GotoSpaw()
    {
        yield return new WaitForSeconds(10);
        _back = true;
        yield return new WaitForSeconds(10);
        _back = false;
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
            FacePlayer(player.transform);
            if (!_attackingCooldownSlap && !isAttacking)
            {
                StartCoroutine(PlayAttack());
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<Player>(out var player))
        {
            FacePlayer(player.transform);
            if (!_attackingCooldownSlap && !isAttacking)
            {
                StartCoroutine(PlayAttack());
            }
        }
    }

    private void FacePlayer(Transform player)
    {
        FlipTowards(player.position);
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
        } while (info.IsName("AttackMorcego") && info.normalizedTime < 1f);

        yield return new WaitForSeconds(1f);
        isAttacking = false;
        _attackingCooldownSlap = false;
    }

    IEnumerator FireCooldown()
    {
        _shootcooldown = true;
        yield return new WaitForSeconds(5);
        _shootcooldown = false;
    }

    [SerializeField] private GameObject _deathPrefab;
    private SpriteRenderer spriteRenderer;

    public override void TakeDamage(float Damage)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        _hp -= Damage;
        StartCoroutine(FlashRed());

        if (_hp <= 0)
        {
            FindAnyObjectByType<Player>().GainXp(_xp);
            FindAnyObjectByType<Player>().GainCoin(_coin);
            Instantiate(_deathPrefab, transform.position, Quaternion.identity);
            State = 3;
            FindAnyObjectByType<SaveController>().SaveBeforeCombate();
        }
    }

    private IEnumerator FlashRed()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.15f);
            spriteRenderer.color = Color.white;
        }
    }
}