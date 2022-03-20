using System;
using UnityEngine;

public class CowardCaterpillar : Enemy
{
    private Animator animator;
    private enum State { IDLE, PREPARE, RUN }
    private State _state;
    
    private float _sizeX;
    private float _moveSpeed;

    public float fearDistance = 200;
    public float prepareTime = 1.0f;
    public float runSpeed = 1000.0f;
    public float runTime = 0.5f;
    private float _timer = 0;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        _sizeX = Math.Abs(transform.localScale.x);
        _state = State.IDLE;
    }

    protected override void LogicAI()
    {
        base.LogicAI();

        switch (_state)
        {
            case State.IDLE:    { LogicIdle();    break; }
            case State.PREPARE: { LogicPrepare(); break; }
            case State.RUN:     { LogicRunAndDissapear(); break; }
        }
    }

    private void LogicIdle()
    {
        if (target != null)
        {
            double distanceToPlayer = GetDistanceToPlayer(fearDistance);
            if (distanceToPlayer < fearDistance)
            {
                SetDirection(1);
                animator.Play("Prepare");
                _state = State.PREPARE;
            }
        }
    }

    private void LogicPrepare()
    {
        _timer += Time.deltaTime;
        if (_timer >= prepareTime)
        {
            animator.Play("Move");
            _moveSpeed = runSpeed * transform.localScale.x;
            _state = State.RUN;
            _timer = 0;
        }
    }

    private void LogicRunAndDissapear()
    {
        _timer += Time.deltaTime;
        if (_timer >= runTime)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (Math.Abs(body.velocity.magnitude) <= Math.Abs(_moveSpeed))
        {
            body.AddForce(Vector2.right * _moveSpeed * 100.0f);
        }
    }

    private void SetDirection(float direction)
    {
        //_localScale.x = transform.localScale.x * direction;
        SetLocalScale(_sizeX * direction, transform.localScale.y);
    }
}