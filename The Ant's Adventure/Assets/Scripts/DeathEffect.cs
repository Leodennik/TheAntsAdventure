using System;
using UnityEngine;

public class DeathEffect : MonoBehaviour
{
    [Serializable]
    public struct DeathType
    {
        public DamageType damageType;
        public BaseFadeEffect effect;
    }
    [SerializeField] private DeathType[] arrayDeathByBulletType;
    
    public void Die(Bullet bullet)
    {
        Collider2D collider2D = GetComponent<Collider2D>();
        if (collider2D != null)
        {
            collider2D.isTrigger = true;
        }
        
        for (int i = 0; i < arrayDeathByBulletType.Length; i++)
        {
            DeathType deathType = arrayDeathByBulletType[i];
            if (deathType.damageType == bullet.GetDamageType())
            {
                if (deathType.effect != null)
                {
                    deathType.effect.AnimateFadeByBullet(bullet);
                    return;
                }
                break;
            }
        }
        
        // Nothing found
        Destroy(gameObject);
    }
}