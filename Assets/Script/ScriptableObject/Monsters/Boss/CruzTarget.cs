using System.Collections;
using UnityEngine;

public class CruzTarget : MonoBehaviour, IDamageable
{
    private Transform _target;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private BossFinal _boss;
    private float _damage;
    private float _hp = 100;

    private Rigidbody2D _rb;

    [SerializeField] private float _attackCooldown = 1f;
    private float _attackTimer = 0f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _damage = _boss.Damage;
        StartCoroutine(Despaw());
    }

    private void FixedUpdate()
    {
        if (_target == null)
        {
            _rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = (_target.position - transform.position).normalized;

        _rb.linearVelocity = direction * _speed;

        transform.up = direction;

        if (_attackTimer > 0)
        {
            _attackTimer -= Time.fixedDeltaTime;
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<Player>(out var player))
        {
            TryAttack(player);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<Player>(out var player))
        {
            TryAttack(player);
        }
    }

    private void TryAttack(Player player)
    {
        if (_attackTimer <= 0f)
        {
            player.TakeDamage(_damage / 2f);
            _attackTimer = _attackCooldown;
        }
    }

    private IEnumerator Despaw()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        _hp -= damage;

        if (_hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}