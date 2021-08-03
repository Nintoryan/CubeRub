using System.Collections.Generic;
using System.Linq;
using CubeRub.Car;
using CubeRub.Controls.CubeRub;
using UnityEngine;

[ExecuteInEditMode]
public class Path : MonoBehaviour
{
    [SerializeField] private List<PathPoint> _pathPoints;
    [SerializeField] private Axis _sameAxis;

    public Axis SameAxis => _sameAxis;

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

        for (int i = 0; i < _pathPoints.Count-1; i++)
        {
            _pathPoints[i].Initialize(_pathPoints[i + 1].Position, this);
        }
        _pathPoints[_pathPoints.Count-1].Initialize(_pathPoints[_pathPoints.Count-2].Position,this);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        for (var j = 1; j < _pathPoints.Count; j++)
        {
            if (_pathPoints[j - 1] == null || _pathPoints[j] == null) continue;
            if (FollowPath.isPointsNear(_pathPoints[j - 1].transform, _pathPoints[j].transform))
                Gizmos.DrawLine(_pathPoints[j - 1].Position, _pathPoints[j].Position);

        }
    }
}
