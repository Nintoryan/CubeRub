using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Controls.CubeRub
{
  public class SpawnCube : MonoBehaviour
  {
    [SerializeField] private Transform _centerPivot;
    [SerializeField] private CubePiece _cubeTemplate;
    [SerializeField] private Vector3Int _size;
    private readonly List<GameObject> _cubePartsList = new List<GameObject>();
    
    public event Action Spawning;
    public List<GameObject> CubePartsList => _cubePartsList;
    public Transform CenterPiece => _centerPivot;

    private void Start()
    {
      Spawn();
    }

    private void Spawn()
    {
      for (int x = 0; x < _size.x; x++)
      {
        for (int y = 0; y < _size.y; y++)
        {
          for (int z = 0; z < _size.z; z++)
          {
            var newPiece = Instantiate(_cubeTemplate, transform, false);
            newPiece.Initialize(x, y, z, _size);
            _cubePartsList.Add(newPiece.gameObject);
            _centerPivot.position = GetCenterPosition();
            Spawning?.Invoke();
          }
        }
      }
    }

    private Vector3 GetCenterPosition()
    {
      return new Vector3((float) -_size.x / 2 + 0.5f, (float) -_size.y / 2 + 0.5f, (float) _size.z / 2);
    }
  }
}