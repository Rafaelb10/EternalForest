using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private GameObject _slotItemPrefab;
    [SerializeField] private Transform _scrollContent;
    [SerializeField] private List<ItensData> _inventoryShop = new List<ItensData>();
    [SerializeField] private float _spacing = 10f;

    private List<GameObject> _instantiatedSlots = new List<GameObject>();

    void Start()
    {
        
    }

    public void UpdateShop(List<ItensData> itensList)
    {
        _inventoryShop.Clear();
        _inventoryShop.AddRange(itensList);
        CreateShopUI();
    }

    private void CreateShopUI()
    {
        ClearShopUI();

        for (int i = 0; i < _inventoryShop.Count; i++)
        {
            ItensData item = _inventoryShop[i];
            GameObject slot = Instantiate(_slotItemPrefab, _scrollContent);

            RectTransform slotRect = slot.GetComponent<RectTransform>();
            slotRect.anchoredPosition -= new Vector2(0, _spacing * i);

            Image spriteImage = slot.transform.Find("spriteItem").GetComponent<Image>();
            TextMeshProUGUI nameText = slot.transform.Find("nameItem").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI priceText = slot.transform.Find("priceItem").GetComponent<TextMeshProUGUI>();

            spriteImage.sprite = item.Aparence;
            nameText.text = item.Name;
            priceText.text = $"{item.Price:F2} G";

            _instantiatedSlots.Add(slot);
        }
    }

    public void CloseShop()
    {
        _inventoryShop.Clear();
        ClearShopUI();
        this.gameObject.SetActive(false);
    }

    private void ClearShopUI()
    {
        foreach (GameObject slot in _instantiatedSlots)
        {
            Destroy(slot);
        }
        _instantiatedSlots.Clear();
    }
}
