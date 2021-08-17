using CubeRub.LevelGenerator;
using UnityEngine;

namespace CubeRub.Controls
{
  public class CameraMovement : MonoBehaviour
  {
    [Header("Same as prebuild Rotation")]
    [SerializeField] private Vector3 _localRotation;
    private Cube _cube;
    private Camera _mainCamera;
    private bool isOnCube;
    

    private void Awake()
    {
      _cube = FindObjectOfType<Cube>();
      _mainCamera = Camera.main;
      _cube.OnCalculated += () => transform.parent.position = _cube.CenterPiece.position;
    }
    
    private void LateUpdate()
    {
      if (Input.GetMouseButtonUp(0))
      {
        isOnCube = false;
      }
      if (Input.GetMouseButton(0))
      {
        var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out _, Mathf.Infinity) && Input.GetMouseButtonDown(0))
        {
          isOnCube = true;
        }
        if(isOnCube) return;
        
        _localRotation.x += Input.GetAxis("Mouse X") * 5;
        _localRotation.y += Input.GetAxis("Mouse Y") * -5;
        _localRotation.y = Mathf.Clamp(_localRotation.y, -90, 90);
      }
      
      var qt = Quaternion.Euler(_localRotation.y, _localRotation.x, 0);
      var parent = transform.parent;
      parent.rotation = Quaternion.Lerp(parent.rotation, qt, Time.deltaTime * 15);
    }
    
  }
}