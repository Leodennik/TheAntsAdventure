using System.Collections;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance {get; private set; }
    
    private bool _isPaused;
    private GameMenu _activeMenu;
    [SerializeField] private GameMenu uiMenuPause;
    [SerializeField] private GameMenu uiMenuSpit;
    [SerializeField] private GameMenu uiMenuShoot;
    [SerializeField] private GameMenu[] uiMenuGameEnds;

    private Player _playerScript;
    private UIFadeToBlack _fadeToBlack;
    private bool _isMenuGameEnd;

    void Awake() {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 1;
    }
    
    private void Start()
    {
        _playerScript = FindObjectOfType<Player>(true);
        Instance = this;
        _fadeToBlack = UIFadeToBlack.Instance;
        
        ShowMenu(uiMenuPause);
    }

    private void Update()
    {
        if (_isMenuGameEnd) return;
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_activeMenu == null)
            {
                ShowMenu(uiMenuPause);
            }
            else
            {
                Resume();
            }
        }
    }

    private void ShowMenu(GameMenu menu)
    {
        Pause();
        menu.Show();
        _activeMenu = menu;
    }
    
    
    public void Resume()
    {
        Time.timeScale = 1.0f;
        _isPaused = false;
        _playerScript.enabled = true;
        _activeMenu.Hide();
        _activeMenu = null;
    }

    public void ShowAbilityMenu(SpecialFood.Type ability)
    {
        Pause();

        switch (ability)
        {
            case SpecialFood.Type.SPIT:  ShowMenu(uiMenuSpit);  break;
            case SpecialFood.Type.SHOOT: ShowMenu(uiMenuShoot); break;
        }
    }
    
    public void ShowMenuGameEnd(int index)
    {
        _isMenuGameEnd = true;
        Pause();
        ShowMenu(uiMenuGameEnds[index]);
    }
    
    public void Pause()
    {
        Time.timeScale = 0.0f;
        _isPaused = true;
        _playerScript.enabled = false;
    }



    public void Exit()
    {
        Application.Quit();
    }

    public bool IsPaused()
    {
        return _isPaused;
    }

    public void GameEnd(int gameEndIndex)
    {
        StartCoroutine(ProcessGameEnd(gameEndIndex));
    }
    
    public IEnumerator ProcessGameEnd(int gameEndIndex)
    {
        _fadeToBlack.FadeToBlack();
        yield return new WaitForSecondsRealtime(_fadeToBlack.getFadeTime());
        ShowMenuGameEnd(gameEndIndex);
        _fadeToBlack.ReturnToNormal();
    }
}
