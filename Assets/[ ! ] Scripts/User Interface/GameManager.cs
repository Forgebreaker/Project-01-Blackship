using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private GameObject Player_GO;
    private GameObject Boss_GO;
    [SerializeField] private GameObject[] ShipList;
    [SerializeField] private TMP_Text Increased_Coin;
    [SerializeField] private TMP_Text HealthPercentage;
    [SerializeField] private TMP_Text ClassPercentage;
    [SerializeField] private TMP_Text BossMaxCoin;
    [SerializeField] private GameObject VictoryScreen;
    [SerializeField] private GameObject DefeatedScreen;

    [SerializeField] private GameObject VIP_Notice;
    [SerializeField] private GameObject F2P_Notice;

    private SpaceshipController Player;
    private bool SetupReady = false;

    private void Start()
    {
        VictoryScreen.transform.localScale = Vector3.zero;
        DefeatedScreen.transform.localScale = Vector3.zero;

        VIP_Notice.SetActive(GameDataManager.IsMonthlySubscriptionActive());
        F2P_Notice.SetActive(!GameDataManager.IsMonthlySubscriptionActive());

        Player_GO = GameObject.FindGameObjectWithTag("Player");
        if (Player_GO == null)
        {
            Instantiate(ShipList[GameDataManager.GetSelectedShip()], new Vector3(0, -3.5f, 0), ShipList[GameDataManager.GetSelectedShip()].transform.rotation);
        }

        StartCoroutine(InitializeGame());
    }

    private IEnumerator InitializeGame()
    {
        yield return new WaitForSeconds(1); // Delay to ensure all objects are loaded

        Player_GO = GameObject.FindGameObjectWithTag("Player");
        if (Player_GO != null)
        {
            Player = Player_GO.GetComponent<SpaceshipController>();
            if (Player != null && Player.CurrentHealth > 10)
            {
                SetupReady = true;
                StartCoroutine(GameLoop()); // Start game loop after setup
            }
        }
    }

    private IEnumerator GameLoop()
    {
        while (SetupReady)
        {
            if (Player != null)
            {
                HealthPercentage.text = (Player.CurrentHealth / (float)Player.MaximumHealth) >= 0.5f ? "high hp %" : "low hp %";

                if (Player.CurrentHealth < 1)
                {
                    Debug.Log("Player defeated");
                    GameDataManager.DeactivatePurchasedShip(GameDataManager.GetSelectedShip());
                    DefeatedScreen.transform.localScale = new Vector3(2, 2, 2);
                    StartCoroutine(WaitToLobby());
                    yield break; // Exit loop when game ends
                }
            }

            Boss_GO = GameObject.FindGameObjectWithTag("Enemy - Boss");
            if (Boss_GO != null)
            {
                BossHealth Boss = Boss_GO.GetComponent<BossHealth>();
                if (Boss != null)
                {
                    Increased_Coin.text = $"{Boss.IncreasedCoin}";
                    BossMaxCoin.text = $"{Boss.BossMaxCoin}";
                    ClassPercentage.text = $"{Boss.ClassPercentage}%";

                    if (Boss.CurrentHealth <= 1f || Boss_GO.transform.localScale == Vector3.zero)
                    {
                        Debug.Log("Boss defeated");
                        VictoryScreen.transform.localScale = new Vector3(2, 2, 2);
                        StartCoroutine(WaitToLobby());
                        yield break; // Exit loop when game ends
                    }
                }
            }

            yield return new WaitForSeconds(0.1f); // Small delay to optimize performance
        }
    }

    private IEnumerator WaitToLobby()
    {
        yield return new WaitForSeconds(4);
        VictoryScreen.transform.localScale = Vector3.zero;
        DefeatedScreen.transform.localScale = Vector3.zero;
        SceneManager.LoadScene("Main Lobby");
    }
}
