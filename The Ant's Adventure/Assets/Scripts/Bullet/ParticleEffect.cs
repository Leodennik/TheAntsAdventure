using UnityEngine;

[RequireComponent (typeof(ParticleSystem))]
public class ParticleEffect : DeleteAfter
{
    public void Detach()
    {
        Vector2 scale = transform.localScale;
        Vector2 parentScale = transform.parent.localScale;
        
        transform.localScale = new Vector2(scale.x/parentScale.x, scale.y/parentScale.y);
        transform.parent = null;
        StopEmission();
        StartDestroy(2);
    }

    private void StopEmission()
    {
        ParticleSystem.EmissionModule em = GetComponent<ParticleSystem>().emission;
        var rate = new ParticleSystem.MinMaxCurve();
        rate.constantMax = 0;
        em.rateOverTime = rate;
    }


}
