using UnityEngine;

public class ColliderDetectedBoss: MonoBehaviour
{
    [SerializeField] private BossFinal _enemy;

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

            FindFirstObjectByType<BossFinal>();

            switch (FindFirstObjectByType<BossFinal>().DialogueOne._npc)
            {
                case DialogueData.TypeNpc.Npc:
                    FindFirstObjectByType<BossFinal>().IsTalking = false;
                    FindFirstObjectByType<BossFinal>().UiDialogue.gameObject.SetActive(false);
                    break;
            }

            switch (FindFirstObjectByType<BossFinal>().DialogueTwo._npc)
            {
                case DialogueData.TypeNpc.Npc:
                    FindFirstObjectByType<BossFinal>().IsTalking = false;
                    FindFirstObjectByType<BossFinal>().UiDialogue.gameObject.SetActive(false);
                    break;
            }
        }

    }
}
