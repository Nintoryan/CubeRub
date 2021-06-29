using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Controls.CubeRub
{
  public class SpawnCube : MonoBehaviour
  {
    [SerializeField] private Transform _centerPivot;
    [SerializeField] private GameObject _cubeTemplate;
    [SerializeField] private Vector3 _size;

    private readonly List<GameObject> _cubePartsList = new List<GameObject>();

    public event Action Spawning;

    public List<GameObject> CubePartsList => _cubePartsList;

    public Transform CenterPiece => _centerPivot;

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

            _centerPivot.position = GetCenterPosition();
            Spawning?.Invoke();
          }
        }
      }
    }

    private Vector3 GetCenterPosition()
    {
      return new Vector3(-_size.x / 2 + 0.5f, -_size.y / 2 + 0.5f, _size.z / 2);
      // return _cubePartsList[(int) (_size.x * _size.y * _size.z / 2)].transform.position;
    }
  }
}