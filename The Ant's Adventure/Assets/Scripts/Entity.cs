using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Entity : MonoBehaviour
{
    protected SpriteRenderer sprite;
    protected Rigidbody2D body;
    protected Collider2D col2D;
    
    protected virtual void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        body   = GetComponent<Rigidbody2D>();
        col2D  = GetComponent<Collider2D>();
    }
}
