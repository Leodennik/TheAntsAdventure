using SpriteGlow;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteGlowEffect))]
public class Bed : MonoBehaviour
{
    private Player      _player;
    private PlayerSleep _playerSleep;
    private Collider2D  _playerCollider;
    private Hunger      _playerHunger;
    
    private Collider2D       _collider2D;
    private SpriteGlowEffect _spriteGlow;

    // Start is called before the first frame update
    private void Start()
    {
        _player         = FindObjectOfType<Player>(true);
        _playerCollider = _player.GetComponent<Collider2D>();
        _playerSleep    = _player.GetComponent<PlayerSleep>();
        _playerHunger   = _player.GetComponent<Hunger>();
        
        _collider2D     = GetComponent<Collider2D>();
        _spriteGlow     = GetComponent<SpriteGlowEffect>();
    }

    private void Update()
    {
        _playerSleep.CanSleep(false);
        _spriteGlow.OutlineWidth = 0;

        if (_collider2D.IsTouching(_playerCollider) && LevelSystem.IsAllowToSleep())
        {
            _spriteGlow.OutlineWidth = 1;
            _playerSleep.CanSleep(true);
        }
    }
}
