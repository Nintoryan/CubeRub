using UnityEngine;

public static class VectorTools
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
        float value;
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
    private static Vector3 GetFlatPoint(Vector3 firstPoint, Vector3 secondPoint, Vector3 IgnorAxis)
    {
        var result = secondPoint;
        if (Mathf.Abs(IgnorAxis.x) > 0.00001f)
        {
            result.x = firstPoint.x;
        }
        else if(Mathf.Abs(IgnorAxis.y) > 0.00001f)
        {
            result.y = firstPoint.y;
        }else if (Mathf.Abs(IgnorAxis.z) > 0.00001f)
        {
            result.z = firstPoint.z;
        }
        return result;
    }
  
    public static Vector3 GetSecondLinePoint(Vector3 firstPoint, Vector3 secondPoint,Vector3 IgnorAxis)
    {

        var result = GetFlatPoint(firstPoint, secondPoint, IgnorAxis) - firstPoint;
        return firstPoint + GetBiggestAxis(result);
    }
}
