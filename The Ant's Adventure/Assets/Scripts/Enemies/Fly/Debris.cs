using UnityEngine;

public class Debris : Entity
{
    private void Start()
    {
        body.AddForce(Vector2.up*Random.Range(0.2f, 0.5f), ForceMode2D.Impulse);
        body.AddTorque(Random.Range(0.0f, 0.2f), ForceMode2D.Impulse);
    }
}
