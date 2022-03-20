using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    private static Level[] _levels;
    private static int _currentLevel = 0;
    
    private static bool _gameFinished;
    private static int _gameEnding = -1;

    private void Awake()
    {
        _levels = GetComponentsInChildren<Level>(true);
    }

    public void NextLevel()
    {
        if (IsMaxLevel()) return;

        _levels[_currentLevel].Deactivate();
        _currentLevel += 1;
        _levels[_currentLevel].Activate();
    }

    public static bool IsLevelFinished()
    {
        return _levels[_currentLevel].IsFinished();
    }

    public static bool IsMaxLevel()
    {
        return _currentLevel == _levels.Length-1;
    }

    public static void SetGameEnding(int index)
    {
        _gameEnding = index;
        _gameFinished = true;
    }
    
    public static bool IsGameFinished()
    {
        return _gameFinished;
    }
    
    public static int GetGameEnding()
    {
        return _gameEnding;
    }

    public static void AddFoodCount(int count)
    {
        _levels[_currentLevel].AddFoodCount(count);
    }
    
    public static void AddEnemyCount(int count)
    {
        _levels[_currentLevel].AddEnemyCount(count);
    }


    public static bool IsAllowToSleep()
    {
        return IsGameFinished() || (IsLevelFinished() && !IsMaxLevel()) ;
    }
}