using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    [Header("Boss Type")]

        [SerializeField] private bool EasyBoss;
        [SerializeField] private bool MediumBoss;
        [SerializeField] private bool HardBoss;

        public int BossMaxCoin;
        public int ClassPercentage;

    [Header("Health Data")]

        [SerializeField] private int MaximumHealth;
        public int CurrentHealth;
        private Collider2D Boss_Collider;
        private GameObject BossHealth_UI;
        private Text BossHealth_Text;
        private Image BossHealth_Bar;

        private AudioSource _audioSource;
        [SerializeField] private AudioClip TakeDamageEffect_AudioClip;
        [SerializeField] private ParticleSystem TakeDamageEffect;
        [SerializeField] private AudioClip BossExplodeEffect_AudioClip;
        [SerializeField] private ParticleSystem BossExplodeEffect;

    [Header("Coin")]

        public int IncreasedCoin;
        private bool IsIncreased;
        private SpaceshipController Player;

    private void Start()
    {
        IsIncreased = false;
        CurrentHealth = MaximumHealth;
        Boss_Collider = GetComponent<Collider2D>();
        BossHealth_UI = GameObject.FindGameObjectWithTag("Boss Health Bar");
        
        if (BossHealth_UI != null)
        {
            BossHealth_Text = BossHealth_UI.GetComponentInChildren<Text>();
            Transform healthBarTransform = BossHealth_UI.transform.Find("HealthBar");
            if (healthBarTransform != null)
            {
                BossHealth_Bar = healthBarTransform.GetComponent<Image>();
            }
        }
        
        _audioSource = GetComponent<AudioSource>();
        
        UpdateHealthUI();
    }

    private void Update()
    {
        HealthSystem();
    }

    private void HealthSystem()
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaximumHealth);

        GameObject Player_GO = GameObject.FindGameObjectWithTag("Player");
        
        if (Player_GO != null) 
        {
            Player = Player_GO.GetComponent<SpaceshipController>();
        }

        if (CurrentHealth <= 0)
        {
            Boss_Collider.enabled = false;
            
            if (EasyBoss && IsIncreased == false) 
            {
                BossMaxCoin = 15;

                if (Player.Aspirants == true)
                {
                    ClassPercentage = 100;
                    CalculateSystem();
                    IsIncreased = true;
                }

                if (Player.Sergeants == true)
                {
                    ClassPercentage = 25;
                    CalculateSystem();
                    IsIncreased = true;
                }

                if (Player.Captains == true)
                {
                    ClassPercentage = 10;
                    CalculateSystem();
                    IsIncreased = true;
                }
            }
            if (MediumBoss && IsIncreased == false) 
            {
                BossMaxCoin = 30;

                if (Player.Aspirants == true)
                {
                    ClassPercentage = 200;
                    CalculateSystem();
                    IsIncreased = true;
                }

                if (Player.Sergeants == true)
                {
                    ClassPercentage = 100;
                    CalculateSystem();
                    IsIncreased = true;
                }

                if (Player.Captains == true)
                {
                    ClassPercentage = 15;
                    CalculateSystem();
                    IsIncreased = true;
                }
            }

            if (HardBoss && IsIncreased == false)
            {
                BossMaxCoin = 50;

                if (Player.Aspirants == true)
                {
                    ClassPercentage = 300;
                    CalculateSystem();
                    IsIncreased = true;
                }

                if (Player.Sergeants == true)
                {
                    ClassPercentage = 200;
                    CalculateSystem();
                    IsIncreased = true;
                }

                if (Player.Captains == true)
                {
                    ClassPercentage = 100;
                    CalculateSystem();
                    IsIncreased = true;
                }
            }
        }
    }

    private void CalculateSystem() 
    {
        if (GameDataManager.IsMonthlySubscriptionActive() == true)
        {
            if (((float)Player.CurrentHealth / (float)Player.MaximumHealth) >= 0.5f)
            {
                IncreasedCoin = (int)(BossMaxCoin * (Random.Range(0.75f, 1f)) * ClassPercentage / 100 * 2);
                GameDataManager.AddCoins(IncreasedCoin);
            }

            if (((float)Player.CurrentHealth / (float)Player.MaximumHealth) < 0.5f)
            {
                IncreasedCoin = (int)(BossMaxCoin * (0.3f) * ClassPercentage / 100 * 2);
                GameDataManager.AddCoins(IncreasedCoin);
            }
        }
        else
        {
            if (((float)Player.CurrentHealth / (float)Player.MaximumHealth) >= 0.5f)
            {
                IncreasedCoin = (int)(BossMaxCoin * (Random.Range(0.75f, 1f)) * ClassPercentage / 100);
                GameDataManager.AddCoins(IncreasedCoin);
            }

            if (((float)Player.CurrentHealth / (float)Player.MaximumHealth) < 0.5f)
            {
                IncreasedCoin = (int)(BossMaxCoin * (0.3f) * ClassPercentage / 100);
                GameDataManager.AddCoins(IncreasedCoin);
            }
        }
    }

    public void TakeDamage(int Damage)
    {
        if (Player.CurrentHealth >= 1)
        {
            CurrentHealth -= Damage;
        }
        
        UpdateHealthUI();

        if (CurrentHealth <= 0)
        {
            ParticleSystem DieEffect = Instantiate(BossExplodeEffect, this.gameObject.transform.position, this.gameObject.transform.rotation);
            DieEffect.Play();
            _audioSource.PlayOneShot(BossExplodeEffect_AudioClip);
            this.gameObject.transform.localScale = Vector3.zero;
        }

        if (CurrentHealth > 0)
        {
            ParticleSystem HitEffect = Instantiate(TakeDamageEffect, this.gameObject.transform.position, this.gameObject.transform.rotation);
            HitEffect.Play();
            Destroy(HitEffect, 1.5f);
            _audioSource.PlayOneShot(TakeDamageEffect_AudioClip);
        }
    }

    private void UpdateHealthUI()
    {
        BossHealth_Text.text = $"{CurrentHealth}";
        BossHealth_Bar.fillAmount = (float)CurrentHealth / MaximumHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SpaceshipController player = collision.GetComponent<SpaceshipController>();
            
            if (player != null)
            {
                player.TakeDamage(player.MaximumHealth / 2);
            }
        }
    }

}
