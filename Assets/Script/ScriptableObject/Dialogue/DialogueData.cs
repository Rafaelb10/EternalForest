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

    [SerializeField] public string _nameCharacther;
    [SerializeField] [TextArea(3, 10)] public string[] _word;
    [SerializeField] public TypeNpc _npc;
    [SerializeField] public List<ItensData> _inventoryNPC = new List<ItensData>();
}
