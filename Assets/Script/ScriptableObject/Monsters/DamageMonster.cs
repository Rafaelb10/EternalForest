using UnityEngine;

public class DamageMonster : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Player>(out var player))
        {
            FindAnyObjectByType<Player>().TakeDamage(FindAnyObjectByType<Monster>().Strenght);
            Destroy(gameObject);
        }
        Destroy(gameObject);
    }
}
