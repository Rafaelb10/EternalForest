using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private GameObject _slotItemPrefab;
    [SerializeField] private Transform _scrollContent;
    [SerializeField] private List<ItensData> _inventoryShop = new List<ItensData>();
    [SerializeField] private float _spacing = 35f;

    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _text;

    private List<GameObject> _instantiatedSlots = new List<GameObject>();

    public void SetDialogue(string name, string text)
    {
        _name.text = name;
        _text.text = text;
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

            Image spriteImage = slot.transform.Find("SpriteItem").GetComponent<Image>();
            TextMeshProUGUI nameText = slot.transform.Find("NameItem").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI priceText = slot.transform.Find("PriceItem").GetComponent<TextMeshProUGUI>();

            spriteImage.sprite = item.Aparence;
            nameText.text = item.Name;
            priceText.text = $"{item.Price:F2} G";
            Itens slotScript = slot.GetComponent<Itens>();
            slotScript.SetItem(item);

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
