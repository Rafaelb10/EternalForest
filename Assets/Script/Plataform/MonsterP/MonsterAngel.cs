using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MonsterAngel : MonsterPlataform, IDamageable
{
    private float moveSpeed = 2f;
    [SerializeField] private Transform groundCheckRight;
    private float groundCheckDistance = 0.7f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private int moveDirection = 1;

    private bool _attackingCoowldownSlap;
    private float _damageAngel = 10;

    private float _hp = 150;
    private float _hpMax;

    private float _xp = 150;
    private float _coin = 75;

    [SerializeField] private Transform forwardCheck;
    [SerializeField] private float forwardCheckDistance = 5f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float dashForce = 10f;

    [SerializeField] private GameObject _enemyHp;
    [SerializeField] private Image _hpImage;

    [SerializeField] private float _hpBarSpeed = 3f;
    private float _targetFill = 1f;

    private bool _playerDetected = false;
    private bool _canDash = true;
    private bool _cooldownDash;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _hpMax = _hp;
        moveDirection = Random.value < 0.5f ? -1 : 1;

        _enemyHp.SetActive(true);
    }

    private void Update()
    {
        Move();

        _targetFill = Mathf.Clamp01(_hp / _hpMax);
        _hpImage.fillAmount = Mathf.Lerp(_hpImage.fillAmount, _targetFill, Time.deltaTime * _hpBarSpeed);

        Damage = _damageAngel;

        if (State == 3)
        {
            gameObject.SetActive(false);
        }
    }

    protected override void Move()
    {
        bool groundAheadRight = Physics2D.Raycast(groundCheckRight.position, Vector2.down, groundCheckDistance, groundLayer);
        Debug.DrawRay(groundCheckRight.position, Vector2.down * groundCheckDistance, Color.blue);

        Vector2 forwardDirection = new Vector2(moveDirection, 0f);
        RaycastHit2D hit = Physics2D.Raycast(forwardCheck.position, forwardDirection, forwardCheckDistance, playerLayer);
        Debug.DrawRay(forwardCheck.position, forwardDirection * forwardCheckDistance, Color.red);

        _playerDetected = hit.collider != null;

        if (!_playerDetected && ((moveDirection == -1 && !groundAheadRight) || (moveDirection == 1 && !groundAheadRight)))
        {
            FlipDirection();
        }

        if (_canDash == true)
        {
            rb.linearVelocity = new Vector2(moveDirection * moveSpeed, rb.linearVelocity.y);
        }
        else if (_canDash == false)
        {
            rb.linearVelocity = new Vector2(moveDirection * dashForce, rb.linearVelocity.y);
        }
        
        if (_playerDetected && _cooldownDash == false)
        {
            StartCoroutine(DashTowardPlayer());
        }
    }

    private void FlipDirection()
    {
        moveDirection *= -1;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * moveDirection;
        transform.localScale = scale;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheckRight != null)
            Gizmos.DrawLine(groundCheckRight.position, groundCheckRight.position + Vector3.down * groundCheckDistance);
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

    IEnumerator DashTowardPlayer()
    {
        _cooldownDash = true;
        if (Random.value < 0.5f)
        {
            _canDash = false;
            yield return new WaitForSeconds(0.5f);
            _canDash = true;
            Debug.Log("dash");
        }
        
        yield return new WaitForSeconds(3f);
        _cooldownDash = false;

    }

    public override void TakeDamage(float Damage)
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

