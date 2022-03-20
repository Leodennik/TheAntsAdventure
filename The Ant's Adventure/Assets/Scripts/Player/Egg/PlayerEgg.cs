using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerEgg : MonoBehaviour
{
    private Animator _animator;
    private ShakeEffect _shakeEffect;
    private Player _player;
    private int _hatchProgress = 0;
    private int _hatchMaxProgress = 4;

    [SerializeField] private GameObject leftHalf;
    [SerializeField] private GameObject rightHalf;
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.speed = 0;

        _shakeEffect = GetComponent<ShakeEffect>();

        _player = FindObjectOfType<Player>(true);

        CinemachineVirtualCamera virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        virtualCamera.Follow = transform;
    }

    private void Update()
    {
        if (MenuManager.Instance.IsPaused()) return;
        
        if (Input.GetMouseButtonDown(0))
        {
            IncreaseHatchProgress();

            if (_hatchProgress == _hatchMaxProgress)
            {
                Hatch();
            }
            
        }
    }
    
    private void IncreaseHatchProgress()
    {
        _hatchProgress += 1;
        _animator.Play("EggHatch", 0 , (1/(float)_hatchMaxProgress)*_hatchProgress);

        _shakeEffect.Begin();
    }
    
    private void Hatch()
    {
        _player.gameObject.SetActive(true);
        
        Vector3 delta = (transform.right * 5);
        CreateHalf(leftHalf, transform.position - delta);
        CreateHalf(rightHalf, transform.position + delta);
        
        Destroy(gameObject);
    }

    private void CreateHalf(GameObject prefab, Vector3 position)
    {
        Instantiate(prefab,  position, Quaternion.identity);
    }
}
