using UnityEngine;

namespace CubeRub.LevelGenerator
{
  public class Spawed : Cube
  {
    [Header("SpawningPart")] [SerializeField] private CubePiece _cubeTemplate;
    protected override void Calculate()
    {
      base.Calculate();
      
      for (var x = 0; x < _size.x; x++)
      {
        for (var y = 0; y < _size.y; y++)
        {
          for (var z = 0; z < _size.z; z++)
          {
            var newPiece = Instantiate(_cubeTemplate, transform, false);
            newPiece.Initialize(x, y, z, _size);
            _cubePartsList.Add(newPiece.gameObject);
          }
        }
      }
      InvokeOnCalculated();
    }
  }
}