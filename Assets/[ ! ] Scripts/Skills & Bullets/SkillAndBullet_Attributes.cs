using System.Collections;
using UnityEngine;

public class SkillAndBullet_Attributes : MonoBehaviour
{
    private AudioSource _audioSource;

    [Header("Normal Bullet")]
        [SerializeField] private bool IsNormalBullet;
        [SerializeField] private int NormalBulletDamage;

    [Header("Explode Skill")]
        [SerializeField] private bool IsSkill_ExplodeBullet;
        [SerializeField] private bool IsExplodeSkill;
        [SerializeField] private int SkillExplodeDamage;
        [SerializeField] private ParticleSystem SkillExplodeEffect;
        
    [Header("Explode Effect")]
        [SerializeField] private bool IsExplodeEffect;
        [SerializeField] private AudioClip ExplodeSound;
        [SerializeField] private int ExplodeDamage;
        [SerializeField] private Collider2D Explode_Collider;   
    
    private void OnEnable()
    {
        _audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    private void Start()
    {
        ExplodeEffect();
    }

    private void ExplodeEffect() 
    {
        if (IsExplodeEffect) {

            _audioSource.PlayOneShot(ExplodeSound);
            Destroy(this.gameObject, 3f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Enemy - Boss") || collision.CompareTag("Enemy - Creep"))
        {
            /************************************************** Explode Bullet ********************************************************/

            if (IsSkill_ExplodeBullet == true)
            {
                ParticleSystem effect = Instantiate(SkillExplodeEffect, transform.position, transform.rotation);
                effect.Play();

                CreepAndObjectHealth target_creep = collision.GetComponent<CreepAndObjectHealth>();
                if (target_creep != null)
                {
                    target_creep.TakeDamage(SkillExplodeDamage);
                }

                BossHealth target_boss = collision.GetComponent<BossHealth>();
                if (target_boss != null)
                {
                    target_boss.TakeDamage(SkillExplodeDamage);
                }

                Destroy(this.gameObject);
            }

            /************************************************** Explode Effect ********************************************************/

            if (IsExplodeEffect == true)
            {
                CreepAndObjectHealth target_creep = collision.GetComponent<CreepAndObjectHealth>();
                if (target_creep != null)
                {
                    target_creep.TakeDamage(ExplodeDamage);
                }

                BossHealth target_boss = collision.GetComponent<BossHealth>();
                if (target_boss != null)
                {
                    target_boss.TakeDamage(ExplodeDamage);
                }

                StartCoroutine(TurnOffExplodeCollider());
            }

            /************************************************** Normal Bullet ********************************************************/

            if (IsNormalBullet == true)
            {
                CreepAndObjectHealth target_creep = collision.GetComponent<CreepAndObjectHealth>();
                if (target_creep != null)
                {
                    target_creep.TakeDamage(NormalBulletDamage);
                }

                BossHealth target_boss = collision.GetComponent<BossHealth>();
                if (target_boss != null)
                {
                    target_boss.TakeDamage(NormalBulletDamage);
                }

                Destroy(this.gameObject);
            }

        }

        if (collision.CompareTag("Player"))
        {
            if (IsExplodeSkill == true)
            {
                ParticleSystem effect = Instantiate(SkillExplodeEffect, transform.position, transform.rotation);
                effect.Play();

                CreepAndObjectHealth target_creep = collision.GetComponent<CreepAndObjectHealth>();
                if (target_creep != null)
                {
                    target_creep.TakeDamage(SkillExplodeDamage);
                }

                BossHealth target_boss = collision.GetComponent<BossHealth>();
                if (target_boss != null)
                {
                    target_boss.TakeDamage(SkillExplodeDamage);
                }

                Destroy(this.gameObject);
            }
        }

        IEnumerator TurnOffExplodeCollider()
        {
            yield return new WaitForSeconds(0.25f);
            Explode_Collider.enabled = false;
        }
    }

}
