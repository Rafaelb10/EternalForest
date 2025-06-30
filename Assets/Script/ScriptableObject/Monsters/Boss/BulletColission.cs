using System.Collections;
using UnityEngine;

public class BulletColission : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private BossFinal _boss;

    private Rigidbody2D rb;
    private float _damage;

    private void Start()
    {
        _damage = _boss.Damage;
        rb = GetComponent<Rigidbody2D>();

        Vector2 initialDirection = Random.insideUnitCircle.normalized;
        rb.linearVelocity = initialDirection * speed;

        StartCoroutine(Despawn());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<Player>(out var player))
        {
            player.TakeDamage(_damage);
        }

        Vector2 reflectDir = Vector2.Reflect(rb.linearVelocity.normalized, collision.contacts[0].normal);

        float randomAngle = Random.Range(-30f, 30f);
        reflectDir = Quaternion.Euler(0, 0, randomAngle) * reflectDir;

        if (Random.value < 0.2f)
        {
            reflectDir = Random.insideUnitCircle.normalized;
        }

        rb.linearVelocity = reflectDir.normalized * speed;
    }

    private IEnumerator Despawn()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}