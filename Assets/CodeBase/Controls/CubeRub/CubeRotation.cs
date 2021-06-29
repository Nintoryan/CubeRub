using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Controls.CubeRub
{
  [RequireComponent(typeof(SpawnCube))]
  public class CubeRotation : MonoBehaviour
  {
    [SerializeField] private SpawnCube _spawnCube;

    private bool _canRotate = true;
    
    private List<GameObject> UpPieces
      => _spawnCube.CubePartsList.FindAll(x => Mathf.Round(x.transform.localPosition.y) == 0);

    private List<GameObject> DownPieces
      =>  _spawnCube.CubePartsList.FindAll(x => Mathf.Round(x.transform.localPosition.y) == -2);

    private List<GameObject> FrontPieces
      =>  _spawnCube.CubePartsList.FindAll(x => Mathf.Round(x.transform.localPosition.x) == 0);

    private List<GameObject> BackPieces
      =>  _spawnCube.CubePartsList.FindAll(x => Mathf.Round(x.transform.localPosition.x) == -2);

    private List<GameObject> LeftPieces
      =>  _spawnCube.CubePartsList.FindAll(x => Mathf.Round(x.transform.localPosition.z) == 0);

    private List<GameObject> RightPieces
      =>  _spawnCube.CubePartsList.FindAll(x => Mathf.Round(x.transform.localPosition.z) == 2);
  
    private List<GameObject> CenterXPieces
      =>  _spawnCube.CubePartsList.FindAll(x => Mathf.Round(x.transform.localPosition.x)  == -1);
  
    private List<GameObject> CenterZPieces
      =>  _spawnCube.CubePartsList.FindAll(x => Mathf.Round(x.transform.localPosition.z)  == 1);
  
    private List<GameObject> CenterYPieces
      =>  _spawnCube.CubePartsList.FindAll(x => Mathf.Round(x.transform.localPosition.y)  == -1);
    
    private void Update()
    {
      if (_canRotate)
        ChekInput();
    }
    
    private void ChekInput()
    {
      if (Input.GetKeyDown(KeyCode.Alpha1))
        StartCoroutine(Rotate(UpPieces, new Vector3(0, 1, 0)));
      else if (Input.GetKeyDown(KeyCode.Alpha2))
        StartCoroutine(Rotate(DownPieces, new Vector3(0, -1, 0)));
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
    }
    
    IEnumerator Rotate(List<GameObject> listCubePieces, Vector3 rotationAxes)
    {
      int angele = 0;
      _canRotate = false;

      while (angele < 90)
      {
        foreach (GameObject piece in listCubePieces)
          piece.transform.RotateAround(_spawnCube.CenterPiece.transform.position, rotationAxes, 5);

        angele += 5;
      
        yield return new WaitForFixedUpdate();
      }

      _canRotate = true;
    }
  }
}