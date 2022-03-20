using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class UIParameterLineBars : MonoBehaviour
{        
    private const int MIN_BARS_COUNT = 2; // Fixed because of image;
    private const int BAR_SIZE = 8;
         
    private Image _image;
    private RectTransform _rect;
    private float _imageWidth;

    private int maxBarsCount = 10;
    private int barsCount = 5;

    [SerializeField] private Color32    color;
    [SerializeField] private Image prefabBar;
    [SerializeField] private Transform  lineBars;

    public void Setup(int barsCount, int maxBarsCount)
    {
        _image = GetComponent<Image>();
        _rect = GetComponent<RectTransform>();
        _imageWidth = _image.sprite.texture.width;
        
        SetMaxBars(maxBarsCount);
        SetBars(barsCount);
    }

    public void SetBars(int count)
    {
        count = Mathf.Clamp(count, 0, maxBarsCount);
        
        barsCount = count;
        FillBars(barsCount, lineBars);
    }
    
    private void FillBars(int count, Transform parent)
    {
        if (parent.childCount < count)
        {
            CreateNewBars(count-parent.childCount, parent);
        }

        for (int i = 0; i < parent.childCount; i++)
        {
            GameObject childGameObject = parent.GetChild(i).gameObject;
            childGameObject.SetActive(i < count);
        }
    }

    private void CreateNewBars(int addCount, Transform parent)
    {
        for (int i = 0; i < addCount; i++)
        {
            Image bar = Instantiate(prefabBar, parent);
            bar.color = color;
        }
    }

    public void SetMaxBars(int count)
    {
        maxBarsCount = count;
        _rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _imageWidth+(maxBarsCount-MIN_BARS_COUNT)*BAR_SIZE);
    }
}
