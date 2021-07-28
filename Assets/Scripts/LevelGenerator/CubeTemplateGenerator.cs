using System.Collections.Generic;
using CubeRub.LevelGenerator;
using UnityEditor.SceneManagement;
using UnityEngine;

[RequireComponent(typeof(Cube))]
public class CubeTemplateGenerator : MonoBehaviour
{
    [Header("SpawningPart")] [SerializeField] private CubePiece _cubeTemplate;
    [Header("Size of the cube")] [SerializeField] private Vector3Int _size;
    [Header("Parent to spawn")] [SerializeField] private Transform _parent;

    public void Create()
    {
        var pieces = new List<GameObject>();
        for (var x = 0; x < _size.x; x++)
        {
            for (var y = 0; y < _size.y; y++)
            {
                for (var z = 0; z < _size.z; z++)
                {
                    var newPiece = Instantiate(_cubeTemplate, _parent, false);
                    newPiece.Initialize(x, y, z, _size);
                    pieces.Add(newPiece.gameObject);
                }
            }
        }
        FindObjectOfType<Cube>().SetSize(_size);
        if (FindObjectOfType<Prebuilt>() != null)
        {
            FindObjectOfType<Prebuilt>().AssignPieces(pieces);
        }
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }

    public void Delete()
    {
        var list = FindObjectsOfType<CubePiece>();
        foreach (var cubePiece in list)
        {
            DestroyImmediate(cubePiece.gameObject);
        }
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }
}
