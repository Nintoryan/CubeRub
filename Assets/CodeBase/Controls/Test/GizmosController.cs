using UnityEngine;

public class GizmosController : MonoBehaviour
{
  private Vector3 screenPoint;
  private Vector3 offset;

  private Vector3 _originPosition;

  private void Start()
  {
    _originPosition = transform.position;
  }

  void OnMouseDown()
  {
    screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
  }

  void OnMouseDrag()
  {
    Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

    Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
    transform.position = curPosition;
  }

  private void OnMouseUp()
  {
    Vector3 direction = (transform.position - _originPosition).normalized;
   
    transform.GetChild(0).localScale = direction * 5;
    transform.position = _originPosition;
  }
}