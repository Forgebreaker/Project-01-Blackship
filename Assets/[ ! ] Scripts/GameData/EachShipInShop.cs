using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EachShipInShop : MonoBehaviour
{
    public int shipId;
    [SerializeField] private Button shipButton;
    [SerializeField] private GameObject shipInfo;
    [SerializeField] private GameObject shipIsLockedIcon;
    [SerializeField] private Button stateButton;
    [SerializeField] private TMP_Text stateButtonText;
    [SerializeField] private Button buyButton;
    [SerializeField] private int shipPrice;

    private static GameObject activeShipInfo;

    [SerializeField] private bool IsVIPShip;
    void Start()
    {
        GameDataManager.Initialize();

        UpdateUI();
        SetupListeners();
    }

    void SetupListeners()
    {
        if (buyButton != null)
            buyButton.onClick.AddListener(BuyShip);
        else
            Debug.LogWarning("Buy Button is not assigned.");

        if (stateButton != null)
            stateButton.onClick.AddListener(SelectShip);
        else
            Debug.LogWarning("State Button is not assigned.");

        if (shipButton != null)
            shipButton.onClick.AddListener(ActivateInfo);
        else
            Debug.LogWarning("Ship Button is not assigned.");
    }

    void UpdateUI()
    {
        if (GameDataManager.GetCoins() == null)
        {
            Debug.LogWarning("GameDataManager not initialized yet.");
            return;
        }

        bool isPurchased = GameDataManager.IsShipPurchased(shipId);
        int selectedShipId = GameDataManager.GetSelectedShip();

        shipIsLockedIcon.SetActive(!isPurchased);
        buyButton.gameObject.SetActive(!isPurchased);
        stateButton.gameObject.SetActive(isPurchased);

        if (isPurchased)
        {
            stateButtonText.text = (selectedShipId == shipId) ? "In Use" : "Select";
        }
    }

    void BuyShip()
    {
        if (GameDataManager.CanSpendCoins(shipPrice) && IsVIPShip == false)
        {
            GameDataManager.PurchaseShip(shipId, shipPrice);
            UpdateUI();
        }
        else
        {
            Debug.LogWarning($"<color=red>Not enough coins to purchase Ship ID: {shipId}</color>");
        }

        if (GameDataManager.CanSpendGems(shipPrice) && IsVIPShip == true)
        {
            GameDataManager.PurchaseVIPShip(shipId, shipPrice);
            UpdateUI();
        }
        else
        {
            Debug.LogWarning($"<color=red>Not enough gems to purchase Ship ID: {shipId}</color>");
        }
    }

    void SelectShip()
    {
        GameDataManager.SetSelectedShip(shipId);
        UpdateAllShipUIs();
    }

    void UpdateAllShipUIs()
    {
        EachShipInShop[] allShipUIs = FindObjectsByType<EachShipInShop>(FindObjectsSortMode.None);

        foreach (var shipUI in allShipUIs)
        {
            shipUI.UpdateUI();
        }
    }

    void ActivateInfo()
    {
        if (activeShipInfo != null && activeShipInfo != shipInfo)
        {
            activeShipInfo.SetActive(false);
        }

        bool isActive = shipInfo.activeSelf;
        shipInfo.SetActive(!isActive);

        if (shipInfo.activeSelf)
        {
            activeShipInfo = shipInfo;
        }
        else if (activeShipInfo == shipInfo)
        {
            activeShipInfo = null;
        }
    }
}
