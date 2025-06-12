using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _slots = new List<GameObject>();
    [SerializeField] private Player _inventorySystem;

    void Start()
    {
        UpdateInventoryUI();
    }

    public void UpdateInventoryUI()
    {
        var items = _inventorySystem.Inventory;

        for (int i = 0; i < _slots.Count; i++)
        {
            GameObject slot = _slots[i];
            Image icon = slot.GetComponent<Image>();
            TextMeshProUGUI countText = slot.GetComponentInChildren<TextMeshProUGUI>();

            if (i < items.Count)
            {
                ItensData item = items[i];

                icon.sprite = item.Aparence;
                icon.color = Color.white;

                countText.text = item.Count.ToString();
            }
            else
            {
                icon.sprite = null;
                icon.color = new Color(1, 1, 1, 0);

                countText.text = "";
            }
        }
    }
}
