using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AlphaFadeEffect : BaseFadeEffect
{
    private SpriteRenderer _sprite;

    protected override void Awake()
    {
        base.Awake();
        _sprite = GetComponent<SpriteRenderer>();
    }
    
    public override void UpdateFadeValue(float value)
    {
        _sprite.color = new Color(1f, 1f, 1f, value);
    }
}
