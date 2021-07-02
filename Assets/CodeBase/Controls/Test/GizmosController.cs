using UnityEngine;

public class GizmosController : MonoBehaviour
{
  private Vector3 _endPosition;


  void OnMouseDrag()
  {
    gameObject.layer = 2;

    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    RaycastHit hit;

    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
    {
      _endPosition = hit.point;
    }
  }

  private void OnMouseUp()
  {
    Vector3 project = Vector3.ProjectOnPlane(_endPosition - transform.position, transform.forward);
    float xyAngle = Vector3.SignedAngle(project, transform.up, transform.forward);
 
    if (xyAngle < 45 && xyAngle > -45)
    {
      print("RIGHT");
    }
    else if (xyAngle > 135 && xyAngle < 180 || xyAngle < -135 && xyAngle > -180)
    {
      print("LEFT");
    }
    else if (xyAngle < -45 && xyAngle > -135)
    {
      print("UP");
    }
    else if (xyAngle > 45 && xyAngle < 135)
    {
      print("DOWN");
    }

    gameObject.layer = 0;
  }
}