using UnityEngine;

public class LabGlow : BaseFadeEffect
{
    private Material _material;
    protected override void Awake()
    {
        base.Awake();
        _material = GetComponent<SpriteRenderer>().material;
    }

    public override void UpdateFadeValue(float value)
    {
        _material.SetFloat("_Intensity", Mathf.Round(this.value*100));
    }
}
