using System;
using System.Collections;
using System.Collections.Generic;
using CubeRub.LevelGenerator;
using UnityEngine;

namespace CubeRub.Controls.CubeRub
{
  [RequireComponent(typeof(Cube))]
  public class CubeRotation : MonoBehaviour
  {
    private const float TOLERANCE = 0.1f;
    [SerializeField] private Cube Cube;
    private bool _canRotate = true;
    private Transform _selectedPiece;
    private static CubeRotation Instance;

    private void Start()
    {
      Instance = this;
    }

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
          pieces = Cube.CubePartsList.FindAll(c =>
            Math.Abs(Mathf.Round(c.transform.localPosition.x) - position) < TOLERANCE);
          rotationAxesVector = new Vector3(1,0,0);
          break;
        case Axis.y:
          pieces = Cube.CubePartsList.FindAll(c =>
            Math.Abs(Mathf.Round(c.transform.localPosition.y) - position) < TOLERANCE);
          rotationAxesVector = new Vector3(0,1,0);
          break;
        case Axis.z:
          pieces = Cube.CubePartsList.FindAll(c =>
            Math.Abs(Mathf.Round(c.transform.localPosition.z) - position) < TOLERANCE);
          rotationAxesVector = new Vector3(0,0,1);
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(same), same, null);
      }

      if (pieces.Count == Cube.CubePartsList.Count)
      {
        Debug.LogWarning("Attempting to rotate whole cube!");
        return;
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
      => Cube.CubePartsList.FindAll(x => Mathf.Round(x.transform.localPosition.y) == 0);

    private List<GameObject> DownPieces
      => Cube.CubePartsList.FindAll(x => Mathf.Round(x.transform.localPosition.y) == -2);


    private List<GameObject> FrontPieces
      => Cube.CubePartsList.FindAll(x => Mathf.Round(x.transform.localPosition.x) == 0);

    private List<GameObject> BackPieces
      => Cube.CubePartsList.FindAll(x => Mathf.Round(x.transform.localPosition.x) == -2);


    private List<GameObject> LeftPieces
      => Cube.CubePartsList.FindAll(x => Mathf.Round(x.transform.localPosition.z) == 0);

    private List<GameObject> RightPieces
      => Cube.CubePartsList.FindAll(x => Mathf.Round(x.transform.localPosition.z) == 2);


    private List<GameObject> CenterXPieces
      => Cube.CubePartsList.FindAll(x => Mathf.Round(x.transform.localPosition.x) == -1);

    private List<GameObject> CenterZPieces
      => Cube.CubePartsList.FindAll(x => Mathf.Round(x.transform.localPosition.z) == 1);

    private List<GameObject> CenterYPieces
      => Cube.CubePartsList.FindAll(x => Mathf.Round(x.transform.localPosition.y) == -1);

    #endregion Rotation
    
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
    private IEnumerator Rotate(List<GameObject> listCubePieces, Vector3 rotationAxes)
    {
      var angele = 0;
      _canRotate = false;

      while (angele < 90)
      {
        foreach (var piece in listCubePieces)
          piece.transform.RotateAround(Cube.CenterPiece.position, rotationAxes, 5);

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