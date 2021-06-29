using CodeBase.Controls.CubeRub;
using CodeBase.Controls.Inputs;
using UnityEngine;

namespace CodeBase.Controls.CameraLogic
{
  public class CameraMovement : MonoBehaviour
  {
    private SpawnCube _spawnCube;
    private Touchpad _touchpad;
    private Vector3 _localRotation;
    private bool _cameraDisable;
    private Camera _mainCamera;

    private void Start()
    {
      _touchpad = FindObjectOfType<Touchpad>();
      _mainCamera = Camera.main;
      _spawnCube = FindObjectOfType<SpawnCube>();
      _spawnCube.Spawning += () => transform.parent.position = _spawnCube.CenterPiece.position;
    }

    private void LateUpdate()
    {
      if (Input.GetMouseButtonDown(0))
      {
       
      }
      
      if (Input.GetMouseButton(0))
      {
        Ray ray = _mainCamera.ScreenPointToRay(_touchpad.PressingPosition);

        if (Physics.Raycast(ray, out var hit, Mathf.Infinity))
          return;
        
          _localRotation.x += Input.GetAxis("Mouse X") * 5;
          _localRotation.y += Input.GetAxis("Mouse Y") * -5;
          _localRotation.y = Mathf.Clamp(_localRotation.y, -90, 90);
      }

      Quaternion qt = Quaternion.Euler(_localRotation.y, _localRotation.x, 0);
      transform.parent.rotation = Quaternion.Lerp(transform.parent.rotation, qt, Time.deltaTime * 15);
    }
  }
}