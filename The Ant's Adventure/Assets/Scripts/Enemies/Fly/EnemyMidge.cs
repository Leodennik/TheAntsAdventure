using UnityEngine;

public class EnemyMidge : MonoBehaviour
{
    [SerializeField] private Debris prefabDebris;
    private Animator _animator;

    private void Start()
    {
        int midgeType = Random.Range(1, 4);
    
        _animator = GetComponent<Animator>();
        _animator.Play("Midge_" + midgeType);
    }

    public void OnDeath()
    {
        Instantiate(prefabDebris, transform.position + Vector3.right * 4.0f, Quaternion.identity);
        Instantiate(prefabDebris, transform.position - Vector3.right * 4.0f, Quaternion.identity);
    }

}