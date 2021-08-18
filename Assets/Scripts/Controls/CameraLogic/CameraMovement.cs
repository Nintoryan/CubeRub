using System;
using CubeRub.LevelGenerator;
using CustomUserInput;
using UnityEngine;

namespace CubeRub.Controls
{
  public class CameraMovement : MonoBehaviour
  {
    [Header("Same as prebuild Rotation")]
    [SerializeField] private Vector3 _localRotation;

    [SerializeField] private HoldAndDrag _holdAndDrag;

    [SerializeField] private float _sensitivity;
    private Cube _cube;
    private Camera _mainCamera;
    private bool isOnCube;

    private Transform _parent;

    private void Awake()
    {
      _cube = FindObjectOfType<Cube>();
      _mainCamera = Camera.main;
      _cube.OnCalculated += () => transform.parent.position = _cube.CenterPiece.position;
      _parent = transform.parent;
    }

    private void OnEnable()
    {
      _holdAndDrag.Started += CheckIsOnCube;
      _holdAndDrag.Dragged += Rotate;
    }

    private void OnDisable()
    {
      _holdAndDrag.Started -= CheckIsOnCube;
      _holdAndDrag.Dragged -= Rotate;
    }

    private void CheckIsOnCube()
    {
      isOnCube = false;
      var ray = _mainCamera.ScreenPointToRay(_holdAndDrag.StartPoint);
      if (Physics.Raycast(ray, out _, Mathf.Infinity) && Input.GetMouseButtonDown(0))
      {
        isOnCube = true;
      }
    }

    private void Rotate()
    {
      if(isOnCube) return;
      _localRotation.x += _holdAndDrag.Delta.x * _sensitivity;
      _localRotation.y += _holdAndDrag.Delta.y * -_sensitivity;
      _localRotation.y = Mathf.Clamp(_localRotation.y, -90, 90);
    }
    
    
    private void LateUpdate()
    {
      var qt = Quaternion.Euler(_localRotation.y, _localRotation.x, 0);
      _parent.rotation = Quaternion.Lerp(_parent.rotation, qt, Time.fixedDeltaTime * 15f);
    }
    
  }
}