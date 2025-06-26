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
        Vector2 reflectedVelocity = Vector2.Reflect(rb.linearVelocity, collision.contacts[0].normal);

        float randomSpeedFactor = Random.Range(1.1f, 1.5f); 
        reflectedVelocity *= randomSpeedFactor;

        rb.linearVelocity = reflectedVelocity;

        float randomAngle = Random.Range(-45f, 45f);
        Vector2 randomDirection = Quaternion.Euler(0, 0, randomAngle) * rb.linearVelocity;

        rb.linearVelocity = randomDirection;

        if (Random.value < 0.2f)
        {
            velocity = Random.insideUnitCircle.normalized * speed;
            rb.linearVelocity = velocity; 
        }
    }

    IEnumerator Despaw()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }
}
