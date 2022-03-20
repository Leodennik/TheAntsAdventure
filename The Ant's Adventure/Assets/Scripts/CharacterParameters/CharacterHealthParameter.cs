using UnityEngine;

public class CharacterHealthParameter : MonoBehaviour
{
    [SerializeField] private UIParameterLineBars healthBar;

    [SerializeField] protected int value = 5;
    [SerializeField] protected int maxValue = 5;
    
    public virtual void Start()
    {
    }
    
    public void SetUI(UIParameterLineBars uiBar)
    {
        healthBar = uiBar;
        healthBar.Setup(value, maxValue);
    }

    public void AddValue(int amount)
    {
        SetValue(value + amount);
    }

    public void SetValue(int amount)
    {
        value = Mathf.Clamp(amount, 0, maxValue);

        if (healthBar != null)
        {
            healthBar.SetBars(value);
        }
    }
    
    public int GetValue()
    {
        return value;
    }

    public void SetMaxValue(int value)
    {
        maxValue = value;
        if (healthBar != null)
        {
            healthBar.SetMaxBars(value);
        }
    }

    public int GetMaxValue()
    {
        return maxValue;
    }
    
    public bool IsMaxValue()
    {
        return value == maxValue;
    }
}
