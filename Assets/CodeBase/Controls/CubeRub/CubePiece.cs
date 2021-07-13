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

    public void Initialize(int x, int y, int z, Vector3Int size)
    {
      gameObject.name = $"{x} {y} {z}";
      transform.localPosition = new Vector3(-x, -y, z);
      if (x == 0)
      {
        FrontFace.gameObject.SetActive(true);
      }
      if (x == size.x - 1)
      {
        BackFace.gameObject.SetActive(true);
      }
      if (y == 0)
      {
        UpFace.gameObject.SetActive(true);
      }
      if (y == size.y - 1)
      {
        DownFace.gameObject.SetActive(true);
      }
      if (z == 0)
      {
        LeftFace.gameObject.SetActive(true);
      }
      if (z == size.z - 1)
      {
        RightFace.gameObject.SetActive(true);
      }
    }
  }
}