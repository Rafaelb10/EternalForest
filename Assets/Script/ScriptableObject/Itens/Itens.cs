using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Itens : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private ItensData _item;
    private string _description;

    public void SetItem(ItensData item)
    {
        _item = item;
        _description = item.Description;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!gameObject.activeInHierarchy) return;

        ShopManager shop = FindAnyObjectByType<ShopManager>();
        Player player = FindAnyObjectByType<Player>();

        if (shop != null && shop.gameObject.activeSelf)
        {
            if (player.Money >= _item.Price)
            {
                player.Money -= _item.Price;
                player.AddItem(_item);
                Debug.Log($"Comprou: {_item.Name} por {_item.Price:F2} G");
            }
            else
            {
                Debug.Log("Dinheiro insuficiente para comprar o item.");
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShopManager shop = FindAnyObjectByType<ShopManager>();

        if (shop != null && shop.gameObject.activeSelf)
        {
            shop.SetDiscription(_description);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ShopManager shop = FindAnyObjectByType<ShopManager>();

        if (shop != null && shop.gameObject.activeSelf)
        {
            shop.SetDiscription("");
        }
    }
}