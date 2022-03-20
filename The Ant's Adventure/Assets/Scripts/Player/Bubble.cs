using UnityEngine;

public class Bubble : MonoBehaviour
{
    private Transform _follow;
    private float _smoothFactor = 20.0f;

    public void SetFollowTarget(GameObject follow)
    {
        _follow = follow.transform;
    }

    private void FixedUpdate()
    {
        if (_follow != null)
        {
            transform.position = Vector3.Lerp(transform.position, _follow.position, Time.deltaTime * _smoothFactor);
        }
    }
}
