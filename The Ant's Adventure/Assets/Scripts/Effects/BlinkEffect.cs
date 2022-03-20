using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BlinkEffect : MonoBehaviour
{
    [SerializeField] private float duration = 1.0f;
    public SpriteRenderer[] sprites;

    public void StartBlink()
    {
        StartCoroutine("DoBlinkEffect");
    }
    
    public IEnumerator DoBlinkEffect()
    {
        for (float t = 0; t < duration ; t += Time.deltaTime)
        {
            Color color = new Color(Mathf.Sin(t * 30) * 0.5f + 0.5f, 0, 0);
            SetAllSpritesColor(color);
            
            yield return null; // Корутина прервется, но продолжит выполняться в следующий кадр;
        }

        SetAllSpritesColor(Color.white);
    }

    private void SetAllSpritesColor(Color color)
    {
        foreach (var sprite in sprites)
        {
            sprite.color = color;
        }
    }
}
