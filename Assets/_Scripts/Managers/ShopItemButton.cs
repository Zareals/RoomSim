using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemButton : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private Button button;

    private ShopSystem.ShopItem item;
    private ShopSystem shopSystem;

    public void Initialize(ShopSystem.ShopItem shopItem, ShopSystem system)
    {
        item = shopItem;
        shopSystem = system;

        iconImage.sprite = item.itemIcon;
        nameText.text = item.itemName;
        costText.text = $"${item.cost:F2}";

        button.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        shopSystem.TryPurchaseItem(item);
    }
}