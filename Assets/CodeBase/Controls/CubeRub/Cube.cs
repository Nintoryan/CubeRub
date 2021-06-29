using System.Collections;
using System.Collections.Generic;
using CodeBase.Controls.CubeRub;
using UnityEngine;

public class Cube : MonoBehaviour
{
  [SerializeField] private GameObject _cubeTemplate;

  private List<GameObject> _cubePartsList = new List<GameObject>();
  private Transform CubeTranform;
  private GameObject _centerPiece;
  private bool _canRotate = true;

  private List<GameObject> UpPieces
    => _cubePartsList.FindAll(x => Mathf.Round(x.transform.localPosition.y) == 0);

  private List<GameObject> DownPieces
    => _cubePartsList.FindAll(x => Mathf.Round(x.transform.localPosition.y) == -2);

  private List<GameObject> FrontPieces
    => _cubePartsList.FindAll(x => Mathf.Round(x.transform.localPosition.x) == 0);

  private List<GameObject> BackPieces
    => _cubePartsList.FindAll(x => Mathf.Round(x.transform.localPosition.x) == -2);

  private List<GameObject> LeftPieces
    => _cubePartsList.FindAll(x => Mathf.Round(x.transform.localPosition.z) == 0);

  private List<GameObject> RightPieces
    => _cubePartsList.FindAll(x => Mathf.Round(x.transform.localPosition.z) == 2);

  private void Start()
  {
    CubeTranform = transform;
    Create();
  }

  private void Update()
  {
    if (_canRotate)
      ChekInput();
  }

  private void Create()
  {
    for (int x = 0; x < 3; x++)
    {
      for (int y = 0; y < 3; y++)
      {
        for (int z = 0; z < 3; z++)
        {
          GameObject newPiece = Instantiate(_cubeTemplate, CubeTranform, false);
          newPiece.transform.localPosition = new Vector3(-x, -y, z);
          newPiece.GetComponent<CubePiece>().SetColor(-x, -y, z);

          _cubePartsList.Add(newPiece);
        }
      }
    }

    _centerPiece = _cubePartsList[13];
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
    
    else if (Input.GetKeyDown(KeyCode.Alpha6))
      StartCoroutine(Rotate(BackPieces, new Vector3(-1, 0, 0)));
  }

  IEnumerator Rotate(List<GameObject> listCubePieces, Vector3 rotationAxes)
  {
    _canRotate = false;
    int angele = 0;

    while (angele < 90)
    {
      foreach (GameObject piece in listCubePieces)
      {
        piece.transform.RotateAround(_centerPiece.transform.position, rotationAxes, 5);
      }

      angele += 5;
      yield return null; //TODO убрать привязку к кадрам
    }

    _canRotate = true;
  }
}