using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Controls.CubeRub
{
  public class SpawnCube : MonoBehaviour
  {
    [SerializeField] private GameObject _cubeTemplate;
    [SerializeField] private Vector3 _size;

    private List<GameObject> _cubePartsList = new List<GameObject>();
    
    public List<GameObject> CubePartsList => _cubePartsList;

    public GameObject CenterPiece { get; private set; }

    private void Start() =>
      Spawn();

    private void Spawn()
    {
      for (int x = 0; x < _size.x; x++)
      {
        for (int y = 0; y < _size.y; y++)
        {
          for (int z = 0; z < _size.z; z++)
          {
            GameObject newPiece = Instantiate(_cubeTemplate, transform, false);
            newPiece.name = $"{x} {y} {z}";
            newPiece.transform.localPosition = new Vector3(-x, -y, z);
            newPiece.GetComponent<CubePiece>().SetColor(-x, -y, z);

            _cubePartsList.Add(newPiece);
          }
        }
      }

      CenterPiece = _cubePartsList[(int)(_size.x * _size.y * _size.z / 2)];
    }
  }
}