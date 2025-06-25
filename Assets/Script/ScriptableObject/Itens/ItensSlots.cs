using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static ItensData;

public class ItensSlots : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private InventoryItem _inventoryItem;

    private string _description;
    private int _typeItem;
    private int _typeEquipment;

    private float _hp;
    private float _strength;
    private float _def;
    private float _speed;

    [SerializeField] private ItensData _itemData;
    private bool _isEquipped;

    public void SetItem(InventoryItem item)
    {
        if (item == null || item.data == null)
        {
            _itemData = null;
            _isEquipped = false;
            return;
        }

        _itemData = item.data;
        _isEquipped = item.isEquipped;

        _description = _itemData.Description;
        _typeItem = (int)_itemData.Type;
        _typeEquipment = (int)_itemData.TypeEquipamente;

        _hp = _itemData.Life;
        _strength = _itemData.Streght;
        _def = _itemData.Def;
        _speed = _itemData.Speed;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_itemData == null) return;

        var player = FindAnyObjectByType<Player>();

        if (_typeItem == 0)
        {
            Debug.Log("Utilizado");
            player.GainHealth(_hp);
            player.RemoveItem(_itemData);
        }
        else if (_typeItem == 1)
        {
            if (!_isEquipped)
            {
                Debug.Log("Equipado");
                player.AddItemEquipament(_itemData);
            }
            else
            {
                Debug.Log("Desequipado");
                player.RemoveItemEquipament(_itemData);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        var image = GetComponent<Image>();
        if (image != null)
        {
            image.color = new Color(1f, 1f, 1f, 0.7f);
        }

        InventoryManager invent = FindAnyObjectByType<InventoryManager>();
        if (invent != null && invent.gameObject.activeSelf)
        {
            invent.ItemDescription.text = _description;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        var image = GetComponent<Image>();
        if (image != null)
        {
            image.color = new Color(1f, 1f, 1f, 1f);
        }

        InventoryManager invent = FindAnyObjectByType<InventoryManager>();
        if (invent != null && invent.gameObject.activeSelf)
        {
            invent.ItemDescription.text = "";
        }
    }
}
