using System;
using UnityEngine;

public abstract class BaseFadeEffect : MonoBehaviour
{
    private enum State { SHOW, FADE }
    private State _state = State.SHOW;
    
    [Header("General")]
    [SerializeField] [Range(0.0f, 1.0f)] protected float value = 1f;
    [SerializeField] protected bool isAutoStart = false;
    [SerializeField] protected bool isDestroyAfterFade = false;
    
    [Header("Show Fade Settings")]
    [SerializeField] protected float showTime = 1.0f;
    [SerializeField] protected float fadeTime = 1.0f;
    
    [Header("Auto Fade")]
    [SerializeField] protected float showTimeInSeconds = 1.0f;
    [SerializeField] protected bool isAutoFadeAfterTime = true;

    private float _targetValue;
    private float _timer;
    
    protected virtual void Awake()
    {
        if (isAutoStart)
        {
            SetFadeTarget(1.0f);
        }
    }

    private void Start()
    {
        UpdateFadeValue(value);
    }

    public void AnimateFade(float value, float target)
    {
        _state = value <= target ? State.SHOW : State.FADE;
        SetFadeValue(value);
        SetFadeTarget(target);
    }

    public void AnimateShow()
    {
        AnimateFade(0, 1);
    }

    public void AnimateFade()
    {
        AnimateFade(1, 0);
    }
    
    public void AnimateFadeByBullet(Bullet bullet)
    {
        SetColor(bullet.GetColor());
        AnimateFade(1, 0);
    }

    protected virtual void SetColor(Color color)
    {
        // Я знаю что правильно будет сделать еще пару классов, типа PhysicalDamage AcidicDamage, чтобы цвет распространялся только на Acidic уроны.
        // Но мне влом.
    }

    private void Update()
    {
        switch (_state)
        {
            case State.SHOW:
            {
                if (value < _targetValue)
                {
                    float showSpeed = 1.0f / showTime * Time.deltaTime * Time.timeScale;
                    float newAlpha = value + showSpeed;
                    if (newAlpha > _targetValue) newAlpha = _targetValue;
                    SetFadeValue(newAlpha);
                }
                
                if (IsFadeTargetReached())
                {
                    if (isAutoFadeAfterTime)
                    {
                        _timer += Time.deltaTime;
                        if (_timer > showTimeInSeconds)
                        {
                            SetFadeTarget(0);
                            _state = State.FADE;
                        }
                    }
                }
                break;
            }
            case State.FADE:
            {
                if (value > _targetValue)
                {
                    float fadeSpeed = 1.0f / fadeTime * Time.deltaTime * Time.timeScale;
                    float newAlpha = value - fadeSpeed;
                    if (newAlpha < _targetValue) newAlpha = _targetValue;
                    SetFadeValue(newAlpha);
                }
                
                if (isDestroyAfterFade && IsFadeTargetReached())
                {
                    Destroy(gameObject);
                }
                
                break;
            }
        }
    }

    private bool IsFadeTargetReached()
    {
        return Math.Abs(value - _targetValue) < 0.001f;
    }

    private void SetFadeValue(float value)
    {
        this.value = Mathf.Clamp(value, 0.0f, 1.0f);
        UpdateFadeValue(this.value);
    }
    
    public abstract void UpdateFadeValue(float value);

    private void SetFadeTarget(float alphaTarget)
    {
        _targetValue = alphaTarget;
        _timer = 0;
    }
}
