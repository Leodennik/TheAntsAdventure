using UnityEngine;

[RequireComponent(typeof(BaseFadeEffect))]
public class PlayerJaws : Jaws
{
    private Animator _animator;
    private BaseFadeEffect _baseFadeEffect;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _baseFadeEffect = GetComponent<BaseFadeEffect>();
        _baseFadeEffect.UpdateFadeValue(0);
    }

    public override void Snap(GameObject creator)
    {
        base.Snap(creator);
        
        _baseFadeEffect.AnimateFade(1.0f, 0.0f);
        _animator.Play("PlayerBite", 0, 0);
    }


}
