using UnityEngine;
using Random = UnityEngine.Random;

public class Bullet : Entity
{
    [SerializeField] private float lifeTime = 10.0f;
    [SerializeField] private float minSpeed = 200.0f;
    [SerializeField] private float maxSpeed = 200.0f;
    [SerializeField] private float spread = 2.0f;
    
    protected Color32 color = Color.white;
    protected GameObject creator;
    protected DamageType damageType;
    
    private int _damage = 1;
    private float timer = 0;

    protected override void Awake()
    {
        base.Awake();
        damageType = DamageType.PHYSICAL;
    }

    public void Create(GameObject creator, Vector2 direction, int damage)
    { 
        this.creator = creator;
        float speed = Random.Range(minSpeed, maxSpeed);
        
        //Quaternion quaternionSpread = Quaternion.Euler(0, 0, Random.Range(-spread, spread));
        Vector2 spreadDelta = new Vector2(Random.Range(-spread, spread), Random.Range(-spread, spread));
        
        body.AddForce((direction * speed) + spreadDelta, ForceMode2D.Impulse);
        _damage = damage;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    public void SetLifeTime(float lifeTime)
    {
        this.lifeTime = lifeTime;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (creator == other.gameObject || other.CompareTag("Bullet")) return; // Ignore creator and Bullet
        
        Health otherHealth = other.GetComponentInParent<Health>();
        if (otherHealth != null)
        {
            if (otherHealth.IsAlive() && otherHealth.CanApplyDamage(_damage, this))
            {
                BeforeDestroy();
                Destroy(gameObject);
            }
            
            if (otherHealth.ApplyDamage(_damage, this))
            {
                Damaged(other, _damage, damageType);
            }
        }
        
        int compositeLayerMask = (1 << 18 | 1 << 20); // will hit only layers 18(Glass) and 20(Solid)
        if (col2D.IsTouchingLayers(compositeLayerMask))
        {
            BeforeDestroy();
            Destroy(gameObject);
        }
    }

    protected virtual void Damaged(Collider2D other, int damage, DamageType damageType)
    {
        
    }

    public DamageType GetDamageType()
    {
        return damageType;
    }
    
    public GameObject GetCreator()
    {
        return creator;
    }

    protected virtual void BeforeDestroy() {}

    public virtual void SetColor(Color32 color, float intensity)
    {
        this.color = color;
        sprite.color = color;
    }
    
    public Color GetColor()
    {
        return color;
    }
}
