using System;
using UnityEngine;

public class Ant : Character
{
    [SerializeField] protected Transform transformHead;
    [SerializeField] protected Transform transformBody;
    [SerializeField] protected Transform transformLegs;

    [SerializeField] private Collider2D groundChecker;
    
    private Animator _animatorHead;
    private Animator _animatorLegs;

    protected BubbleMaker bubbleMaker;
    protected AntAttack antAttack;

    protected float moveSpeed;

    private float _size;
    private float _angleOffset = 25f;
    private float _rotationSpeed = 4f;
    private bool _isCanJump = true;

    protected virtual void Start()
    {
        bubbleMaker = GetComponentInChildren<BubbleMaker>(true);
        antAttack   = GetComponent<AntAttack>();

        _animatorHead = transformHead.GetComponent<Animator>();
        _animatorLegs = transformLegs.GetComponent<Animator>();
        
        SetSize(transform.localScale.x);
    }

    protected void SetSize(float size)
    {
        _size = size;
        SetLocalScale(_size*Math.Sign(transform.localScale.x), _size);
    }
    
    protected void UpdateAngles(Vector2 direction)
    {
        if (Math.Abs(direction.x) > 0.1)
        {
            SetLocalScale(_size * Mathf.Sign(direction.x), _size);
        }
        
        float scaleX = Math.Sign(transform.localScale.x);
        float angle = (Mathf.Atan2(direction.y * scaleX, direction.x * scaleX) * Mathf.Rad2Deg) + _angleOffset*scaleX;

        // Для мягкости поворота ///////////////////////////////////////////////////////////////////////////////////////
        Quaternion headRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Quaternion bodyRotation = Quaternion.AngleAxis(angle / 4.0f, Vector3.forward);

        transformHead.rotation = Quaternion.Slerp(transformHead.rotation, headRotation, _rotationSpeed * Time.deltaTime);
        transformBody.rotation = Quaternion.Slerp(transformBody.rotation, bodyRotation, _rotationSpeed * Time.deltaTime);
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }

    protected void SetAnimationIdle()
    {
        _animatorLegs.Play("LegsIdle");
        _animatorHead.Play("HeadIdle");
    }
    
    protected void SetAnimationWalk()
    {
        _animatorLegs.Play("LegsWalk");
        _animatorHead.Play("HeadWalk");
    }

    protected void Move(float direction, float speed)
    {
        moveSpeed = direction * speed;

        if (Math.Abs(moveSpeed) > 0.01)
        {
            SetAnimationWalk();
        }
        else
        {
            SetAnimationIdle();
        }
    }
    
    private void FixedUpdate()
    {
        body.velocity = new Vector2(moveSpeed, body.velocity.y);
    }

    protected void Jump(float force)
    {
        if (_isCanJump && Physics2D.OverlapCircle(groundChecker.transform.position, 1f, MaskContainer.maskSolid.value))
        {
            body.AddForce(Vector2.up * force, ForceMode2D.Impulse);
            _isCanJump = false;
            Invoke(nameof(AllowJump), 0.2f);
        }
    }

    private void AllowJump()
    {
        _isCanJump = true;
    }
    
    public void SetSleepPosition()
    {
        transform.localRotation     = Quaternion.Euler(0, 0, -90);
        transformHead.localRotation = Quaternion.Euler(0, 0, 90);
        moveSpeed = 0;
        _animatorLegs.Play("LegsIdle");
        _animatorHead.Play("HeadIdle");
    }

}
