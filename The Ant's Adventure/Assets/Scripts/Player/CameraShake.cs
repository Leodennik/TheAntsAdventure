using Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance {get; private set; }
    private CinemachineVirtualCamera _virtualCamera;
    private CinemachineBasicMultiChannelPerlin _perlin;
    private float _shakeTime;

    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _perlin = _virtualCamera.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();
        Instance = this;
    }

    public void Shake()
    {
        Shake(100.0f, 0.2f);
    }
    // Update is called once per frame
    public void Shake(float intensity, float time)
    {
        _perlin.m_AmplitudeGain = intensity;
        _shakeTime = time;
    }

    private void Update()
    {
        if (_shakeTime > 0) _shakeTime -= Time.deltaTime;

        if (_shakeTime < 0)
        {
            _perlin.m_AmplitudeGain = 0;
            _shakeTime = 0;
        }
    }

}


