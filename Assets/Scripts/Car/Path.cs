using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class Path : MonoBehaviour
{
    [SerializeField] private List<PathPoint> _pathPoints;

    public List<PathPoint> PathPoints => _pathPoints;

    public Transform LastPoint => _pathPoints.Last().transform;
    public Transform FirstPoint => _pathPoints.First().transform;

    public Vector3 LastPosition => _pathPoints.Last().transform.position;
    public Vector3 FirstPosition => _pathPoints.First().transform.position;

    public Vector3[] PathPositions => _pathPoints.Select(p => p.Position).ToArray();

    private void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var pathPoint = transform.GetChild(i).GetComponent<PathPoint>();
            if(pathPoint == null) continue;
            if (!PathPoints.Contains(pathPoint))
            {
                PathPoints.Add(pathPoint);
            }
        }
    }
}
