using System;
using UnityEngine;

public class Enemy : Character
{
    protected Player player;
    protected Collider2D target;
    protected Vector2 vectorToTarget;
    protected Vector2 directionToTarget;

    protected virtual void Start()
    {
        player = FindObjectOfType<Player>(true);
        target = player.GetComponent<Collider2D>();
        
        LevelSystem.AddEnemyCount(1);
    }
    protected virtual void Update()
    {
        if (target != null)
        {
            vectorToTarget = target.transform.position - transform.position;
            directionToTarget = vectorToTarget.normalized;
        }
        LogicAI();
    }

    protected virtual void LogicAI()
    {

    }

    protected double GetDistanceToPlayer(float maxDistance)
    {
        int compositeLayerMask = (1 << 6 | 1 << 20); // will hit only layers 8 and 9
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToTarget, maxDistance, compositeLayerMask);
        
        if (hit.collider == target)
        {
            return hit.distance;
        }

        return Double.MaxValue;
    }

    private void OnDestroy()
    {
        LevelSystem.AddEnemyCount(-1);
    }
}
