using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : CharacterHealthParameter
{
    [SerializeField] private int armor = 0;
    [SerializeField] private float noDamageTime = 0.0f;
    [SerializeField] private bool isDestroyOnZero = true;
    [SerializeField] private GameObject deadBodyPrefab;
    [SerializeField] private List<DamageType> immuneTo;
    
    [SerializeField] private UnityEvent EventOnTakeDamage;
    [SerializeField] private UnityEvent EventOnDeath;

    private DeathEffect _deathEffect;
    private bool _isNoDamage = false;

    public override void Start()
    {
        base.Start();
        _deathEffect = GetComponent<DeathEffect>();
    }

    public void AddImmune(DamageType damageType)
    {
        if (!immuneTo.Contains(damageType))
        {
            immuneTo.Add(damageType);
        }
    }
    
    public int GetArmor()
    {
        return armor;  
    }
    
    public void SetArmor(int armor)
    {
        this.armor = armor;
    }
    
    public bool CanApplyDamage(int damage, Bullet bullet)
    {
        if (_isNoDamage || damage <= armor || !IsAlive()) return false;
        if (bullet != null && immuneTo.Contains(bullet.GetDamageType())) return false;

        return true;
    }
    
    public bool ApplyDamage(int damage, Bullet bullet)
    {
        if (!CanApplyDamage(damage, bullet)) return false;
        
        AddValue(-(damage-armor));
        if (value == 0)
        {
            Die(bullet);
        }
        
        EventOnTakeDamage.Invoke(); // Сюда можно помещать события, которые будут происходить;

        if (noDamageTime > 0)
        {
            _isNoDamage = true;
            Invoke(nameof(AllowTakeDamage), noDamageTime); // Через одну секунду вызываем StopInvulnerable;
            //if (soundTakeDamage != null) soundTakeDamage.Play();
        }

        return true;
    }

    public bool IsAlive()
    {
        return value > 0;
    }

    private void AllowTakeDamage()
    {
        _isNoDamage = false;
    }
    
    public void Die(Bullet bullet)
    {
        EventOnDeath.Invoke();

        if (_deathEffect != null && bullet != null)
        {
            _deathEffect.Die(bullet);
        }
        
        if (isDestroyOnZero && !IsAlive()) // check for IsAlive because EventOnDeath can increase health.
        {
            Rigidbody2D body = gameObject.GetComponent<Rigidbody2D>();
            
            if (deadBodyPrefab != null)
            {
                GameObject deadBody = Instantiate(deadBodyPrefab);
                deadBody.transform.position = transform.position;

                if (body != null)
                {
                    Rigidbody2D deadRigidBody = deadBody.GetComponent<Rigidbody2D>();
                    if (deadRigidBody != null)
                    {
                        deadRigidBody.AddForce(body.velocity*body.mass*20);
                    }
                }
            }
            
            Destroy(gameObject);
        }
    }
}
