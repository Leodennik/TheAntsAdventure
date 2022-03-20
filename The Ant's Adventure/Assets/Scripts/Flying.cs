using System;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Flying : MonoBehaviour
{
    [Range(1.0f, 100.0f)]
    [SerializeField] protected float moveSpeed = 10.0f;
   
    [Range(1.0f, 5.0f)]
    [SerializeField] protected float maxMoveTime = 1.0f;
    
    [Range(1.0f, 500.0f)]
    [SerializeField] protected float flyZoneRadius = 100;

    private bool _isStarted = false;
    private Vector3 _startPosition;
    
    private Vector2 _direction;
    private float _changeDirectionTimer;
    private float _moveTime;


    private void Start()
    {
        _isStarted = true;
        _startPosition = transform.position;
        
        ChangeDirection();
    }

    private void Update()
    {
        _changeDirectionTimer += Time.deltaTime;
        if (IsZoneCrossed())
        {
            _direction = (_startPosition - transform.position).normalized;
        }
        
        if (_changeDirectionTimer >= _moveTime)
        {
            ChangeDirection();
        }
        
        transform.Translate(_direction * moveSpeed * Time.deltaTime, Space.Self);
        //transform.Translate(_direction * moveSpeed * Time.deltaTime);

    }

    private bool IsZoneCrossed()
    {
        float distance = (_startPosition - transform.position).magnitude;
        return Math.Abs(distance) > flyZoneRadius;
    }

    private void ChangeDirection() 
    {
        _changeDirectionTimer = 0;
        _moveTime = Random.Range(0.0f, maxMoveTime);
        _direction = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
    }

    public Vector3 GetStartPosition()
    {
        if (_isStarted)
        {
            return _startPosition;
        }
        return transform.position;
    }

    public float GetFlyZoneRadius()
    {
        return flyZoneRadius;
    }
}

/*
// Magic in Editor /////////////////////////////////////////////////////////////////////////////////////////////////////
[CustomEditor(typeof(Flying), true )]
public class FlyEditor: Editor 
{
    private Flying _fly;
    public void OnSceneGUI () 
    {
        _fly = target as Flying;
        Handles.color = Color.red;
        Handles.DrawWireDisc(_fly.GetStartPosition(), Vector3.back, _fly.GetFlyZoneRadius());
    }
}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
*/