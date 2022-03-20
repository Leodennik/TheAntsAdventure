using UnityEngine;
using Random = UnityEngine.Random;

public class RedAnt : Ant
{
    [SerializeField] private int biteDamage = 5;
    [SerializeField] private int spitDamage = 1;
    [SerializeField] private int bombDamage = 10;
    [SerializeField] private float pursueSpeed = 200.0f;
    [SerializeField] private float jumpForce = 520.0f;
    [SerializeField] private float returnSpeed = 240.0f;

    private Player _player;
    private Collider2D _target;
    
    private Health _health;

    private Health _glassHealth;
    private Health _stoneHealth;
    
    public enum State { IDLE, NOTICE_PLAYER, SEE_PLAYER, ATTACK, RETURN,  CONFUSED, ATTACK_GLASS, ESCAPE }
    public State _state;

    public float distanceToBite        = 50;
    public float stopDistanceToPlayer  = 150;
    public float distanceToNotice      = 340;
    public float distanceToAttack      = 250;
    public float maxDistanceToPursue   = 1500;
    public float distanceToWallForJump = 100;
    public float distanceToShootBomb   = 400;
    public float distanceToSpit        = 300;
    public float attackDistanceDelta   = 30;
    
    public float delayBeforeLookAtPlayer = 1.0f;

    private Vector2 _eatAngle = new Vector2(-0.2f, -0.5f);
    
    private float _timer;
    
    private float _timerToShoot;
    public float delayBetweenShoots = 1.0f;

    
    private float _timerToSpit;
    public float delayBetweenSpits = 1f;
    
    private float _timerToBite;
    public float delayBetweenBites = 0.5f;
        
    private Vector3 _startPosition;
    private Vector2 _vectorToTarget;
    private Vector2 _directionToTarget;

    private float _confuseTime = 3.0f;
    
    protected override void Start()
    {
        base.Start();
        SetSize(2f);
        _state = State.IDLE;
        
        _player = FindObjectOfType<Player>(true);
        _target = _player.GetComponent<Collider2D>();
        
        _health = GetComponent<Health>();

        _startPosition = transform.position;

        _stoneHealth = GameObject.FindGameObjectWithTag("TargetStone").GetComponent<Health>();
        _glassHealth = GameObject.FindGameObjectWithTag("TargetGlass").GetComponent<Health>();
    }
    
    private void Update()
    {
        if (_target != null)
        {
            _vectorToTarget = _target.transform.position - transform.position;
            _directionToTarget = _vectorToTarget.normalized;
        }
        
        LogicAI();
    }
    
    public void LogicAI()
    {
        switch (_state)
        {
            case State.IDLE:          { LogicIdle();         break; }
            case State.NOTICE_PLAYER: { LogicNoticePlayer(); break; }
            case State.SEE_PLAYER:    { LogicSeePlayer();    break; }
            case State.ATTACK:        { LogicAttack();       break; }
            case State.RETURN:        { LogicReturn();       break; }
            case State.CONFUSED:      { LogicConfused();     break; }
            case State.ATTACK_GLASS:  { LogicAttackGlass();  break; }
            case State.ESCAPE:        { LogicEscape();       break; }
        }

        if (Input.GetKey(KeyCode.O))
        {
            Jump(jumpForce);
        }
    }

    private void LogicIdle()
    {
        SetEatPosition();
        Move(-1, 0);
        if (_vectorToTarget.magnitude < distanceToNotice)
        {
            _state = State.NOTICE_PLAYER;
        }
        
        _health.SetValue(_health.GetMaxValue());
    }

    private void LogicNoticePlayer()
    {
        _timer += Time.deltaTime;
        if (_timer >= delayBeforeLookAtPlayer)
        {
            _state = State.SEE_PLAYER;

            if (delayBeforeLookAtPlayer > 0)
            {
                delayBeforeLookAtPlayer = 0;
                bubbleMaker.CreateBubbleStop();                
            }
        }
    }

    private void LogicSeePlayer()
    {
        UpdateAngles(_directionToTarget);

        if (_vectorToTarget.magnitude > distanceToNotice)
        {
            _state = State.IDLE;
        }

        if (_vectorToTarget.magnitude < distanceToAttack)
        {
            BeginAttackPlayer();
        }

    }
        
    private void LogicAttack()
    {
        if (_player.IsSleeping())
        {
            _state = State.RETURN;
        }
        
        UpdateAngles(_directionToTarget);
        
        float distanceToPlayer = _vectorToTarget.magnitude;
        float randomRange = Random.Range(-attackDistanceDelta, attackDistanceDelta);
        if (distanceToPlayer > stopDistanceToPlayer + randomRange)
        {
            Move(_directionToTarget.x, pursueSpeed);
        }
        else
        {
            Move(_directionToTarget.x, 0);
        }

        if (CheckForWall())
        {
            Jump(jumpForce);
        }
        
        if (CheckForSpitFromPlayer()) // Отбегать назад, чтобы увернуться
        {
            Move(-_directionToTarget.x, pursueSpeed/2.0f);
            if (Random.Range(0, 100) == 0)
            {
                Jump(jumpForce);
            }
        }

        _timerToBite += Time.deltaTime;
        _timerToSpit += Time.deltaTime;
        _timerToShoot += Time.deltaTime;
        
        float shootModifier = -0.2f + distanceToPlayer / (distanceToShootBomb*2.0f);

        if (distanceToPlayer < distanceToBite)
        {
            Move(-_directionToTarget.x, pursueSpeed);
            Bite();
        }
        
        if (distanceToPlayer < distanceToSpit + randomRange)
        { 
            Spit(shootModifier);
        }
        
        if (distanceToPlayer < distanceToShootBomb + randomRange) 
        { 
            Shoot(shootModifier); 
        }

        
        if (distanceToPlayer > maxDistanceToPursue)
        {
            _state = State.RETURN;
        }

        if (_stoneHealth.GetValue() == 0) // The stone is destroyed
        {
            StopAttack();
        }
    }

    private void LogicReturn()
    {
        Vector2 vectorToStartPosition = _startPosition - transform.position;
        Vector2 directionToStartPosition =vectorToStartPosition.normalized;
        
        UpdateAngles(directionToStartPosition);
        Move(directionToStartPosition.x, returnSpeed);

        if (CheckForWall())
        {
            Jump(jumpForce);
        }
        
        if (vectorToStartPosition.magnitude < 20)
        {
            _state = State.IDLE;
        }

        if (!_player.IsSleeping())
        {
            if (_vectorToTarget.magnitude < distanceToAttack)
            {
                BeginAttackPlayer();
            }
        }
    }
    
    private void LogicConfused()
    {
        _timer += Time.deltaTime;
        if (_timer >= _confuseTime)
        {
            Move(-1, pursueSpeed);
            
            if (CheckForWall())
            {
                Jump(jumpForce);
            }

            if (IsNearGlass())
            {
                Move(-1, 0);
                _timer = 0;
                _state = State.ATTACK_GLASS;
                _glassHealth.SetArmor(_glassHealth.GetArmor()/2);
            }
        }
    }
    
    private void LogicAttackGlass()
    {
        _timer += Time.deltaTime;
        if (_timer >= _confuseTime)
        {
            _timerToBite += Time.deltaTime;
            UpdateAngles(new Vector2(-0.5f, Random.Range(-0.5f, 0.5f)));
            Bite();

            if (!_glassHealth.IsAlive())
            {
                _state = State.ESCAPE;
                _timer = 0;
            }
        }
    }

    private void LogicEscape()
    {
        _timer += Time.deltaTime;
        if (_timer > 1.0f) // Wait for 1 second and move outside. 
        {
            Move(-1, pursueSpeed/2.0f);

            if (_timer > 2.5f) // Move for 1.5 seconds then stop
            {
                Move(_directionToTarget.x, 0);
            }
        }
    }

    private void Bite()
    {
        if (_timerToBite > delayBetweenBites)
        {
            antAttack.Bite(biteDamage);
            _timerToBite = 0;
        }
    }

    private void Spit(float shootModifier)
    {
        if (_timerToSpit > delayBetweenSpits)
        {
            _timerToSpit = 0;
            antAttack.Spit(_directionToTarget + Vector2.up*shootModifier, spitDamage, 3);
        }
    }

    private void Shoot(float shootModifier)
    {
        if (_timerToShoot > delayBetweenShoots)
        {
            _timerToShoot = 0;
            _timerToSpit = 0;
            antAttack.Shoot(_directionToTarget + Vector2.up*(shootModifier + Random.Range(-0.5f, 0.1f)), bombDamage, 2);
        }
    }

    public void BeginAttackPlayer()
    {
        _state = State.ATTACK;
    }

    private void StopAttack()
    {
        _timerToShoot = 0;
        _timerToSpit = 0;
        _timerToBite = 0;
        _state = State.CONFUSED;
        Move(-1, 0);
        bubbleMaker.CreateBubble(BubbleMaker.BubbleType.Confused);

        _player.SetConfused();
        UpdateAngles(new Vector2(-0.5f, 0));

        _health.AddImmune(DamageType.PHYSICAL);
        _health.AddImmune(DamageType.ACIDIC);
        biteDamage = 0;
    }

    private bool CheckForSpitFromPlayer()
    {
        Collider2D collider2D = Physics2D.OverlapCircle(transform.position, 200, MaskContainer.maskSpit);
        if (collider2D != null)
        {
            Bullet bullet = collider2D.GetComponent<Bullet>();
            if (bullet.GetCreator() == _player.gameObject)
            {
                return true;
            }
        }
        
        return false;
    }

    private bool IsNearGlass()
    {
        Collider2D collider2D = Physics2D.OverlapCircle(transform.position, 30, MaskContainer.maskGlass);
        return (collider2D != null);
    }

    private bool CheckForWall()
    {
        Vector2 checkDirection = new Vector2(Mathf.Sign(moveSpeed), 0);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, checkDirection, distanceToWallForJump, MaskContainer.maskSolid.value);
        
        Debug.DrawRay(transform.position, checkDirection*distanceToWallForJump);
        
        return hit.collider;
    }

    private void SetEatPosition()
    {
        UpdateAngles(_eatAngle);
    }
}
