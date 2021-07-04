using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CodeBase.Controls.CubeRub
{
  public class CubePiece : MonoBehaviour
  {
    public PieceFace FrontFace;
    public PieceFace BackFace;
    public PieceFace UpFace;
    public PieceFace DownFace;
    public PieceFace LeftFace;
    public PieceFace RightFace;
    
    public List<CubeBigFace> Planes = new List<CubeBigFace>();
    private List<PieceFace> _activePieceFaces = new List<PieceFace>();

    public void Initialize(int x, int y, int z, Vector3Int size)
    {
      gameObject.name = $"{x} {y} {z}";
      transform.localPosition = new Vector3(-x, -y, z);
      if (x == 0)
      {
        FrontFace.gameObject.SetActive(true);
        _activePieceFaces.Add(FrontFace);
      }
      if (x == size.x - 1)
      {
        BackFace.gameObject.SetActive(true);
        _activePieceFaces.Add(BackFace);
      }
      if (y == 0)
      {
        UpFace.gameObject.SetActive(true);
        _activePieceFaces.Add(UpFace);
      }
      if (y == size.y - 1)
      {
        DownFace.gameObject.SetActive(true);
        _activePieceFaces.Add(DownFace);
      }
      if (z == 0)
      {
        LeftFace.gameObject.SetActive(true);
        _activePieceFaces.Add(LeftFace);
      }
      if (z == size.z - 1)
      {
        RightFace.gameObject.SetActive(true);
        _activePieceFaces.Add(RightFace);
      }
    }

    public void RecalculateFaces()
    {
      foreach (var pieceFace in _activePieceFaces)
      {
        pieceFace.Changebles.Clear();
        //pieceFace.Changebles.AddRange(Planes.Where(p=>p.similar == pieceFace.IgnorAxis));
      }
    }
  }
}