using Cinemachine;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class PlayerInput : MonoBehaviour
{
    private Camera _camera;
    
    [SerializeField] private Transform aimOrigin;
    private Vector2 _lookDirection;

    private float _clickTime;

    public float Vertical { get; private set; }
    public float Horizontal { get; private set; }
    
    public void Start()
    {
        _camera = Camera.main;
    }

    public void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");
        Vertical   = Input.GetAxisRaw("Vertical");

        Vector3 normalizedScreenCursor = _camera.WorldToScreenPoint(aimOrigin.position);
        _lookDirection = (Input.mousePosition - normalizedScreenCursor).normalized;
        
        if (Input.GetMouseButtonDown(1)) // Press right mouse
        {
            _clickTime = Time.time;
        }
    }

    public Vector2 GetLookDirection()
    {
        return _lookDirection;
    }
    
    public bool IsKeyJump()
    {
        return Input.GetKeyDown(KeyCode.W);
    }

    public bool IsKeyBite()
    {
        return Input.GetMouseButtonDown(0);
    }

    public bool IsKeySpit()
    {
        return (Input.GetMouseButtonUp(1) && (Time.time - _clickTime) < 0.5);
    }

    public bool IsKeyShoot()
    {
        return (Input.GetMouseButtonUp(1) && (Time.time - _clickTime) > 0.5);
    }
}
