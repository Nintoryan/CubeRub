using CubeRub.Controls.CubeRub;
using UnityEngine;
using static VectorTools;
using Vector3 = UnityEngine.Vector3;

namespace CubeRub.LevelGenerator
{
  public class PieceFace : MonoBehaviour
  {
    private Vector3 IgnorAxis => GetBiggestAxis(-transform.forward);
    private Camera _camera;
    private bool isMouseDown;

    private Vector3 _firstLinePoint;
    private Vector3 _secondLinePoint;

    private void Start()
    {
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
        _secondLinePoint = GetSecondLinePoint(_firstLinePoint, hit.point, IgnorAxis);
      }
    }

    private void OnMouseUp()
    {
      isMouseDown = false;
      var inputResult = _secondLinePoint - _firstLinePoint;
      var RotationAxis = GetRotationAxis(inputResult);
      var position = GetRotationPosition(RotationAxis);
      var isForward = GetRotationDirection(inputResult, RotationAxis);

      CubeRotation.RotateCube(RotationAxis, position, isForward);

      Debug.Log($"IgnorAxis:{IgnorAxis};RotationAxis:{RotationAxis}; Position:{position}");
    }

    private Axis GetRotationAxis(Vector3 inputResult)
    {
      var RotationAxis = Axis.x;
      if (Mathf.Round(IgnorAxis.x) == 0 && Mathf.Round(inputResult.x) == 0)
      {
        RotationAxis = Axis.x;
      }
      else if (Mathf.Round(IgnorAxis.y) == 0 && Mathf.Round(inputResult.y) == 0)
      {
        RotationAxis = Axis.y;
      }
      else if (Mathf.Round(IgnorAxis.z) == 0 && Mathf.Round(inputResult.z) == 0)
      {
        RotationAxis = Axis.z;
      }

      return RotationAxis;
    }

    private float GetRotationPosition(Axis rotationsAxis)
    {
      return rotationsAxis switch
      {
        Axis.x => _secondLinePoint.x,
        Axis.y => _secondLinePoint.y,
        _ => _secondLinePoint.z - 0.5f
      };
    }

    private bool GetRotationDirection(Vector3 inputResult, Axis RotationAxis)
    {
      var isForward = IsBiggestPositiv(inputResult);
      if (!IsBiggestPositiv(IgnorAxis))
      {
        isForward = !isForward;
      }

      if (Mathf.Abs(IgnorAxis.x) > 0.001f && RotationAxis == Axis.y ||
          Mathf.Abs(IgnorAxis.y) > 0.001f && RotationAxis == Axis.z ||
          Mathf.Abs(IgnorAxis.z) > 0.001f && RotationAxis == Axis.x)
      {
        isForward = !isForward;
      }

      return isForward;
    }


    private void OnDrawGizmos()
    {
      if (!isMouseDown) return;
      Gizmos.color = Color.black;
      Gizmos.DrawSphere(_firstLinePoint, 0.1f);
      Gizmos.color = Color.cyan;
      Gizmos.DrawSphere(_secondLinePoint, 0.1f);
      Gizmos.color = Color.yellow;
    }
  }
}