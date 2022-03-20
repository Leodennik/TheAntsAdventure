using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSleep : MonoBehaviour
{
    [SerializeField] private float sleepTimeInSeconds = 3.0f;
    private Collider2D   _collider;

    private Player       _player;
    private PlayerStats  _playerStats;
    private UIFadeToBlack _fadeToBlack;   
    private bool         _isCanSleep;
    private bool         _isSleeping;
    private Bed          _bed;   

    private LevelSystem    _levelSystem;
    private ContactFilter2D _contactFilter2D;
    
    // Start is called before the first frame update
    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        
        _player         = FindObjectOfType<Player>(true);
        _playerStats    = _player.GetComponent<PlayerStats>();
        
        _fadeToBlack = UIFadeToBlack.Instance;
        _levelSystem    = FindObjectOfType<LevelSystem>(true);
        _bed = FindObjectOfType<Bed>(true);
        
        _contactFilter2D = new ContactFilter2D();
        _contactFilter2D.layerMask = MaskContainer.maskBed.value;
        
        UIHealthPanel uiHealthPanel = FindObjectOfType<UIHealthPanel>(true);
        uiHealthPanel.gameObject.SetActive(true);
    }

    public IEnumerator ProcessSleep()
    {
        SetSleep();
        
        yield return new WaitForSeconds(sleepTimeInSeconds+_fadeToBlack.getFadeTime());

        _player.LevelUp();
        _levelSystem.NextLevel();
        _fadeToBlack.ReturnToNormal();
        
        yield return new WaitForSeconds(_fadeToBlack.getFadeTime());
        
        WakeUp();
    }
    
    public void CanSleep(bool isCanSleep)
    {
      _isCanSleep = isCanSleep;
    }
        
    public bool IsSleeping()
    {
        return _isSleeping;
    }

    public void SetSleep()
    {
        _isSleeping = true;
        _player.SetSleepPosition();
        _fadeToBlack.FadeToBlack();
    }

    public void ReSpawn()
    {
        StartCoroutine(ProcessReSpawn());
    }
    
    private IEnumerator ProcessReSpawn()
    {
        SetSleep();
        
        yield return new WaitForSeconds(sleepTimeInSeconds+_fadeToBlack.getFadeTime());
        _player.transform.position = _bed.transform.position;
        _playerStats.SetLevel(_playerStats.GetLevel());
        
        yield return new WaitForSeconds(1.0f);
        
        _fadeToBlack.ReturnToNormal();
        
        yield return new WaitForSeconds(_fadeToBlack.getFadeTime());
        
        WakeUp();
    }
    
    public void WakeUp()
    {
        _isSleeping = false;
        _player.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    private void Update()
    {
        if (IsSleeping() || !_isCanSleep) return;

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (LevelSystem.IsGameFinished())
            {
                SetSleep();
                MenuManager.Instance.GameEnd(LevelSystem.GetGameEnding());
            }
            else
            {
                StartCoroutine(ProcessSleep());
            }
        }
    }
}
