using System;
using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "ItensData", menuName = "Scriptable Objects/ItensData")]
public class ItensData : ScriptableObject
{
    public enum TypeItem
    {
        Consumables,
        Equipament
    }


    private string _uniqueID;
    
    [SerializeField] private int _count;
    [SerializeField] private Sprite _aparence;
    [SerializeField] private string _name;
    [SerializeField] private string _description;

    [SerializeField] private TypeItem _type;

    [SerializeField] private float _streght;
    [SerializeField] private float _def;
    [SerializeField] private float _speed;
    [SerializeField] private float _life;

    public string UniqueID { get => _uniqueID; set => _uniqueID = value; }
    public int Count { get => _count; set => _count = value; }
    public string Name { get => _name; set => _name = value; }
    public Sprite Aparence { get => _aparence; set => _aparence = value; }

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(_uniqueID))
        {
            _uniqueID = Guid.NewGuid().ToString();
        #if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
        #endif
        }
    }
}
