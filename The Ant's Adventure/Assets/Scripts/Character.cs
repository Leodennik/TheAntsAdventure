using UnityEngine;

public class Character : Entity
{
    public void SetLocalScale(float scaleX, float scaleY)
    {
        transform.localScale = new Vector2(scaleX, scaleY);
    }
}
