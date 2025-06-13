using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueData", menuName = "Scriptable Objects/DialogueData")]
public class DialogueData : ScriptableObject
{
    public enum TypeNpc
    {
        Npc,
        NpcQuest,
        ShopNpc
    }

    [SerializeField] private string _nameCharacther;
    [SerializeField] [TextArea(3, 10)] private string[] _word;
    [SerializeField] private TypeNpc _npc;
    [SerializeField] private List<ItensData> _inventoryNPC = new List<ItensData>();
}
