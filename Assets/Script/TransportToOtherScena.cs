using UnityEditor;
using UnityEngine;

public class TransportToOtherScena : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private bool _saveThis;
    private bool _playerInRange = false;

    void Update()
    {
        if (_playerInRange == true && Input.GetKeyDown(KeyCode.E))
        {
            TradeScena();
        }        
    }

    void TradeScena()
    {
        FindAnyObjectByType<SaveController>().SavePlayerPosition();
        if (_saveThis == true)
        {
            FindAnyObjectByType<SaveController>().ChangeScena(sceneName);
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Player>(out var player))
        {
            _playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<Player>(out var player))
        {
            Debug.Log("PlayerLeave");
            _playerInRange = false;
        }
    }
}
