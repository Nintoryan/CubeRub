using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CodeBase.Controls.CubeRub
{
  public class SpawnCube : MonoBehaviour
  {
    [SerializeField] private Transform _centerPivot;
    [SerializeField] private GameObject _cubeTemplate;
    [SerializeField] private Vector3Int _size;

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

      List<CubeBigFace> _cubeBigFaces = new List<CubeBigFace>();
      
      for (int i = 0; i < Mathf.Max(Mathf.Max(_size.x, _size.y), _size.z); i++)
      {
        var simXFace = _cubePartsList.Where(c => Math.Abs(c.transform.localPosition.x - -i) < 0.01f).ToList();
        var simYFace = _cubePartsList.Where(c => Math.Abs(c.transform.localPosition.y - -i) < 0.01f).ToList();
        var simZFace = _cubePartsList.Where(c => Math.Abs(c.transform.localPosition.z - i) < 0.01f).ToList();
        
        if (_size.x >= i)
        {
          _cubeBigFaces.Add(new CubeBigFace(simXFace, new Similar(Axis.x,-i)));
        }
        if (_size.y >= i)
        {
          _cubeBigFaces.Add(new CubeBigFace(simYFace, new Similar(Axis.y,-i)));
        }
        if (_size.z >= i)
        {
          _cubeBigFaces.Add(new CubeBigFace(simZFace, new Similar(Axis.z,-i)));
        }
      }
    }

    private Vector3 GetCenterPosition() => 
      new Vector3((float)-_size.x / 2 + 0.5f, (float)-_size.y / 2 + 0.5f, (float)_size.z / 2);
  }
}