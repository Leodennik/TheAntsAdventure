using UnityEngine;

public class DissolveEffect : BaseFadeEffect
{
    private Material _material;
    [SerializeField] float emission = 10;
    
    protected override void Awake()
    {
        base.Awake();
        _material = GetComponent<SpriteRenderer>().material;
    }

    protected override void SetColor(Color color)
    {
        _material.SetColor("_DissolveColor", color);
        _material.SetFloat("_Emission", emission);
    }

    public override void UpdateFadeValue(float value)
    {
        _material.SetFloat("_DissolveFactor", this.value);
    }
}
