using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ParallaxBackgroundLayer : MonoBehaviour
{
    private SpriteRenderer _sprite;
    private Vector2 _boundsSize;

    private float _localStartPosition;
    private float _minLocalSwapPosition; // Минимальная граница фона, после которой его надо переместить;
    private float _maxLocalSwapPosition; // Максимальная граница фона, после которой его надо переместить;

    public float parallaxFactorX;
    public float parallaxFactorY;

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();

        _boundsSize = _sprite.bounds.size/transform.lossyScale.x;
        _localStartPosition = transform.localPosition.x;
    }

    public void Init(GameObject parent)
    {
        SetLocalSwapPositions(_localStartPosition - _boundsSize.x,
                              _localStartPosition + _boundsSize.x);

        CreateClone(parent, _minLocalSwapPosition, new Vector2(transform.position.x - _boundsSize.x, transform.position.y));
        CreateClone(parent, _maxLocalSwapPosition, new Vector2(transform.position.x + _boundsSize.x, transform.position.y));
    }

    public void SetLocalStartPosition(float localStartPosition)
    {
        _localStartPosition = localStartPosition;
    }

    public void SetLocalSwapPositions(float minLocalSwapPosition, float maxLocalSwapPosition)
    {
        _minLocalSwapPosition = minLocalSwapPosition;
        _maxLocalSwapPosition = maxLocalSwapPosition;
    }

    private void CreateClone(GameObject parent, float localStartPosition, Vector2 position)
    {
        GameObject sideClone = Instantiate(gameObject, position, Quaternion.identity, parent.transform);
        
        ParallaxBackgroundLayer parallaxBackgroundLayer = sideClone.GetComponent<ParallaxBackgroundLayer>();
        parallaxBackgroundLayer.SetLocalStartPosition(localStartPosition);
        parallaxBackgroundLayer.SetLocalSwapPositions(_minLocalSwapPosition, _maxLocalSwapPosition);
    }



    public void UpdateParallaxPosition(float parallaxPosition)
    {
        float newParallaxPosition = _localStartPosition + parallaxPosition * parallaxFactorX;
        
        float swapDelta = _boundsSize.x * 3;
        float newLocalPosition = _minLocalSwapPosition + Mathf.Repeat(newParallaxPosition, swapDelta);

        transform.localPosition = new Vector2(newLocalPosition, transform.localPosition.y);
    }
}
