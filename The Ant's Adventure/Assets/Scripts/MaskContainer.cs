using UnityEngine;

public class MaskContainer : MonoBehaviour
{
    public static LayerMask maskBed   = LayerMask.GetMask("Bed");
    public static LayerMask maskSolid = LayerMask.GetMask("Solid");
    public static LayerMask maskSpit  = LayerMask.GetMask("Spit");
    public static LayerMask maskGlass = LayerMask.GetMask("Glass");
}
