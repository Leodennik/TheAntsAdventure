using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    public float _targetWidth = 266.6667f;
    private Camera _camera;
    private CinemachineVirtualCamera _virtualCamera;
    public float additionalSize = 0;

    private void Awake()
    {
        _camera = Camera.main;
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        Instance = this;
    }

    void Update() {
        float unitsPerPixel = _targetWidth / Screen.width;
        float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;

        _virtualCamera.m_Lens.OrthographicSize = desiredHalfHeight + additionalSize;
    }

    public void SetAdditionalSize(float addCameraSize)
    {
        additionalSize = addCameraSize;
    }

    public void SetFollow(Transform followTransform)
    {
        _virtualCamera.Follow = followTransform;
    }
}
