using UnityEngine;

public class ItemFood : MonoBehaviour
{
    public int amount = 1;

    private void Start()
    {
        LevelSystem.AddFoodCount(1);
    }

    public virtual void OnEat(GameObject eater)
    {
        Hunger characterHunger = eater.GetComponent<Hunger>();
        if (characterHunger != null)
        {
            characterHunger.AddValue(amount);
            if (characterHunger.IsMaxValue())
            {
                Health characterHealth = eater.GetComponent<Health>();
                if (characterHunger != null)
                {
                    characterHealth.AddValue(1);
                }
            }
        }
    }

    private void OnDestroy()
    {
        LevelSystem.AddFoodCount(-1);
    }
}
