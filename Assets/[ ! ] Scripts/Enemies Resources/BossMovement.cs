using System.Collections;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float MoveSpeed;
    [SerializeField] private float XOffsetRange;

    [Header("Spawning Settings")]
        [SerializeField] private GameObject[] Creeps;
        [SerializeField] private Transform[] SpawnPoints;
        [SerializeField] private float InitialSpawnRate;
        [SerializeField] private int SpecialMonsterSpawnPercentage;
        [SerializeField] private float SpawnRateDecrease;
        [SerializeField] private float SurvivalTime;
        [SerializeField] private float MinimumSpawnRate;

        private float CurrentSpawnRate;
        private float SpawnTimer;
        private float SurvivalTimer;
        private Vector3 TargetPosition;

        private GameObject player_GO;
        private GameObject player;

    private void Start()
    {
        CurrentSpawnRate = InitialSpawnRate;
        SpawnTimer = 0f;
        SurvivalTimer = 0f;
        SetTargetPosition();
    }

    private void Update()
    {
        MoveLogic();
        HandleSpawning();
        CheckSurvivalTime();
        PlayerDetection();
    }

    private void MoveLogic()
    {
        transform.position = Vector3.MoveTowards(transform.position, TargetPosition, MoveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, TargetPosition) <= 0.25f)
        {
            SetTargetPosition();
        }
    }

    private void SetTargetPosition()
    {
        if (player != null)
        {
            float randomXOffset = Random.Range(-XOffsetRange, XOffsetRange);
            TargetPosition = new Vector3(player.transform.position.x + randomXOffset, transform.position.y, transform.position.z);
        }
        else
        {
            TargetPosition = transform.position;
        }
    }

    private void HandleSpawning()
    {
        SpawnTimer += Time.deltaTime;

        if (SpawnTimer >= CurrentSpawnRate && player != null)
        {
            SpawnCreeps();
            SpawnTimer = 0f;
        }
    }

    private void SpawnCreeps()
    {
        foreach (Transform spawnPoint in SpawnPoints)
        {
            if (SpawnPoints.Length >= 1 && this.gameObject.transform.localScale != Vector3.zero)
            {
                if (spawnPoint == SpawnPoints[0] && Creeps != null)
                {
                    Instantiate(Creeps[0], spawnPoint.position, spawnPoint.rotation);
                }

                if (spawnPoint != SpawnPoints[0] && Creeps != null)
                {
                    int randomNumb = Random.Range(0, 100);
                    if (randomNumb < SpecialMonsterSpawnPercentage)
                    {
                        Instantiate(Creeps[Random.Range(1, Creeps.Length)], spawnPoint.position, spawnPoint.rotation);
                    }
                }
            }
        }
    }

    private void CheckSurvivalTime()
    {
        SurvivalTimer += Time.deltaTime;

        if (SurvivalTimer >= SurvivalTime)
        {
            CurrentSpawnRate = Mathf.Max(MinimumSpawnRate, CurrentSpawnRate - SpawnRateDecrease);
            SurvivalTimer = 0f;
        }
    }

    private void PlayerDetection()
    {
        player_GO = GameObject.FindGameObjectWithTag("Player");

        if (player_GO != null)
        {
            if (player_GO.transform.localScale != Vector3.zero)
            {
                player = player_GO;
            }
            else
            {
                player = null;
            }
        }
    }
}
