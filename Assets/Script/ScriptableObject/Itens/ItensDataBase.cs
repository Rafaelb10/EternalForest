using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Database/ItemDatabase")]
public class ItensDataBase : ScriptableObject
{
    [SerializeField] private List<ItensData> _allItems;

    public List<ItensData> AllItems { get => _allItems; set => _allItems = value; }
}

