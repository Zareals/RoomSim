using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopSystem : MonoBehaviour
{
    [System.Serializable]
    public class ShopItem
    {
        public string itemName;
        public GameObject itemPrefab;
        public Sprite itemIcon;
        public float cost = 100f;
    }

    [Header("Shop Settings")]
    [SerializeField] private List<ShopItem> availableItems = new List<ShopItem>();
    [SerializeField] private Transform itemSpawnPoint;
    [SerializeField] private float spawnOffset = 1f;

    [Header("UI References")]
    [SerializeField] private GameObject shopWindow;
    [SerializeField] private Transform shopContent;
    [SerializeField] private GameObject shopButtonPrefab;

    [Header("Player Currency")]
    [SerializeField] private float playerMoney = 1000f;

    private List<GameObject> spawnedObjects = new List<GameObject>();

    private void Start()
    {
        //UpdateMoneyUI();
        InitializeShop();
        CloseShop();
    }

    private void InitializeShop()
    {
        foreach (Transform child in shopContent)
        {
            Destroy(child.gameObject);
        }

        foreach (ShopItem item in availableItems)
        {
            GameObject buttonObj = Instantiate(shopButtonPrefab, shopContent);
            ShopItemButton button = buttonObj.GetComponent<ShopItemButton>();

            if (button != null)
            {
                button.Initialize(item, this);
            }
        }
    }

    public void TryPurchaseItem(ShopItem item)
    {
        if (playerMoney >= item.cost)
        {
            playerMoney -= item.cost;
            //UpdateMoneyUI();
            SpawnItem(item.itemPrefab);
        }
        else
        {
            Debug.Log("Not enough money!");
            // You could add UI feedback here
        }
    }

    private void SpawnItem(GameObject prefab)
    {
        if (itemSpawnPoint == null)
        {
            Debug.LogError("No spawn point set!");
            return;
        }

        // Calculate spawn position with offset
        Vector3 spawnPosition = itemSpawnPoint.position + 
                               Random.insideUnitSphere * spawnOffset;
        spawnPosition.y = itemSpawnPoint.position.y;

        GameObject newItem = Instantiate(prefab, spawnPosition, Quaternion.identity);
        spawnedObjects.Add(newItem);

        // Optional: Make spawned items interactable
        // IInteractable interactable = newItem.GetComponent<IInteractable>();
        // if (interactable != null)
        // {
        //     interactable.OnApproachStart();
        // }
    }

    public void OpenShop()
    {
        shopWindow.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CloseShop()
    {
        shopWindow.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ToggleShop()
    {
        if (shopWindow.activeSelf)
        {
            CloseShop();
        }
        else
        {
            OpenShop();
        }
    }

    // private void UpdateMoneyUI()
    // {
    //     if (moneyText != null)
    //     {
    //         moneyText.text = $"Money: ${playerMoney:F2}";
    //     }
    // }

    public void AddMoney(float amount)
    {
        playerMoney += amount;
        //UpdateMoneyUI();
    }

    // For testing - call this with a UI button
    public void DebugAddMoney()
    {
        AddMoney(500f);
    }
}