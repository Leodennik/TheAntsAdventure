using System.Collections;
using UnityEngine;

public class DeleteAfter : MonoBehaviour
{
    protected void StartDestroy(float delay)
    {
        StartCoroutine(WaitForDestroy(delay));
    }
    
    IEnumerator WaitForDestroy(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
