using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _slots = new List<GameObject>();
    [SerializeField] private List<GameObject> _slotsEquipamente = new List<GameObject>();
    [SerializeField] private Player _inventorySystem;

    [SerializeField] private TextMeshProUGUI _itemDescription;

    public TextMeshProUGUI ItemDescription { get => _itemDescription; set => _itemDescription = value; }


    void Update()
    {
        if (_inventorySystem == null)
        {
            _inventorySystem = FindAnyObjectByType<Player>();
        }

        UpdateInventoryUI();
        UpdateEquipamentUI();
    }

    public void UpdateInventoryUI()
    {
        var items = _inventorySystem.Inventory;

        for (int i = 0; i < _slots.Count; i++)
        {
            GameObject slot = _slots[i];
            Image icon = slot.GetComponent<Image>();
            TextMeshProUGUI countText = slot.GetComponentInChildren<TextMeshProUGUI>();
            var itemComponent = slot.GetComponent<ItensSlots>();

            if (i < items.Count)
            {
                InventoryItem inventoryItem = items[i];

                itemComponent.SetItem(inventoryItem);
                icon.sprite = inventoryItem.data.Aparence;
                icon.color = Color.white;

                countText.text = inventoryItem.count > 1 ? inventoryItem.count.ToString() : "";
            }
            else
            {
                itemComponent.SetItem(null);
                icon.sprite = null;
                icon.color = new Color(1, 1, 1, 0);
                countText.text = "";
            }
        }
    }

    public void UpdateEquipamentUI()
    {
        var items = _inventorySystem.InventoryEquiped;

        for (int i = 0; i < _slotsEquipamente.Count; i++)
        {
            GameObject slot = _slotsEquipamente[i];
            Image icon = slot.GetComponent<Image>();
            var itemComponent = slot.GetComponent<ItensSlots>();

            InventoryItem inventoryItem = (i < items.Count) ? items[i] : null;

            if (inventoryItem != null && inventoryItem.data != null)
            {
                itemComponent.SetItem(inventoryItem);
                icon.sprite = inventoryItem.data.Aparence;
                icon.color = Color.white;
            }
            else
            {
                itemComponent.SetItem(null);
                icon.sprite = null;
                icon.color = new Color(1, 1, 1, 0);
            }
        }
    }

}