using SpriteGlow;
using UnityEngine;

public class AcidBomb : Bullet
{
    [SerializeField] private DeleteAfter destroyEffect;
    [SerializeField] private ParticleEffect particles;

    protected override void Awake()
    {
        base.Awake();
        damageType = DamageType.ACIDIC;
    }
    protected override void BeforeDestroy()
    {
        particles.Detach();

        Vector3 createOffset = body.velocity.normalized * -2;
        Instantiate(destroyEffect, transform.position + createOffset, Quaternion.identity);
    }
    
    public override void SetColor(Color32 color, float intensity)
    {
        base.SetColor(color, intensity);
        SpriteGlowEffect glow = GetComponent<SpriteGlowEffect>();
        glow.GlowColor = color;
        glow.GlowBrightness = intensity;
    }
}
