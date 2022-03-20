using UnityEngine;

public class PlayerEggHalf : Entity
{
    private float _alpha = 1.0f;
    private bool isDissappear = false;
    private void Start()
    {
        Invoke(nameof(StartDissappear), 5.0f);
    }

    private void Update()
    {
        if (isDissappear)
        {
            _alpha -= 0.005f;
            sprite.color = new Color(1f,1f,1f, _alpha);
        
            if (_alpha <= 0)
            {
                Destroy(gameObject);
            } 
        }

    }

    private void StartDissappear()
    {
        isDissappear = true;
    }
}
