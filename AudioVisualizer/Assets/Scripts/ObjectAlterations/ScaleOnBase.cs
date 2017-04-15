using UnityEngine;

public class ScaleOnBase : MonoBehaviour
{
    public bool X, Y, Z, UseBuffer = true;
    public float MinScale = 1, /*MaxScale = 1,*/ Multiplier = 1;
    
    protected void UpdateScale(float scale)
    {
        scale = Mathf.Max(scale * Multiplier + MinScale, MinScale);
        transform.localScale = new Vector3(X ? scale : transform.localScale.x, Y ? scale : transform.localScale.y, Z ? scale : transform.localScale.z);
    }
}
