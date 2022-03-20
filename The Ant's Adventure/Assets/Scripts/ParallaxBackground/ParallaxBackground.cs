using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ParallaxBackground : MonoBehaviour
{
    private Camera _camera;
    private Vector3 _startOffset;
    
    private Vector3 _startPosition;
    private ParallaxBackgroundLayer[] _parallaxBackgroundLayers;

    void Start()
    {
        _camera = Camera.main;
        _startPosition = transform.position;
        _startOffset = transform.position - _camera.transform.position;
        
        _parallaxBackgroundLayers = GetComponentsInChildren<ParallaxBackgroundLayer>();
        foreach (ParallaxBackgroundLayer layer in _parallaxBackgroundLayers)
        {
            layer.Init(gameObject);
        }
    }

    void LateUpdate()
    {
        transform.position = _camera.transform.position + _startOffset;
        float deltaPositionX = _startPosition.x - transform.position.x;
        UpdateLayerPositions(deltaPositionX);
    }

    private void UpdateLayerPositions(float parallaxFactor)
    {
        _parallaxBackgroundLayers = GetComponentsInChildren<ParallaxBackgroundLayer>();
        foreach (ParallaxBackgroundLayer layer in _parallaxBackgroundLayers)
        {
            layer.UpdateParallaxPosition(parallaxFactor);
        }
    }
}
