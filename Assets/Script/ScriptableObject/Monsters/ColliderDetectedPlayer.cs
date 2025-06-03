using UnityEngine;

public class ColliderDetectedPlayer : MonoBehaviour
{
    [SerializeField] private Monster _enemy;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Player>(out var player))
        {
            _enemy.PlayerInZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<Player>(out var player))
        {
            _enemy.PlayerInZone = false;
        }
    }
}
