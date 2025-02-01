using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using TMPro;

public class ButtonFunctions : MonoBehaviour
{
    [Header("Main Lobby")]
    [SerializeField] private Button Play_Button;
    [SerializeField] private Button Shop_Button; // this one is also added to the "Ship Selector"
    [SerializeField] private GameObject[] ShipList;
    [SerializeField] private GameObject Notification;

    [Header("Level Select")]
    [SerializeField] private Button HighLevel_Button;
    [SerializeField] private Button MidLevel_Button;
    [SerializeField] private Button LowLevel_Button;

    [Header("General Buttons")]
    [SerializeField] private Button Return_Button;
    [SerializeField] private Button AddGems_Button;
    [SerializeField] private Button AddCoins_Button;
    private int selectedShip;

    [Header("Add Gems Screen")]
    private string Add20Gems_ID = "com.OutbreakCompany.Blackship.20gemspack";
    private string Add50Gems_ID = "com.OutbreakCompany.Blackship.50gemspack";
    private string Add100Gems_ID = "com.OutbreakCompany.Blackship.100gemspack";

    [Header("Subscriptions")]
    private string SubX2CoinsDaily_ID = "com.OutbreakCompany.Blackship.subx2coinsdaily";
    private string SubX2CoinsWeekly_ID = "com.OutbreakCompany.Blackship.subx2coinsweekly";
    private string SubX2CoinsMonthly_ID = "com.OutbreakCompany.Blackship.subx2coinsmonthly";

    [SerializeField] private TMP_Text Subscribe_Notification;
    [SerializeField] private GameObject VIP_Icon;
    [SerializeField] private GameObject DailySubButton;
    [SerializeField] private GameObject WeeklySubButton;
    [SerializeField] private GameObject MonthlySubButton;

    private void Start()
    {
        if (Play_Button != null)
        {
            Play_Button.onClick.AddListener(AbleToPlay);
        }

        if (Shop_Button != null)
        {
            Shop_Button.onClick.AddListener(() => LoadScene("Shop"));
        }

        if (Return_Button != null)
        {
            Return_Button.onClick.AddListener(() => LoadScene("Main Lobby"));
        }

        if (AddGems_Button != null)
        {
            AddGems_Button.onClick.AddListener(() => LoadScene("Add Gems"));
        }

        if (AddCoins_Button != null)
        {
            AddCoins_Button.onClick.AddListener(() => LoadScene("Add Coins"));
        }

        if (HighLevel_Button != null)
        {
            HighLevel_Button.onClick.AddListener(() => EnterRoom("Hard"));
        }

        if (MidLevel_Button != null)
        {
            MidLevel_Button.onClick.AddListener(() => EnterRoom("Medium"));
        }

        if (LowLevel_Button != null)
        {
            LowLevel_Button.onClick.AddListener(() => EnterRoom("Easy"));
        }
    }

    private void Update()
    {
        if (GameDataManager.GetTotalPurchasedShips() == 0)
        {
            selectedShip = -1;
        }

        if (GameDataManager.GetTotalPurchasedShips() > 0)
        {
            selectedShip = GameDataManager.GetSelectedShip();
        }

        if (ShipList != null && ShipList.Length > 0)
        {
            bool validSelection = selectedShip >= 0 && selectedShip < ShipList.Length;

            for (int counter = 0; counter < ShipList.Length; counter++)
            {
                ShipList[counter]?.SetActive(validSelection && counter == selectedShip);
            }

            if (Notification != null)
            {
                Notification.SetActive(!validSelection);
            }
        }

        if (GameDataManager.IsMonthlySubscriptionActive() == true && DailySubButton != null
            && WeeklySubButton != null && MonthlySubButton != null && Subscribe_Notification != null) 
        { 
            DailySubButton.gameObject.SetActive(false);
            WeeklySubButton.gameObject.SetActive(false);
            MonthlySubButton.gameObject.SetActive(false);
            Subscribe_Notification.text = $"X2 Reward Pack Subscribed\n\n{GameDataManager.GetRemainingSubscriptionTime()}";
        }

        if (GameDataManager.IsMonthlySubscriptionActive() != true && DailySubButton != null
            && WeeklySubButton != null && MonthlySubButton != null && Subscribe_Notification != null)
        {
            DailySubButton.gameObject.SetActive(true);
            WeeklySubButton.gameObject.SetActive(true);
            MonthlySubButton.gameObject.SetActive(true);
            Subscribe_Notification.text = "";
        }

        if (VIP_Icon != null) 
        {
            VIP_Icon.SetActive(GameDataManager.IsMonthlySubscriptionActive());
        }
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void AbleToPlay()
    {
        if (GameDataManager.GetTotalPurchasedShips() != 0 && selectedShip >= 0)
        {
            SceneManager.LoadScene("Level Select");
        }
    }

    private void EnterRoom(string Type)
    {
        int choosenumb = Random.Range(1, 3);

        if (choosenumb == 1)
        {
            SceneManager.LoadScene($"{Type} Challenge [1]");
        }

        if (choosenumb == 2)
        {
            SceneManager.LoadScene($"{Type} Challenge [2]");
        }
    }

    public void OnPurchaseComplete(Product product)
    {
        if (product.definition.id == Add20Gems_ID)
        {
            GameDataManager.AddGems(20);
        }

        if (product.definition.id == Add50Gems_ID)
        {
            GameDataManager.AddGems(50);
        }

        if (product.definition.id == Add100Gems_ID)
        {
            GameDataManager.AddGems(100);
        }

        if (product.definition.id == SubX2CoinsDaily_ID)
        {
            GameDataManager.ActivateMonthlySubscription(1); 
        }

        if (product.definition.id == SubX2CoinsWeekly_ID)
        {
            GameDataManager.ActivateMonthlySubscription(7); 
        }

        if (product.definition.id == SubX2CoinsMonthly_ID)
        {
            GameDataManager.ActivateMonthlySubscription(30);
        }
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription reason)
    {
        Debug.Log(reason);
    }
}
