using UnityEngine;

public class Level : MonoBehaviour
{
    private int _enemyCount;
    private int _foodCount;

    public void Activate()
    {
        gameObject.SetActive(true);

    }
    
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void AddFoodCount(int count)
    {
        _foodCount += count;
    }
    
    public void AddEnemyCount(int count)
    {
        _enemyCount += count;
    }
    
    public bool IsFinished()
    {
        return _enemyCount == 0 && _foodCount == 0;
    }
}
