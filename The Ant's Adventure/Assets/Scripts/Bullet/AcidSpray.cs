using SpriteGlow;
using UnityEngine;

public class AcidSpray : Bullet
{
    protected override void Awake()
    {
        base.Awake();
        damageType = DamageType.ACIDIC;
    }

    public override void SetColor(Color32 color, float intensity)
    {
        base.SetColor(color, intensity);
        SpriteGlowEffect glow = GetComponent<SpriteGlowEffect>();
        glow.GlowColor = color;
        glow.GlowBrightness = intensity;
    }
}
