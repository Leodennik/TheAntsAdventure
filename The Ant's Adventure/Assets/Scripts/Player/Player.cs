using System;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(AntAttack))]
[RequireComponent(typeof(PlayerSleep))]
public class Player : Ant
{
    private PlayerStats _stats;

    private bool _isStopped;

    private PlayerInput _playerInput;
    private PlayerSleep _playerSleep;
    
            
    private CinemachineVirtualCamera _virtualCamera;
    
    protected override void Start()
    {
        base.Start();

        _stats = GetComponent<PlayerStats>();
        _playerInput = GetComponent<PlayerInput>();
        _playerSleep = GetComponent<PlayerSleep>();

        _virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        _virtualCamera.Follow = antAttack.GetAcidCreator().transform;
        
        _stats.Init(this);
        SetSize(_stats.GetSize());
    }

    public void Update()
    {
        if (_playerSleep.IsSleeping()) return;
        
        Vector2 lookDirection = _playerInput.GetLookDirection();
        UpdateAngles(lookDirection);

        if (!_isStopped)
        {
            if (_playerInput.IsKeyJump())
            {
                Jump(_stats.getJumpForce());
            }

            if (_playerInput.IsKeyBite()) // ЛКМ
            {
                antAttack.Bite(_stats.GetDamage());
            }

            if (_stats.IsCanSpit() && _playerInput.IsKeySpit()) // Fast ПКМ
            {
                antAttack.Spit(lookDirection, _stats.GetDamage(), _stats.GetSize());
                _stats.addEnergy(-1);
            }
        
            if (_stats.IsCanShoot() &&_playerInput.IsKeyShoot()) // Long ПКМ
            {
                antAttack.Shoot(lookDirection, _stats.GetDamage()*5, _stats.GetSize());
                _stats.addEnergy(-1);
            }
        }
        
        float playerSpeed = (_playerSleep.IsSleeping() || _isStopped) ? 0 : _stats.getMoveSpeed();
        Move(_playerInput.Horizontal, playerSpeed);
    }
    
    public void LevelUp()
    {
        _stats.LevelUp();
        SetSize(_stats.GetSize());
    }

    public bool IsSleeping()
    {
        return _playerSleep.IsSleeping();
    }

    public void StopMove(float stopTime)
    {
        if (Math.Abs(stopTime) < 0.001f) return;
        
        _isStopped = true;
        body.velocity = Vector2.zero;
        SetAnimationIdle();

        Invoke(nameof(AllowMove), stopTime);
    }

    public void AllowMove()
    {
        _isStopped = false;
    }

    public void CreateBubble(BubbleMaker.BubbleType type)
    {
        bubbleMaker.CreateBubble(type);
    }

    // Вызывается когда уничтожается камень и красный муравей внезапно прекращает атаку.
    public void SetConfused() 
    {
        CreateBubble(BubbleMaker.BubbleType.ConfusedPlayer);
        StopMove(3.0f);
    }
}
    
