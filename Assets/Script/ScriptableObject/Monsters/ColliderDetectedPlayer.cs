using UnityEngine;

public class ColliderDetectedPlayer : MonoBehaviour
{
    [SerializeField] private Monster _enemy;
    private int _state = 0;

    private void Update()
    {
        _state = FindAnyObjectByType<Player>().State;
        if (_state == 1)
        {
            Destroy(gameObject);
        }
        
        transform.position = Vector3.MoveTowards(transform.position, _enemy.transform.position, 3 * Time.deltaTime);
    }

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
