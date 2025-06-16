using UnityEngine;
using UnityEngine.EventSystems;

public class Itens : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private ItensData _item;

    public void SetItem(ItensData item)
    {
        _item = item;
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
}