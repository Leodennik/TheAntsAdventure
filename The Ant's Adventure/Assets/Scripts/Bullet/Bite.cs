using UnityEngine;

public class Bite : Bullet
{
    protected override void Damaged(Collider2D other, int damage, DamageType damageType)
    {
        ItemFood food = other.GetComponentInParent<ItemFood>();
        if (food != null) // Other is Food
        {
            food.OnEat(creator);
        }
    }
}
