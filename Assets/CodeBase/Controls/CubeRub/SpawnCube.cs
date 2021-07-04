using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CodeBase.Controls.CubeRub
{
  public class SpawnCube : MonoBehaviour
  {
    [SerializeField] private Transform _centerPivot;
    [SerializeField] private CubePiece _cubeTemplate;
    [SerializeField] private Vector3Int _size;
    [SerializeField] private CubeRotation _cubeRotation;

    private readonly List<GameObject> _cubePartsList = new List<GameObject>();
    private List<CubeBigFace> allBigFaces;

    public event Action Spawning;

    public List<GameObject> CubePartsList => _cubePartsList;

    public Transform CenterPiece => _centerPivot;

    private void Start()
    {
      Spawn();
      _cubeRotation.Roating += RecalculateWholeCube;
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
            newPiece.Initialize(x,y,z,_size);
            _cubePartsList.Add(newPiece.gameObject);
            _centerPivot.position = GetCenterPosition();
            Spawning?.Invoke();
          }
        }
      }
      RecalculateWholeCube();
    }
    private Vector3 GetCenterPosition()
    {
      return new Vector3((float)-_size.x / 2 + 0.5f, (float)-_size.y / 2 + 0.5f, (float)_size.z / 2);
    }
    
    private void RecalculateWholeCube()
    {
      foreach (var _gameObject in _cubePartsList)
      {
        _gameObject.GetComponent<CubePiece>().Planes = new List<CubeBigFace>();
      }
      RecalculateBigFaces();
      foreach (var _gameObject in _cubePartsList)
      {
        _gameObject.GetComponent<CubePiece>().RecalculateFaces();
      }
    }
    private void RecalculateBigFaces()
         {
           var _cubeBigFaces = new List<CubeBigFace>();
           
           for (int i = 0; i < Mathf.Max(Mathf.Max(_size.x, _size.y), _size.z); i++)
           {
             var simXFace = _cubePartsList.Where(c => Math.Abs(c.transform.localPosition.x - -i) < 0.01f).ToList();
             var simYFace = _cubePartsList.Where(c => Math.Abs(c.transform.localPosition.y - -i) < 0.01f).ToList();
             var simZFace = _cubePartsList.Where(c => Math.Abs(c.transform.localPosition.z - i) < 0.01f).ToList();
             if (_size.x >= i)
             {
               _cubeBigFaces.Add(new CubeBigFace(simXFace));
             }
             if (_size.y >= i)
             {
               _cubeBigFaces.Add(new CubeBigFace(simYFace));
             }
             if (_size.z >= i)
             {
               _cubeBigFaces.Add(new CubeBigFace(simZFace));
             }
           }
           allBigFaces = _cubeBigFaces;
         }
  }
}