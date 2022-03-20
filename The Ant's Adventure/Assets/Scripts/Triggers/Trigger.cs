using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    protected Player player;
    private Collider2D _playerCollider2D;
    [SerializeField] private float stopTime = 0.0f;

    public UnityEvent OnCollideWithPlayer; 
    private void Start()
    {
        player = FindObjectOfType<Player>(true);
        _playerCollider2D = player.GetComponent<Collider2D>();

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == _playerCollider2D)
        {
            if (stopTime > 0)
            {
                player.StopMove(stopTime);
            }

            if (OnCollideWithPlayer != null)
            {
                OnCollideWithPlayer.Invoke();
            }
            
            CollideWithPlayer();
            Destroy(gameObject);
        }
    }

    protected virtual void CollideWithPlayer()
    {
        
    }
}
