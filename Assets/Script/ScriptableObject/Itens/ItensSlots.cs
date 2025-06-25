using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItensSlots : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private InventoryItem _inventoryItem;
    private ItensData _itemData;

    private string _description;
    private int _typeItem;
    private int _typeEquipment;

    private float _hp;
    private float _strength;
    private float _def;
    private float _speed;

    private bool _isEquipped;

    public void SetItem(InventoryItem item)
    {
        _inventoryItem = item;

        if (item != null && item.data != null)
        {
            _itemData = item.data;

            _description = _itemData.Description;
            _typeItem = (int)_itemData.Type;
            _typeEquipment = (int)_itemData.TypeEquipamente;

            _hp = _itemData.Life;
            _strength = _itemData.Streght;
            _def = _itemData.Def;
            _speed = _itemData.Speed;

            _isEquipped = _itemData.Equipament;
        }
        else
        {
            _itemData = null;
            _description = "";
            _typeItem = -1;
            _typeEquipment = -1;
            _hp = _strength = _def = _speed = 0;
            _isEquipped = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_itemData == null) return;

        var player = FindAnyObjectByType<Player>();

        if (_typeItem == 0)
        {
            player.GainHealth(_hp);
            player.RemoveItem(_itemData);
        }
        else if (_typeItem == 1)
        {
            if (!_isEquipped)
            {
                player.AddItemEquipament(_itemData);
            }
            else
            {
                player.RemoveItem(_itemData);
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
