using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Glass : MonoBehaviour
{
    [SerializeField] private RenderTexture renderTexture;
    private static readonly int RenderTexture = Shader.PropertyToID("_RenderTexture");

    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material.SetTexture(RenderTexture, renderTexture);
    }
}
