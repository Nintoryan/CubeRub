using System;
using CubeRub.Controls.CubeRub;
using UnityEngine;

public class PathPoint : MonoBehaviour
{
    private const float NormalStep = 0.1f;
    public Vector3 Position => transform.position;
    [SerializeField] private Vector3 up;

    public Vector3 Up => up;

    public void Initialize(Vector3 nextPoint,Path parent)
    {
        up = GetNormal(Position, nextPoint, parent);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(Position,0.02f);
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(Position,Up);
        Gizmos.DrawSphere(Up,0.01f);
    }

    private static Vector3 GetNormal(Vector3 p1, Vector3 p2,Path parentPath)
        {
            Vector2 x = new Vector2(), y = new Vector2();
            Vector3 candidate1 = new Vector3(),candidate2 = new Vector3();
            switch (parentPath.SameAxis)
            {
                case Axis.x:
                    x = new Vector2(p1.y,p1.z);
                    y = new Vector2(p2.y,p2.z);
                    break;
                case Axis.y:
                    x = new Vector2(p1.x,p1.z);
                    y = new Vector2(p2.x,p2.z);
                    break;
                case Axis.z:
                    x = new Vector2(p1.x,p1.y);
                    y = new Vector2(p2.x,p2.y);
                    break;
            }
            if (VectorTools.isSameX(x, y))
            {
                switch (parentPath.SameAxis)
                {
                    case Axis.z:
                        candidate1 = new Vector3(p1.x,p1.y,p1.z+NormalStep);
                        candidate2 = new Vector3(p1.x,p1.y,p1.z-NormalStep);
                        break;
                    case Axis.y:
                        goto case Axis.x;
                    case Axis.x:
                        candidate1 = new Vector3(p1.x,p1.y+NormalStep,p1.z);
                        candidate2 = new Vector3(p1.x,p1.y-NormalStep,p1.z);
                        break;
                }
            }
            else if (VectorTools.isSameY(x, y))
            {
                switch (parentPath.SameAxis)
                {
                    case Axis.x:
                        candidate1 = new Vector3(p1.x,p1.y+NormalStep,p1.z);
                        candidate2 = new Vector3(p1.x,p1.y-NormalStep,p1.z);
                        break;
                    case Axis.y:
                        candidate1 = new Vector3(p1.x+NormalStep,p1.y,p1.z);
                        candidate2 = new Vector3(p1.x-NormalStep,p1.y,p1.z);
                        break;
                    case Axis.z:
                        goto case Axis.y;
                }
            }
            else
            {
                var c = -(x.x * y.y - y.x * x.y) / (y.x - x.x);
                var Kn = (y.x - x.x) / (x.y - y.y);
                var pc = new Vector2((x.x + y.x)/2f,(x.y+y.y)/2f);
            
                var pc1 = new Vector2(pc.x+NormalStep,Kn*(pc.x+NormalStep)+c);
                var pc2 = new Vector2(pc.x-NormalStep,Kn*(pc.x-NormalStep)+c);
                switch (parentPath.SameAxis)
                {
                    case Axis.x:
                        candidate1 = new Vector3(p1.x,pc1.x,pc1.y);
                        candidate2 = new Vector3(p1.x,pc2.x,pc2.y);
                        break;
                    case Axis.y:
                        candidate1 = new Vector3(pc1.x,p1.y,pc1.y);
                        candidate2 = new Vector3(pc2.x,p1.y,pc2.y);
                        break;
                    case Axis.z:
                        candidate1 = new Vector3(pc1.x,pc1.y,p1.z);
                        candidate2 = new Vector3(pc2.x,pc2.y,p1.z);
                        break;
                }
            }
            return Vector3.Distance(candidate1, parentPath.transform.position) >
                   Vector3.Distance(candidate2, parentPath.transform.position) ? 
                candidate1 : 
                candidate2;
        }
}
