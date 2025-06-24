using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItensSlots : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private ItensData _item;
    private string _description;
    private int _typeIten;
    private int _typeEquipamente;

    private float _hp;
    private float _strength;
    private float _def;
    private float _speed;

    public void SetItem(ItensData item)
    {
        _item = item;
        _description = item.Description;

        _typeIten = (int)item.Type;
        _typeEquipamente = (int)item.TypeEquipamente;

        _hp = item.Life;
        _strength = item.Streght;
        _def = item.Def;
        _speed = item.Speed;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_typeIten == 0)
        {
            FindFirstObjectByType<Player>().GainHealth(_hp);
        }
        if (_typeIten == 1)
        {
            if (_typeEquipamente == 1)
            {

            }
            else if (_typeEquipamente == 2)
            {

            }
            else if (_typeEquipamente == 3)
            {

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
