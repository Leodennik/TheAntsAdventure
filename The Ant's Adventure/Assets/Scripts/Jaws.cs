using UnityEngine;

public class Jaws : MonoBehaviour
{
    [SerializeField] private Bite biteBulletPrefab;
    [SerializeField] private int biteDamage = 1;
    [SerializeField] private float bulletOffset = 10.0f;
    [SerializeField] private float bulletLifeTime = 1.0f;

    public void SetDamage(int damage)
    {
        biteDamage = damage;
    }
    
    public virtual void Snap(GameObject creator)
    {
        float playerScaleX = creator.transform.localScale.x;
        Vector3 offset = transform.right * playerScaleX * bulletOffset;

        Bite newBiteBullet = Instantiate(biteBulletPrefab, transform.position + offset, Quaternion.identity, transform);
        newBiteBullet.Create(creator, Vector2.right, biteDamage);
        newBiteBullet.SetLifeTime(bulletLifeTime);
    }

}
