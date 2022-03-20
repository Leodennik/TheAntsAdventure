using UnityEngine;

public class AntAttack : MonoBehaviour
{
    private Jaws _jaws;
    private AcidCreator _acidCreator;
    
    private void Awake()
    {
        _jaws = GetComponentInChildren<Jaws>(true);
        _acidCreator = GetComponentInChildren<AcidCreator>(true);
    }
    
    public AcidCreator GetAcidCreator()
    {
        return _acidCreator;
    }

    public void Bite(int damage)
    {
        _jaws.SetDamage(damage);
        _jaws.Snap(gameObject);
    }

    public void Spit(Vector2 direction, int damage, float speedMultiplier)
    {
        _acidCreator.Spit(gameObject, direction*speedMultiplier, damage);
    }
    
    public void Shoot(Vector2 direction, int damage, float speedMultiplier)
    {
        _acidCreator.Shoot(gameObject, direction*speedMultiplier, damage);
    }
}
