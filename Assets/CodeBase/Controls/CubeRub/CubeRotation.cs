using System;
using System.Collections;
using System.Collections.Generic;
using CodeBase.Controls.Inputs;
using UnityEngine;

namespace CodeBase.Controls.CubeRub
{
  [RequireComponent(typeof(SpawnCube))]
  public class CubeRotation : MonoBehaviour
  {
    private const float TOLERANCE = 0.1f;
    [SerializeField] private SpawnCube _spawnCube;
    [SerializeField] private Transform _cubeCast;

    private Touchpad _touchpad;
    private bool _canRotate = true;
    private Camera _mainCamera;
    private Transform _selectedPiece;
    private static CubeRotation Instance;
    

    public static void RotateCube(Axis same, float position, bool isForward = true)
    {
      Instance.RotateCubeFace(same, position,isForward);
    }
    private void RotateCubeFace(Axis same, float position, bool isForward = true)
    {
      List<GameObject> pieces;
      Vector3 rotationAxesVector;
      switch (same)
      {
        case Axis.x:
          pieces = _spawnCube.CubePartsList.FindAll(c =>
            Math.Abs(Mathf.Round(c.transform.localPosition.x) - position) < TOLERANCE);
          rotationAxesVector = new Vector3(1,0,0);
          break;
        case Axis.y:
          pieces = _spawnCube.CubePartsList.FindAll(c =>
            Math.Abs(Mathf.Round(c.transform.localPosition.y) - position) < TOLERANCE);
          rotationAxesVector = new Vector3(0,1,0);
          break;
        case Axis.z:
          pieces = _spawnCube.CubePartsList.FindAll(c =>
            Math.Abs(Mathf.Round(c.transform.localPosition.z) - position) < TOLERANCE);
          rotationAxesVector = new Vector3(0,0,1);
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(same), same, null);
      }
      if (!isForward)
      {
        rotationAxesVector *= -1;
      }
      StartCoroutine(Rotate(pieces, rotationAxesVector));
    }
    

    public event Action Roating;

    #region AxesRotation

    private List<GameObject> UpPieces
      => _spawnCube.CubePartsList.FindAll(x => Mathf.Round(x.transform.localPosition.y) == 0);

    private List<GameObject> DownPieces
      => _spawnCube.CubePartsList.FindAll(x => Mathf.Round(x.transform.localPosition.y) == -2);


    private List<GameObject> FrontPieces
      => _spawnCube.CubePartsList.FindAll(x => Mathf.Round(x.transform.localPosition.x) == 0);

    private List<GameObject> BackPieces
      => _spawnCube.CubePartsList.FindAll(x => Mathf.Round(x.transform.localPosition.x) == -2);


    private List<GameObject> LeftPieces
      => _spawnCube.CubePartsList.FindAll(x => Mathf.Round(x.transform.localPosition.z) == 0);

    private List<GameObject> RightPieces
      => _spawnCube.CubePartsList.FindAll(x => Mathf.Round(x.transform.localPosition.z) == 2);


    private List<GameObject> CenterXPieces
      => _spawnCube.CubePartsList.FindAll(x => Mathf.Round(x.transform.localPosition.x) == -1);

    private List<GameObject> CenterZPieces
      => _spawnCube.CubePartsList.FindAll(x => Mathf.Round(x.transform.localPosition.z) == 1);

    private List<GameObject> CenterYPieces
      => _spawnCube.CubePartsList.FindAll(x => Mathf.Round(x.transform.localPosition.y) == -1);

    #endregion Rotation

    private void Start()
    {
      Instance = this;
      _mainCamera = Camera.main;

      _touchpad = FindObjectOfType<Touchpad>();
    }

    private void Update()
    {
      if (_canRotate)
        ChekInput();
    }

    
    
    private void ChekInput()
    {
      #region Input

      if (Input.GetKeyDown(KeyCode.Alpha1))
        StartCoroutine(Rotate(UpPieces, new Vector3(0, 1, 0)));
      else if (Input.GetKeyDown(KeyCode.Alpha2))
        StartCoroutine(Rotate(UpPieces, new Vector3(0, -1, 0)));

      else if (Input.GetKeyDown(KeyCode.Alpha3))
        StartCoroutine(Rotate(LeftPieces, new Vector3(0, 0, -1)));
      else if (Input.GetKeyDown(KeyCode.Alpha4))
        StartCoroutine(Rotate(RightPieces, new Vector3(0, 0, 1)));

      else if (Input.GetKeyDown(KeyCode.Alpha5))
        StartCoroutine(Rotate(FrontPieces, new Vector3(1, 0, 0)));
      else if (Input.GetKeyDown(KeyCode.Alpha6))
        StartCoroutine(Rotate(BackPieces, new Vector3(-1, 0, 0)));

      else if (Input.GetKeyDown(KeyCode.Alpha8))
        StartCoroutine(Rotate(CenterXPieces, new Vector3(-1, 0, 0)));
      else if (Input.GetKeyDown(KeyCode.Alpha9))
        StartCoroutine(Rotate(CenterYPieces, new Vector3(0, -1, 0)));
      else if (Input.GetKeyDown(KeyCode.Alpha0))
        StartCoroutine(Rotate(CenterZPieces, new Vector3(0, 0, -1)));

      #endregion
    }
    IEnumerator Rotate(List<GameObject> listCubePieces, Vector3 rotationAxes)
    {
      int angele = 0;
      _canRotate = false;

      while (angele < 90)
      {
        foreach (GameObject piece in listCubePieces)
          piece.transform.RotateAround(_spawnCube.CenterPiece.position, rotationAxes, 5);

        angele += 5;

        yield return new WaitForFixedUpdate();
      }

      _canRotate = true;
      
      Roating?.Invoke();
    }
  }

  public enum Axis
  {
    x,y,z
  }
}