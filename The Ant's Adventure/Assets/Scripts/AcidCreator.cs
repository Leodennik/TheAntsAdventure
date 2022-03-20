using UnityEngine;

public class AcidCreator : MonoBehaviour
{
    [SerializeField] private Color32 acidColor = Color.white;
    [SerializeField] private int acidBrightness = 10;
    [SerializeField] private AcidBomb prefabAcidBomb;
    [SerializeField] private AcidSpray prefabAcidSpray;
    [SerializeField] private int spitCount = 10;

    public void Spit(GameObject creator, Vector2 direction, int damage)
    {
        if (prefabAcidSpray == null) return;
        
        for (int i = 0; i < spitCount; i++)
        {
            Bullet newSpitBullet = Instantiate(prefabAcidSpray, transform.position, Quaternion.identity);
            newSpitBullet.Create(creator, direction, damage);
            newSpitBullet.SetColor(acidColor, acidBrightness);
        }
    }
    
    // Update is called once per frame
    public void Shoot(GameObject creator, Vector2 direction, int damage)
    {
        if (prefabAcidBomb == null) return;
        
        AcidBomb newAcidBomb = Instantiate(prefabAcidBomb, transform.position, Quaternion.identity);
        newAcidBomb.Create(creator, direction, damage);
        newAcidBomb.SetColor(acidColor, acidBrightness);
    }
}
