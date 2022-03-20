using System;
using UnityEditor;
using UnityEngine;

public class EnemyCaterpillar : Enemy
{
    private enum State { WANDER, PREPARE, ATTACK, RESTORE }
    private State _state;

    private Animator _animator;
    private Jaws _jaws;
    private float _sizeX;
    
    public float wanderDistance = 200;
    public float attackDistance = 100; // If sees player - stop and prepare
    public float prepareTime = 1.0f;   // Time of preparing to attack
    public float attackTime = 2.0f;    // Time of attacking
    public float restoreTime = 1.0f;   // Restore time after attack

    public float moveSpeed = 50.0f;
    public float wanderSpeed = 50.0f;
    public float attackSpeed = 250.0f;
    
    private Vector3 _startPosition;
    private Vector3 _wanderZone;
    private bool _isStarted = false;
    
    private float _timer = 0;
    
    protected override void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
        _jaws = GetComponentInChildren<Jaws>(true);
        
        _sizeX = Math.Abs(transform.localScale.x);
        _startPosition = transform.position;
        _isStarted = true;
    }
    
    public Vector3 GetWanderZone()
    {
        return _wanderZone;
    }

    private void OnValidate()
    {
        _wanderZone = new Vector3(wanderDistance, 50);
    }

    public Vector3 GetStartPosition()
    {
        if (_isStarted)
        {
            return _startPosition;
        }
        return transform.position;
    }
    
    protected override void LogicAI()
    {
        base.LogicAI();
        
        switch (_state)
        {
            case State.WANDER:  { LogicWander();  break; }
            case State.PREPARE: { LogicPrepare(); break; }
            case State.ATTACK:  { LogicAttack();  break; }
            case State.RESTORE: { LogicRestore(); break; }
        }
    }
    
    private void LogicWander()
    {
        if (IsOutsideZone())
        {
            Vector2 directionToCenter = _startPosition - transform.position;
            moveSpeed = wanderSpeed*Mathf.Sign(directionToCenter.x);
            SetDirection(Mathf.Sign(moveSpeed));
        }

        if (target != null)
        {
            double distanceToPlayer = GetDistanceToPlayer(attackDistance);
            if (distanceToPlayer <= attackDistance)
            {
                moveSpeed = 0;
                SetDirection(Math.Sign(directionToTarget.x));
                _state = State.PREPARE;
                _animator.Play("Prepare");
                _timer = 0;
            }
        }
    }

    private bool IsOutsideZone()
    {
        return (transform.position.x < _startPosition.x - wanderDistance/2.0f) ||
               (transform.position.x > _startPosition.x + wanderDistance/2.0f);
    }

    private void LogicPrepare()
    {
        _timer += Time.deltaTime;
        if (_timer >= prepareTime)
        {
            _state = State.ATTACK;
            _animator.Play("Move");
            _timer = 0;
            moveSpeed = attackSpeed * Mathf.Sign(transform.localScale.x);

            _jaws.Snap(gameObject);
        }
    }
    
    private void LogicAttack()
    {
        _timer += Time.deltaTime;
        
        if (_timer >= attackTime)
        {
            _state = State.RESTORE;
            _timer = 0;
        }
    }
    
    private void LogicRestore()
    {
        _timer += Time.deltaTime;
        moveSpeed *= 0.95f; // Smooth slow down
        if (_timer >= restoreTime)
        {
            _state = State.WANDER;
            moveSpeed = wanderSpeed * Mathf.Sign(transform.localScale.x);
        }
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            moveSpeed = 0;
            _state = State.PREPARE;
            _animator.Play("Prepare");
            _timer = 0;
        }
    }
    
    private void FixedUpdate()
    {
        body.velocity = (Vector2.right * moveSpeed);
    }

    private void SetDirection(float direction)
    {
        //_localScale.x = transform.localScale.x * direction;
        SetLocalScale(_sizeX * direction, transform.localScale.y);
    }
}


/*
// Magic in Editor /////////////////////////////////////////////////////////////////////////////////////////////////////
[CustomEditor(typeof(EnemyCaterpillar))]
public class EnemyCaterpillarEditor: Editor 
{
    private EnemyCaterpillar _caterpillar;
    public void OnSceneGUI () 
    {
        _caterpillar = target as EnemyCaterpillar;
        Handles.color = Color.red;
        Handles.DrawWireCube(_caterpillar.GetStartPosition(), _caterpillar.GetWanderZone());
    }
}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
*/