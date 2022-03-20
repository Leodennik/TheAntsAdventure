using UnityEngine;

public class SpecialFood : ItemFood
{
    public enum Type {SPIT, SHOOT }
    public Type giveAbility;

    public override void OnEat(GameObject eater)
    {
        base.OnEat(eater);
        
        PlayerStats stats = eater.GetComponent<PlayerStats>();
        if (stats != null)
        {
            stats.GiveAbility(giveAbility);
        }
    }
}
