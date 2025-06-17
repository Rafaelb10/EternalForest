using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class NpcDialogue : MonoBehaviour
{
    [SerializeField] private DialogueData _dialogue;
    [SerializeField] private DialogueManager _dialogueManager;
    [SerializeField] private ShopManager _shopManager;
    [SerializeField] private GameObject _shop;
    [SerializeField] private GameObject _uiDialogue;

    private int _dialogueIndex = 0;
    private bool _isTalking = false;

    private bool _playerInRange = false;

    void Update()
    {
        if (_playerInRange == true && _isTalking == false && Input.GetKeyDown(KeyCode.Q))
        {
            StartDialogue();
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse1) && _isTalking == true)
        {
            if (_dialogue._npc == DialogueData.TypeNpc.Npc)
            {
                ShowNextDialogueLine();
            }
        }
    }

    void StartDialogue()
    {
        switch (_dialogue._npc)
        {
            case DialogueData.TypeNpc.NpcQuest:
                break;

            case DialogueData.TypeNpc.Npc:
                _uiDialogue.gameObject.SetActive(true);
                _dialogueManager.SetName(_dialogue._nameCharacther);
                _dialogueIndex = 0;
                _isTalking = true;
                ShowNextDialogueLine();
                break;

            case DialogueData.TypeNpc.ShopNpc:
                _isTalking = true;
                Cursor.lockState = CursorLockMode.None;
                _shop.gameObject.SetActive(true);
                _shopManager.UpdateShop(_dialogue._inventoryNPC);
                StartCoroutine(LoopRandomDialogue());
                break;
        }
    }

    private void ShowNextDialogueLine()
    {
        if (_dialogueIndex < _dialogue._word.Length)
        {
            _dialogueManager.SetDialogue(_dialogue._word[_dialogueIndex]);
            _dialogueIndex++;
        }
        else
        {
            _isTalking = false;
            _uiDialogue.gameObject.SetActive(false);
        }
    }

    private IEnumerator LoopRandomDialogue()
    {
        while (_dialogue._npc == DialogueData.TypeNpc.ShopNpc)
        {
            string randomLine = _dialogue._word[Random.Range(0, _dialogue._word.Length)];
            _shopManager.SetDialogue(_dialogue._nameCharacther, randomLine);
            yield return new WaitForSeconds(3f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Player>(out var player))
        {
            Debug.Log("PlayerEnter");
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