using CodeBase.Controls.CubeRub;
using UnityEngine;

public class GizmosController : MonoBehaviour
{
  private Vector3 screenPoint;
  private Vector3 offset;

  private Vector3 _originPosition;

  private void Start()
  {
    CubeRotation cubeRotation = transform.root.GetComponent<CubeRotation>();
    cubeRotation.Roating += UpdatePosition;
    UpdatePosition();
  }

  private void UpdatePosition()
  {
    _originPosition = transform.position;
  }

  void OnMouseDown()
  {
    screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
  }

  void OnMouseDrag()
  {
    gameObject.layer = 2;

    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    RaycastHit hit;

    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
    {
      transform.position = hit.point;
    }
  }

  private void OnMouseUp()
  {
    Vector3 direction = (transform.position - _originPosition).normalized;
    Debug.Log(direction);

    transform.position = _originPosition;
    gameObject.layer = 0;
  }
}