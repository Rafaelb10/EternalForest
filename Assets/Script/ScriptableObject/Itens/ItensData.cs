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

    public enum TypeEquipament
    {
        Nothing,
        Sword,
        Armor,
        Boots
    }


    private string _uniqueID;
    
    [SerializeField] private int _count;
    [SerializeField] private Sprite _aparence;
    [SerializeField] private string _name;
    [SerializeField] private string _description;

    [SerializeField] private TypeItem _type;
    [SerializeField] private TypeEquipament _typeEquipamente;

    private bool _equipament;

    [SerializeField] private float _streght;
    [SerializeField] private float _def;
    [SerializeField] private float _speed;
    [SerializeField] private float _life;
    [SerializeField] private float _price;

    public string UniqueID { get => _uniqueID; set => _uniqueID = value; }
    public int Count { get => _count; set => _count = value; }
    public string Name { get => _name; set => _name = value; }
    public Sprite Aparence { get => _aparence; set => _aparence = value; }
    public float Price { get => _price; set => _price = value; }
    public string Description { get => _description; set => _description = value; }
    public TypeItem Type { get => _type; set => _type = value; }
    public TypeEquipament TypeEquipamente { get => _typeEquipamente; set => _typeEquipamente = value; }
    public float Streght { get => _streght; set => _streght = value; }
    public float Def { get => _def; set => _def = value; }
    public float Speed { get => _speed; set => _speed = value; }
    public float Life { get => _life; set => _life = value; }
    public bool Equipament { get => _equipament; set => _equipament = value; }

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
