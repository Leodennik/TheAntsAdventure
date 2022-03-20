using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIFadeToBlack : MonoBehaviour
{
    public static UIFadeToBlack Instance {get; private set; }

    private Image _image;

    [SerializeField] private float fadeSpeed = 1.0f;
    
    private void Start()
    {
        _image = GetComponent<Image>();
        Instance = this;
    }
    private void Update()
    {
        //if (Input.GetKeyDown((KeyCode.A)))
        //{
        //    StartCoroutine(ProcessFade(1));
        //}
        //if (Input.GetKeyDown((KeyCode.D)))
        //{
        //    StartCoroutine(ProcessFade(0));
        //}
    }

    public void FadeToBlack()
    {
        StopAllCoroutines();
        StartCoroutine(ProcessFade(1));
    }

    public void ReturnToNormal()
    {
        StopAllCoroutines();
        StartCoroutine(ProcessFade(0));
    }

    public float getFadeTime()
    {
        return 1.0f / fadeSpeed;
    }
    
    public IEnumerator ProcessFade(float targetAlpha)
    {
        Color color = _image.color;

        while (Mathf.Abs(targetAlpha - color.a ) >= 0.1)
        {
            float fadeDirection = Mathf.Sign(targetAlpha - color.a);
            color.a += fadeDirection * Time.unscaledDeltaTime * fadeSpeed;
            _image.color = color;
            yield return null;
        }
        
        color.a = targetAlpha;
        _image.color = color;
    }
}
