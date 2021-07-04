using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using CodeBase.Controls.CubeRub;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PieceFace : MonoBehaviour
{
  public Vector3 IgnorAxis => VectorTools.GetBiggestAxis(-transform.forward);
  public List<CubeBigFace> Changebles = new List<CubeBigFace>();
  private Camera _camera;
  private Vector3 firstPoint;
  private Vector3 secondPoint;
  private CubePiece _cubePiece;
  private bool isMouseDown;

  [SerializeField] private Transform DetermingRayPoint;
  private Vector3 _firstLinePoint;
  private Vector3 _secondLinePoint;
  
  private void Start()
  {
    _cubePiece = GetComponentInParent<CubePiece>();
    _camera = Camera.main;
  }
  
  private void OnMouseDown()
  {
    _firstLinePoint = transform.position;
    isMouseDown = true;
  }
  
  private void OnMouseDrag()
  {
    var ray = _camera.ScreenPointToRay(Input.mousePosition);
    if (Physics.Raycast(ray, out var hit, Mathf.Infinity))
    {
      _secondLinePoint = GetSecondLinePoint(_firstLinePoint,GetFlatPoint(hit.point));
    }
  }

  private void OnMouseUp()
  {
    isMouseDown = false;
    //Debug.Log($"point 1:{_firstLinePoint} point 2:{_secondLinePoint} IgnorAxis:{IgnorAxis}");
    var inputResult = _secondLinePoint - _firstLinePoint;
    //Debug.Log($"Input Result: {inputResult}");
    var RotationAxis = Axis.x;
    var position = _secondLinePoint.x;
    var isForward = VectorTools.IsBiggestPositiv(inputResult);
    if (Mathf.Round(IgnorAxis.x) == 0 && Mathf.Round(inputResult.x) == 0)
    {
      RotationAxis = Axis.x;
      position = _secondLinePoint.x;
    }
    else if (Mathf.Round(IgnorAxis.y) == 0 && Mathf.Round(inputResult.y) == 0)
    {
      RotationAxis = Axis.y;
      position = _secondLinePoint.y;
    }
    else if(Mathf.Round(IgnorAxis.z) == 0 && Mathf.Round(inputResult.z) == 0)
    {
      RotationAxis = Axis.z;
      position = _secondLinePoint.z - 0.5f;
    }
    Debug.Log($"IgnorAxis:{IgnorAxis};RotationAxis:{RotationAxis}; Position:{position}" );
    if (!VectorTools.IsBiggestPositiv(IgnorAxis))
    {
      isForward = !isForward;
    }
    if (Mathf.Abs(IgnorAxis.x) > 0.001f && RotationAxis == Axis.y||
        Mathf.Abs(IgnorAxis.y) > 0.001f && RotationAxis == Axis.z||
        Mathf.Abs(IgnorAxis.z) > 0.001f && RotationAxis == Axis.x)
    {
      isForward = !isForward;
    }
    CubeRotation.RotateCube(RotationAxis,position,isForward);

    
  }
  
  private void OnDrawGizmos()
  {
    if(!isMouseDown) return;
    Gizmos.color = Color.black;
    Gizmos.DrawSphere(_firstLinePoint,0.1f);
    Gizmos.color = Color.cyan;
    Gizmos.DrawSphere(_secondLinePoint,0.1f);
    Gizmos.color = Color.yellow;
  }
  
  private Vector3 GetFlatPoint(Vector3 point)
  {
    var result = point;
    if (Mathf.Abs(IgnorAxis.x) > 0.00001f)
    {
      result.x = _firstLinePoint.x;
    }
    else if(Mathf.Abs(IgnorAxis.y) > 0.00001f)
    {
      result.y = _firstLinePoint.y;
    }else if (Mathf.Abs(IgnorAxis.z) > 0.00001f)
    {
      result.z = _firstLinePoint.z;
    }
    return result;
  }
  
  private Vector3 GetSecondLinePoint(Vector3 firstPoint, Vector3 secondPoint)
  {
    var result = secondPoint - firstPoint;
    return firstPoint + VectorTools.GetBiggestAxis(result);
  }
}