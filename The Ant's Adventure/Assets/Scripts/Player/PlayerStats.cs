using System;
using Cinemachine;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private CinemachineVirtualCamera _virtualCamera;
    private Player _player;
    private int _level;
    
    private Health  _health;
    private Energy _energy;
    private Hunger  _hunger;

    [SerializeField] private float restoreEnergyDelay = 2.0f;
    [SerializeField] private StatsPerLevel[] arrayStatsPerLevel;

    private float _restoreEnergyTimer;

    private bool _isCanSpit;
    private bool _isCanShoot;
    
    
    public void Init(Player player)
    {
        _player = player;
        _virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        
        _health  = _player.GetComponent<Health>();
        _energy  = _player.GetComponent<Energy>();
        _hunger  = _player.GetComponent<Hunger>();

        _health.SetUI(FindObjectOfType<UIPlayerHealth>(true));
        _energy.SetUI(FindObjectOfType<UIPlayerEnergy>(true));
        _hunger.SetUI(FindObjectOfType<UIPlayerHunger>(true));
        
        if (arrayStatsPerLevel.Length == 0)
        {
            FixEmptyStats();
        }
        
        SetLevel(0);
    }

    private void Update()
    {
        _restoreEnergyTimer += Time.deltaTime;
        if (_restoreEnergyTimer >= restoreEnergyDelay)
        {
            _energy.AddValue(1);
            _restoreEnergyTimer = 0;
        }
    }

    private void FixEmptyStats()
    {
        arrayStatsPerLevel = new StatsPerLevel[1];
        arrayStatsPerLevel[0] = new StatsPerLevel(5, 5, 5, 1, 1.0f, 180, 320, 100);
    }

    public void LevelUp()
    {
        if (!IsMaxLevel())
        {
            SetLevel(_level+1);
        }
    }

    public int GetLevel()
    {
        return _level;
    }

    public void SetLevel(int level)
    {
        _level = level;
        _health.SetMaxValue(arrayStatsPerLevel[_level].maxHealth);
        _energy.SetMaxValue(arrayStatsPerLevel[_level].maxStamina);
        _hunger.SetMaxValue(arrayStatsPerLevel[_level].maxHunger);
            
        _virtualCamera.m_Lens.OrthographicSize = arrayStatsPerLevel[_level].cameraSize;

        float newSize = arrayStatsPerLevel[_level].playerSize;
        _player.SetLocalScale(newSize, newSize);
        
        _health.SetValue(_health.GetMaxValue());
        _energy.SetValue(_energy.GetMaxValue());
        _hunger.SetValue(0);
    }

    public int getHealth() { return _health.GetValue();  }
    public int getEnergy() { return _energy.GetValue(); }
    public int getHunger() { return _hunger.GetValue();  }
    
    public void addHealth(int value) { _health.AddValue(value);  }
    public void addEnergy(int value) { _energy.AddValue(value); }
    public void addHunger(int value) { _hunger.AddValue(value);  }
    
    public float getMoveSpeed() { return arrayStatsPerLevel[_level].moveSpeed; }
    public float getJumpForce() { return arrayStatsPerLevel[_level].jumpForce; }
    public float GetSize()      { return arrayStatsPerLevel[_level].playerSize; }
    public int GetDamage()      { return arrayStatsPerLevel[_level].damage; }

    
    public void SetCanSpit(bool value)  { _isCanSpit  = value; }
    public void SetCanShoot(bool value) { _isCanShoot = value; }
    
    public bool IsCanSpit()             { return _isCanSpit && getEnergy() > 0; }
    public bool IsCanShoot()            { return _isCanShoot && getEnergy() > 0;; }

    [Serializable]
    public struct StatsPerLevel
    {
        public int   maxHealth;
        public int   maxStamina;
        public int   maxHunger;
        public int   damage;
        public float playerSize;
        public float moveSpeed;
        public float jumpForce;
        public float cameraSize;

        public StatsPerLevel(int maxHealth, int maxStamina, int maxHunger, int damage, float playerSize, float moveSpeed, float jumpForce, float cameraSize)
        {
            this.maxHealth  = maxHealth;
            this.maxStamina = maxStamina;
            this.maxHunger  = maxHunger;
            this.damage     = damage;
            this.playerSize = playerSize;
            this.moveSpeed  = moveSpeed;
            this.jumpForce  = jumpForce;
            this.cameraSize = cameraSize;
        }
    }

    public bool IsMaxLevel()
    {
        return _level == arrayStatsPerLevel.Length - 1;
    }
    
    public void GiveAbility(SpecialFood.Type ability)
    {
        switch (ability)
        {
            case SpecialFood.Type.SPIT:
            {
                if (_isCanSpit == false)
                {
                    _isCanSpit = true;
                    MenuManager.Instance.ShowAbilityMenu(ability);
                }

                break;
            }
            case SpecialFood.Type.SHOOT:
            {
                if (_isCanShoot == false) 
                {
                    _isCanShoot = true;
                    MenuManager.Instance.ShowAbilityMenu(ability);
                }  
                break;
            }
        }
    }
}

