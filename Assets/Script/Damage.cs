using System.Collections;
using UnityEngine;

public class Damage : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector2 pushDirection = (collision.transform.position - FindAnyObjectByType<Player>().transform.position).normalized;
        float pushForce = 5000f;

        if (collision.gameObject.TryGetComponent<Monster>(out var monster))
        {
            monster.TakeDamage(FindAnyObjectByType<Player>().Strenght);
            ResetMovement(collision.attachedRigidbody);
            PushBack(collision.attachedRigidbody, pushDirection, pushForce);
        }
        else if (collision.gameObject.TryGetComponent<MonsterPlataform>(out var monsterPlataform))
        {
            monsterPlataform.TakeDamage(FindAnyObjectByType<Player>().Strenght);
            ResetMovement(collision.attachedRigidbody);
            PushBack(collision.attachedRigidbody, pushDirection, pushForce);
        }
        else if (collision.gameObject.TryGetComponent<BossFinal>(out var bossFinal))
        {
            bossFinal.TakeDamage(FindAnyObjectByType<Player>().Strenght);
            ResetMovement(collision.attachedRigidbody);
            PushBack(collision.attachedRigidbody, pushDirection, pushForce);
        }
        else if (collision.gameObject.TryGetComponent<CruzTarget>(out var cruzTarget))
        {
            cruzTarget.TakeDamage(FindAnyObjectByType<Player>().Strenght);
            ResetMovement(collision.attachedRigidbody);
            PushBack(collision.attachedRigidbody, pushDirection, pushForce);
        }
    }

    private void ResetMovement(Rigidbody2D rb)
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

    private void PushBack(Rigidbody2D rb, Vector2 direction, float force)
    {
        if (rb != null)
        {
            rb.AddForce(direction * force, ForceMode2D.Impulse);
        }
    }
}