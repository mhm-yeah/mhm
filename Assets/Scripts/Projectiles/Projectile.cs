using System.Collections;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector] public float speed;
    [HideInInspector] public float damage;
    [HideInInspector] public float lifetime;

    [Header("Visual effects")]
    [SerializeField] private GameObject fireVfx;
    [SerializeField] private GameObject iceVfx;
    [SerializeField] private GameObject lightningVfx;

    [SerializeField] private GameObject fireSynergyVfx;
    [SerializeField] private GameObject iceSynergyVfx;
    [SerializeField] private GameObject lightningSynergyVfx;

    protected bool hasElementalSynergy = false;
    public Utilities.Element element;

    public virtual void Init(Weapon weapon)
    {
        damage = weapon.damage;
        speed = weapon.projectileSpeed;
        lifetime = weapon.projectileLifetime;
        element = weapon.element;
    }

    public virtual void Init(Ability ability)
    {
        damage = ability.currentDamage;
        speed = ability.abilitySpeed;
        lifetime = ability.lifeTime;
        element = ability.element;
    }

    void Start()
    {
        SpawnVFX();
        Invoke(nameof(DestroyProjectile), lifetime);
    }
    //public void FinalizeProjectile()
    //{
    //    SpawnVFX();
    //}

    void DestroyProjectile()
    {
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
    public void SetElementSynergy(bool value)
    {
        hasElementalSynergy = value;
    }

    public void SpawnVFX()
    {
        GameObject vfx = null;
        Debug.Log($"{element} current");
        if (hasElementalSynergy)
        {
            switch (element)
            {
                case Utilities.Element.Lightning:
                    vfx = lightningSynergyVfx;
                    break;
                case Utilities.Element.Fire:
                    vfx = fireSynergyVfx;
                    break;
                case Utilities.Element.Ice:
                    vfx = iceSynergyVfx;
                    break;
                default:
                    vfx = null;
                    break;

            }
        }
        else
        {
            switch (element)
            {
                case Utilities.Element.Lightning:
                    vfx = lightningVfx;
                    break;
                case Utilities.Element.Fire:
                    vfx = fireVfx;
                    break;
                case Utilities.Element.Ice:
                    vfx = iceVfx;
                    break;
                default:
                    vfx = null;
                    break;
            }
        }
        Debug.Log($"{vfx} vfx");
        if (vfx != null)
        {
            Instantiate(vfx, transform.position, transform.rotation, transform); // the issue is w the parent objects
            //Instantiate(vfx, transform.position, Quaternion.identity);
        }
    }
}
