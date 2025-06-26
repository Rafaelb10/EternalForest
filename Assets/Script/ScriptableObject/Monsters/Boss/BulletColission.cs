using System.Collections;
using UnityEngine;

public class BulletColission : MonoBehaviour
{
    private float speed = 20f;
    private Vector2 velocity;
    private Rigidbody2D rb;
    [SerializeField] private BossFinal _boss;
    private float _damage;

    void Start()
    {
        _damage = _boss.Damage;
        rb = GetComponent<Rigidbody2D>();  

        velocity = Random.insideUnitCircle.normalized * speed;

        rb.linearVelocity = velocity;

        StartCoroutine(Despaw());
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<Player>(out var player))
        {
            FindAnyObjectByType<Player>().TakeDamage(_damage);
        }

        Vector2 reflectedDirection = Vector2.Reflect(rb.linearVelocity.normalized, collision.contacts[0].normal);

        float randomAngle = Random.Range(-45f, 45f);
        reflectedDirection = Quaternion.Euler(0, 0, randomAngle) * reflectedDirection;

        rb.linearVelocity = reflectedDirection.normalized * speed;

        if (Random.value < 0.2f)
        {
            Vector2 newDirection = Random.insideUnitCircle.normalized;
            rb.linearVelocity = newDirection * speed;
        }
    }

    IEnumerator Despaw()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }
}
