using UnityEngine;

public class VectorTools
{
    
    public static Vector3 GetBiggestAxis(Vector3 a)
    {
        if (Mathf.Abs(a.x) > Mathf.Abs(a.y) && Mathf.Abs(a.x) > Mathf.Abs(a.z))
        {
            return new Vector3(a.x,0,0);
        }
        if (Mathf.Abs(a.y) > Mathf.Abs(a.x) && Mathf.Abs(a.y) > Mathf.Abs(a.z))
        {
            return new Vector3(0,a.y,0);
        }
        return new Vector3(0,0,a.z);
    }

    public static bool IsBiggestPositiv(Vector3 a)
    {
        float value = 0;
        if (Mathf.Abs(a.x) > Mathf.Abs(a.y) && Mathf.Abs(a.x) > Mathf.Abs(a.z))
        {
            value = a.x;
        }else
        if (Mathf.Abs(a.y) > Mathf.Abs(a.x) && Mathf.Abs(a.y) > Mathf.Abs(a.z))
        {
            value = a.y;
        }
        else
        {
            value = a.z;
        }

        return value > 0;
    }
    

    public static Vector3 FillZeros(Vector3 toFill, Vector3 fill)
    {
        var result = toFill;
        if (result.x == 0) result.x = fill.x;
        if (result.y == 0) result.y = fill.y;
        if (result.z == 0) result.z = fill.z;
        return result;
    }
}
