using UnityEngine;
using UnityEngine.UI;
public class ResourcesCollect : MonoBehaviour
{
    [Header("Class Data")]

        [SerializeField] public bool SpecialSkill_SimpleShooting;

        [SerializeField] public bool SpecialSkill_HealClass;
        [SerializeField] public float HealPercentage;

        [SerializeField] public bool SpecialSkill_BulletHell;
        [SerializeField] public int BulletHell_Rows;

    [Header("Movement")]

        private GameObject JoyStick_GO;
        public FixedJoystick JoyStick;
        [SerializeField] public Animator[] MovingFlames;

    [Header("Shoot & Skill")]

        public AudioSource _audioSource;

        [SerializeField] public GameObject[] SkillSpawnPoints;
        [SerializeField] public GameObject Skill_Prefabs;
        [SerializeField] public AudioClip Skill_AudioClip;

        [SerializeField] public GameObject HealingEffect;
        [SerializeField] public AudioClip Healing_AudioClip;

        [SerializeField] public GameObject[] BulletSpawnPoints;
        [SerializeField] public GameObject Bullet_Prefabs;
        [SerializeField] public AudioClip Bullet_AudioClip;

        private GameObject ShootButton_GO;
        public Button ShootButton;
        private GameObject ShootCooldown_GO;
        public Image ShootCooldown;

        private GameObject SkillButton_GO;
        public Button SkillButton;
        private GameObject SkillCooldown_GO;
        public Image SkillCooldown;

    [Header("Health")]

        [SerializeField] public GameObject TakeDamageEffect;
        [SerializeField] public AudioClip TakeDamageEffect_AudioClip;
        [SerializeField] public ParticleSystem ShipExplodeEffect;
        [SerializeField] public AudioClip ShipExplodeEffect_AudioClip;
        private GameObject PlayerHealth_UI;
        public Text PlayerHeath_Text;
        public Image PlayerHeath_Bar;
        
    public void ResourcesGathering()
    {
        _audioSource = this.gameObject.GetComponent<AudioSource>();

        /******************************************************************************************/

        PlayerHealth_UI = GameObject.FindGameObjectWithTag("Player Health Bar");
        if (PlayerHealth_UI != null) 
        { 
            PlayerHeath_Text = PlayerHealth_UI.GetComponentInChildren<Text>();
            Transform healthBarTransform = PlayerHealth_UI.transform.Find("HealthBar"); 
            if (healthBarTransform != null) 
            { 
                PlayerHeath_Bar = healthBarTransform.GetComponent<Image>(); 
            }
        }

        /******************************************************************************************/

        JoyStick_GO = GameObject.FindGameObjectWithTag("JoyStick");

        if (JoyStick_GO != null)
        {
            JoyStick = JoyStick_GO.GetComponent<FixedJoystick>();
        }

        /******************************************************************************************/

        ShootButton_GO = GameObject.FindGameObjectWithTag("Shoot Button");

        if (ShootButton_GO != null)
        {
            ShootButton = ShootButton_GO.GetComponent<Button>();
        }

        ShootCooldown_GO = GameObject.FindGameObjectWithTag("Shoot Cooldown");

        if (ShootCooldown_GO != null)
        {
            ShootCooldown = ShootCooldown_GO.GetComponent<Image>();
        }

        /******************************************************************************************/

        SkillButton_GO = GameObject.FindGameObjectWithTag("Skill Button");

        if (SkillButton_GO != null)
        {
            SkillButton = SkillButton_GO.GetComponent<Button>();
        }

        SkillCooldown_GO = GameObject.FindGameObjectWithTag("Skill Cooldown");

        if (SkillCooldown_GO != null)
        {
            SkillCooldown = SkillCooldown_GO.GetComponent<Image>();
        }

    }
}
