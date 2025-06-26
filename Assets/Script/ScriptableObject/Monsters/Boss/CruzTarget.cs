using System.Collections;
using UnityEngine;

public class CruzTarget : MonoBehaviour, IDamageable
{
    private Transform _target;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private BossFinal _boss;
    private float _damage;
    private float _hp = 100;

    private void Start()
    {
        _damage = _boss.Damage;
        StartCoroutine(Despaw());
    }

    void Update()
    {
        if (_target == null) return;

        Vector3 direction = (_target.position - transform.position).normalized;
        transform.position += direction * _speed * Time.deltaTime;
    }


    public void SetTarget(Transform target)
    {
        _target = target;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<Player>(out var player))
        {
            FindAnyObjectByType<Player>().TakeDamage(_damage/2);
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<Player>(out var player))
        {
            FindAnyObjectByType<Player>().TakeDamage(_damage/2);
        }
    }

    IEnumerator Despaw()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }

    public void TakeDamage(float Damage)
    {
        _hp = _hp - Damage;

        if (_hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}