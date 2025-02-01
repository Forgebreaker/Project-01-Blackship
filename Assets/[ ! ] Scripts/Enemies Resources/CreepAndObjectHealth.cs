 using UnityEngine;
using System.Collections;

public class CreepAndObjectHealth : MonoBehaviour
{
    [SerializeField] private int MaximumHealth;
    private Collider2D Creep_Collider;
    private int CurrentHealth;

    private GameObject Player_GO;
    private GameObject Player;
    private bool ExplodedWhenNoPlayer = false;

    private GameObject Boss_GO;
    private GameObject Boss;
    private bool ExplodedWhenNoBoss = false;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip TakeDamageEffect_AudioClip;
    [SerializeField] private GameObject TakeDamageEffect;
    [SerializeField] private AudioClip ExplodeEffect_AudioClip;
    [SerializeField] private ParticleSystem ExplodeEffect;

    private void Start()
    {
        PlayerDetection();
        CurrentHealth = MaximumHealth;
        Creep_Collider = GetComponent<Collider2D>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        HealthSystem();
        PlayerDetection();
        BossDetection();
        
        if (Player == null && !ExplodedWhenNoPlayer)
        {
            TakeDamage(10000);
            ExplodedWhenNoPlayer = true;
        }

        if (Boss == null && !ExplodedWhenNoBoss)
        {
            TakeDamage(10000);
            ExplodedWhenNoBoss = true;
        }
    }

    private void HealthSystem()
    {
        //if (CurrentHealth > 0)
        //{
        //    this.gameObject.transform.localScale = Vector3.one;
        //}

        if (this.gameObject.transform.localScale == Vector3.zero)
        {
            Destroy(this.gameObject, 2.5f);
        }

        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaximumHealth);
    }

    public void TakeDamage(int Damage)
    {
        CurrentHealth -= Damage;

        if (CurrentHealth <= 0)
        {
            ParticleSystem DieEffect = Instantiate(ExplodeEffect, this.gameObject.transform.position, this.gameObject.transform.rotation);
            DieEffect.Play();
            _audioSource.PlayOneShot(ExplodeEffect_AudioClip);
            this.gameObject.transform.localScale = Vector3.zero;
            Creep_Collider.enabled = false;
        }

        if (CurrentHealth > 0)
        {
            TakeDamageEffect.SetActive(true);
            _audioSource.PlayOneShot(TakeDamageEffect_AudioClip);
            StartCoroutine(TakeDamageEffectTurnOff());
        }
    }

    IEnumerator TakeDamageEffectTurnOff()
    {
        yield return new WaitForSeconds(0.5f);
        TakeDamageEffect.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SpaceshipController player = collision.GetComponent<SpaceshipController>();

            if (player != null)
            {
                player.TakeDamage(CurrentHealth);
                TakeDamage(player.CurrentHealth);
            }
        }
    }

    private void PlayerDetection()
    {
        Player_GO = GameObject.FindGameObjectWithTag("Player");

        if (Player_GO != null)
        {
            if (Player_GO.transform.localScale != Vector3.zero)
            {
                Player = Player_GO;
            }
            else
            {
                Player = null;
            }
        }
    }

    private void BossDetection()
    {
        Boss_GO = GameObject.FindGameObjectWithTag("Enemy - Boss");

        if (Boss_GO != null)
        {
            if (Boss_GO.transform.localScale != Vector3.zero)
            {
                Boss = Boss_GO;
            }
            else
            {
                Boss = null;
            }
        }
    }
}
