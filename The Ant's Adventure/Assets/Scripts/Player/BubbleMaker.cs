using System;
using UnityEngine;

public class BubbleMaker : MonoBehaviour
{
    public enum BubbleType { Food, Question, Ant, ConfusedPlayer,   Stop, Confused }

    [SerializeField] private Bubble prefabBubbleFood;
    [SerializeField] private Bubble prefabBubbleQuestion;
    [SerializeField] private Bubble prefabBubbleAnt;
    [SerializeField] private Bubble prefabBubbleStop;
    [SerializeField] private Bubble prefabBubbleConfused;
    [SerializeField] private Bubble prefabBubbleConfusedPLayer;

    public void CreateBubbleFood()           { CreateBubble(prefabBubbleFood);     }
    public void CreateBubbleQuestion()       { CreateBubble(prefabBubbleQuestion); }
    public void CreateBubbleStop()           { CreateBubble(prefabBubbleStop);     }
    public void CreateBubbleAnt()            { CreateBubble(prefabBubbleAnt);      }
    public void CreateBubbleConfused()       { CreateBubble(prefabBubbleConfused); }
    public void CreateBubbleConfusedPlayer() { CreateBubble(prefabBubbleConfusedPLayer); }
    
    public void CreateBubble(BubbleType type)
    {
        switch (type)
        {
            // Player bubbles
            case BubbleType.Food:           { CreateBubbleFood();           break; }
            case BubbleType.Question:       { CreateBubbleQuestion();       break; }
            case BubbleType.Ant:            { CreateBubbleAnt();            break; }
            case BubbleType.ConfusedPlayer: { CreateBubbleConfusedPlayer(); break; }
            
            // Red ant bubbles
            case BubbleType.Stop:           { CreateBubbleStop();     break; }
            case BubbleType.Confused:       { CreateBubbleConfused(); break; }
        }
    }
    
    private void CreateBubble(Bubble prefab)
    {
        Bubble bubble = Instantiate(prefab, transform.position, Quaternion.identity);
        if (bubble != null)
        {
            bubble.SetFollowTarget(gameObject);
        }
    }
}
