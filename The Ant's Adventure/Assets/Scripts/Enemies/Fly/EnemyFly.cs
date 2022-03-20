using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Flying))]
public class EnemyFly : Enemy
{
    private enum State { IDLE, PURSUE, RETURN, PREPARE, ATTACK }
    private State _state;

    [SerializeField] private int   damage              = 2;
    [SerializeField] private float pursueSpeed         = 100.0f;
    [SerializeField] private float returnSpeed         = 50.0f;
    [SerializeField] private float pursueDistance      = 300; // If sees player - pursue
    [SerializeField] private float stopPursueDistance  = 400; // stop pursue if distance > this
    [SerializeField] private float attackDistance      = 60;  // stop pursue if distance > this
    [SerializeField] private float attackDistanceDelta = 20;  // randomness attack distance
    [SerializeField] private  float delayBetweenShoots = 0.5f;
    
    private Animator _animator;
    private AcidCreator _acidCreator;
    private Flying _flying;
    private float _timerToAttack;
    private bool _isReadyToSpit;

    protected override void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
        _acidCreator = GetComponentInChildren<AcidCreator>(true);
        _flying = GetComponent<Flying>();

        attackDistance += Random.Range(-attackDistanceDelta, attackDistanceDelta); 
        
        SetFlyAnimation();
        _state = State.IDLE;
    }

    public void SetHurtAnimation()
    {
        _animator.Play("Hurt");
        Invoke(nameof(SetFlyAnimation), 0.25f);
    }

    public void SetFlyAnimation()
    {
        _animator.Play("Fly");
    }

    protected override void LogicAI()
    {
        base.LogicAI();
        if (target == null)
        {
            if (_state != State.IDLE)
            {
                _state = State.RETURN;
            }
            return;
        }
        
        switch (_state)
        {
            case State.IDLE:    { LogicIdle();    break; }
            case State.PURSUE:  { LogicPursue();  break; }
            case State.RETURN:  { LogicReturn();  break; }
            case State.PREPARE: { LogicPrepare(); break; }
            case State.ATTACK:  { LogicAttack();  break; }
        }
    }

    private void LogicIdle()
    {
        double distanceToPlayer = GetDistanceToPlayer(pursueDistance);
        if (distanceToPlayer <= pursueDistance && !player.IsSleeping())
        {
            _state = State.PURSUE;
        }
    }
    
    private void LogicPursue()
    {
        if (player.IsSleeping())
        {
            _state = State.IDLE;
        }
        
        Vector2 heightCorrection = Vector2.up * 0.3f;
        Vector2 speed = (directionToTarget+heightCorrection) * pursueSpeed * Time.deltaTime;
        Move(speed);
        
        double distanceToPlayer = GetDistanceToPlayer(stopPursueDistance);
        if (distanceToPlayer > stopPursueDistance)
        {
            _state = State.RETURN;
        }
        else if (distanceToPlayer <= attackDistance)
        {
            _state = State.PREPARE;
            _timerToAttack = delayBetweenShoots; // Instant attack
        }
    }
    
    private void LogicReturn()
    {
        Vector2 directionToStart = (_flying.GetStartPosition() - transform.position);
        Vector2 speed = directionToStart.normalized * returnSpeed * Time.deltaTime;
        Move(speed);
        
        if (directionToStart.magnitude <= _flying.GetFlyZoneRadius()/2.0f)
        {
            _state = State.IDLE;
        }

        LogicIdle();
    }

    private void Move(Vector2 speed)
    {
        transform.Translate(speed);
        transform.localScale = new Vector2(-Math.Sign(speed.x), 1);
    }

    private void LogicPrepare()
    {
        if (player.IsSleeping())
        {
            _state = State.IDLE;
        }
        
        _timerToAttack += Time.deltaTime;
        if (_timerToAttack >= delayBetweenShoots)
        {
            _isReadyToSpit = true;
            _state = State.ATTACK;
            _timerToAttack = 0;
        }
        Move(directionToTarget.normalized*Time.deltaTime);
        
        double distanceToPlayer = GetDistanceToPlayer(pursueDistance);
        float distanceAttackBuffer = (pursueDistance - attackDistance) / 2.0f;
        if (distanceToPlayer > attackDistance+distanceAttackBuffer && distanceToPlayer <= pursueDistance)
        {
            _state = State.PURSUE;
        } 
        else if (distanceToPlayer > pursueDistance)
        {
            _state = State.RETURN;
        }
    }
    
    private void LogicAttack()
    {
        _animator.Play("Attack");
        _state = State.PREPARE;
    }
    
    // Called in animation event
    public void AnimationEventAttack()
    {
        if (_isReadyToSpit)
        {
            float directionCorrection = (0.4f + Random.Range(0.0f, 0.6f)); // To spit higher
            _acidCreator.Spit(gameObject, directionToTarget.normalized+Vector2.up*directionCorrection, damage);
            _isReadyToSpit = false;
        }
    }

    public void AnimationEventFinishAttack()
    {
        _isReadyToSpit = false;
    }
}
