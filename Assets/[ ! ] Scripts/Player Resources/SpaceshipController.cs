using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class SpaceshipController : MonoBehaviour
{
    [Header("Ready To Launch ?")]
        private ResourcesCollect _resources;
        private bool SetupCompleted = false;

    [Header("Ship Class")]
        [SerializeField] public bool Aspirants;
        [SerializeField] public bool Sergeants;
        [SerializeField] public bool Captains;

    [Header("Movement Data")]
        [SerializeField] private float OriginalMoveSpeed = 5f;
        private float MoveSpeed = 0;
        private Vector2 MovementDirection = Vector2.zero;
        private float HorizontalInput = 0;
        private float VerticalInput = 0;
        private float MoveLimitPosition_X = 6.5f;
        [SerializeField] private float MoveLimitPosition_Y = 4.25f;

    [Header("Shoot & Skill Data")]

        private GameObject ShootButton_GO;
        private Button ShootButton;
        private GameObject ShootButton_ReloadEffect;

        [SerializeField] private float ShootingCoolDown = 1f;
        private float CurrentShootCoolDown;
        private bool AbleToShoot;
        private bool ShootPressed;

        private GameObject SkillButton_GO;
        private Button SkillButton;
        private GameObject SkillButton_ReloadEffect;

        [SerializeField] private float SkillCoolDown = 5f;
        private float CurrentSkillCoolDown;
        private bool AbleToActiveSkill;
        private bool SkillPressed;

    [Header("Health Data")]

        [SerializeField] public int MaximumHealth;
        public int CurrentHealth;  
        private bool IsAlive;
        private GameObject Boss_GO;

    private void Start()
    {
        if (SetupCompleted == false) 
        {
            MoveSpeed = OriginalMoveSpeed;
            CurrentHealth = MaximumHealth;
            IsAlive = true;

            _resources = this.gameObject.GetComponent<ResourcesCollect>();
            
            if (_resources != null) 
            {
                _resources.ResourcesGathering();
            }
            
            SetupCompleted = true;
        }
    }

    private void Update()
    {
        if (SetupCompleted == false) return;
        
        MovementSystem();
        OnShootButtonDown();
        OnSkillButtonDown();
        HealthSystem();

        Boss_GO = GameObject.FindGameObjectWithTag("Enemy - Boss");
        _resources.PlayerHeath_Text.text = $"{CurrentHealth}";
        _resources.PlayerHeath_Bar.fillAmount = (float)CurrentHealth / (float)MaximumHealth;
    }

    private void MovementSystem() 
    {
        if (_resources.JoyStick != null)
        {
            HorizontalInput = _resources.JoyStick.Horizontal;
            VerticalInput = _resources.JoyStick.Vertical;
        }

        if (Mathf.Abs(HorizontalInput) + Mathf.Abs(VerticalInput) > 0)
        {
            MoveSpeed = 0.70710678118f * OriginalMoveSpeed;
        }
        else
        {
            MoveSpeed = OriginalMoveSpeed;
        }

        HorizontalInput = ConstrainInput(HorizontalInput, transform.position.x, MoveLimitPosition_X);
        VerticalInput = ConstrainInput(VerticalInput, transform.position.y, MoveLimitPosition_Y);

        foreach (Animator MovingFlame in _resources.MovingFlames)
        {
            MovingFlame.SetFloat("Move", Mathf.Abs(HorizontalInput) + Mathf.Abs(VerticalInput));
        }

        MovementDirection = new Vector2(HorizontalInput, VerticalInput);
        transform.Translate(MovementDirection * MoveSpeed * Time.deltaTime);

        float ConstrainInput(float input, float position, float limit)
        {
            if ((position <= -limit && input < 0) || (position >= limit && input > 0))
            {
                return 0;
            }
            return input;
        }
    }


 /************************************************** SHOOT & SKILLS **************************************************/
    private void OnShootButtonDown()
    {
        if (_resources.ShootButton != null)
        {
            _resources.ShootButton.onClick.AddListener(() =>
            {
                if (CurrentShootCoolDown <= 0)
                {
                    ShootPressed = true;
                }
            });
        }

        if (CurrentShootCoolDown > 0)
        {
            AbleToShoot = false;
            CurrentShootCoolDown -= Time.deltaTime;
            _resources.ShootCooldown.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            _resources.ShootCooldown.fillAmount = CurrentShootCoolDown / ShootingCoolDown;
            _resources.ShootCooldown.transform.position = _resources.ShootButton.transform.position;
        }
        else
        {
            _resources.ShootCooldown.transform.localScale = Vector3.zero;
        }

        if (ShootPressed == true && CurrentShootCoolDown <= 0 && CurrentHealth >= 1)
        {
            AbleToShoot = true;
            if (IsAlive)
            {
                if (_resources.BulletSpawnPoints.Length > 0 && _resources.Bullet_Prefabs != null)
                {
                    foreach (GameObject BulletSpawnPoint in _resources.BulletSpawnPoints)
                    {
                        Instantiate(_resources.Bullet_Prefabs, BulletSpawnPoint.transform.position, BulletSpawnPoint.transform.rotation);
                        _resources._audioSource.PlayOneShot(_resources.Bullet_AudioClip);
                    }
                }
            }
            //if (CurrentSkillCoolDown <= 0)
            //{
            //    CurrentSkillCoolDown = ShootingCoolDown;
            //}
            CurrentShootCoolDown = ShootingCoolDown;
            ShootPressed = false;
        }
    }

    private void OnSkillButtonDown()
    {
        if (_resources.SkillButton != null)
        {
            _resources.SkillButton.onClick.AddListener(() =>
            {
                if (CurrentSkillCoolDown <= 0)
                {
                    SkillPressed = true;
                }
            });
        }

        if (CurrentSkillCoolDown > 0)
        {
            AbleToActiveSkill = false;
            CurrentSkillCoolDown -= Time.deltaTime;
            _resources.SkillCooldown.transform.localScale = new Vector3(0.125f, 0.125f, 0.125f);
            _resources.SkillCooldown.fillAmount = CurrentSkillCoolDown / SkillCoolDown;
            _resources.SkillCooldown.transform.position = _resources.SkillButton.transform.position;
        }
        else
        {
            _resources.SkillCooldown.transform.localScale = Vector3.zero;
        }

        if (SkillPressed == true && CurrentSkillCoolDown <= 0 && CurrentHealth >= 1)
        {
            AbleToActiveSkill = true;
            if (IsAlive)
            {
                if (_resources.SpecialSkill_SimpleShooting) { SimpleShootingSkill(); }
                if (_resources.SpecialSkill_HealClass) { HealingPower(); }
                if (_resources.SpecialSkill_BulletHell) { BulletHell(); }
            }
            CurrentSkillCoolDown = SkillCoolDown;
            CurrentShootCoolDown = ShootingCoolDown;
            SkillPressed = false;
        }
    }

    private void SimpleShootingSkill() 
    {
        if (_resources.SkillSpawnPoints.Length > 0 && _resources.Skill_Prefabs != null)
        {
            foreach (GameObject SkillSpawnPoint in _resources.SkillSpawnPoints)
            {
                Instantiate(_resources.Skill_Prefabs, SkillSpawnPoint.transform.position, SkillSpawnPoint.transform.rotation);
                _resources._audioSource.PlayOneShot(_resources.Skill_AudioClip);
            }
        }
    }

    private void HealingPower() 
    {
        if (_resources.HealingEffect != null)
        {
            _resources.HealingEffect.SetActive(true);
            CurrentHealth += (int)(MaximumHealth * (_resources.HealPercentage / 100));
            _resources._audioSource.PlayOneShot(_resources.Healing_AudioClip);
            StartCoroutine(HealingEffectTurnOff());
        }
    }

    private void BulletHell() 
    {
        StartCoroutine(BulletHellCore());
    }

    IEnumerator BulletHellCore()
    {
        if (_resources.SkillSpawnPoints.Length > 0 && _resources.Bullet_Prefabs != null)
        {
            for (int counter = 0; counter < _resources.BulletHell_Rows; counter++)
            {
                foreach (GameObject SkillSpawnPoint in _resources.SkillSpawnPoints)
                {
                    Instantiate(_resources.Bullet_Prefabs, SkillSpawnPoint.transform.position, SkillSpawnPoint.transform.rotation);
                    _resources._audioSource.PlayOneShot(_resources.Bullet_AudioClip);
                }

                yield return new WaitForSeconds(0.15f);
            }
        }
    }

    IEnumerator HealingEffectTurnOff()
    {
        yield return new WaitForSeconds(0.75f);
        _resources.HealingEffect.SetActive(false);
    }

    /************************************************** HEALTH **************************************************/

    private void HealthSystem()
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaximumHealth);
        if (CurrentHealth > 0)
        {
            IsAlive = true;
            this.gameObject.transform.localScale = Vector3.one;
        }
        else
        {
            IsAlive = false;
        }  
    }

    public void TakeDamage(int Damage)
    {
        if (Boss_GO.transform.localScale != Vector3.zero)
        {
            CurrentHealth -= Damage;
        }

        if (CurrentHealth <= 0)
        {
            ParticleSystem effect = Instantiate(_resources.ShipExplodeEffect, transform.position, transform.rotation);
            effect.Play();
            _resources._audioSource.PlayOneShot(_resources.ShipExplodeEffect_AudioClip);
            this.gameObject.transform.localScale = Vector3.zero;
        }

        if (CurrentHealth > 0)
        {
            _resources.TakeDamageEffect.SetActive(true);
            _resources._audioSource.PlayOneShot(_resources.TakeDamageEffect_AudioClip);
            StartCoroutine(TakeDamageEffectTurnOff());
        }
    }

    IEnumerator TakeDamageEffectTurnOff()
    {
        yield return new WaitForSeconds(0.5f);
        _resources.TakeDamageEffect.SetActive(false);
    }
}
