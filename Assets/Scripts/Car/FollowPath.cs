using System.Collections.Generic;
using System.Linq;
using CubeRub.Controls.CubeRub;
using CubeRub.LevelGenerator;
using DG.Tweening;
using Score;
using UnityEngine;
using UnityEngine.Events;

namespace CubeRub.Car
{
    public class FollowPath : MonoBehaviour
    {
        [SerializeField] private List<CubePiece> _cubePieces;
        [SerializeField] private float Speed;
        [SerializeField] private float NearPoinTreshold;

        private int _currentID;
        private Path CurrentPath => _cubePieces[_currentID].carPath;

        private Path NextPath => _currentID + 1 < _cubePieces.Count ? _cubePieces[_currentID + 1].carPath : null;
        
        private bool isGoing;
        private UnityAction OnCarStoped;
        private void OnDrawGizmos()
        {
            foreach (var t in _cubePieces)
            {
                Gizmos.color = Color.black;
                for (var j = 1; j < t.carPath.PathPoints.Count; j++)
                {
                    if (t.carPath.PathPoints[j - 1] == null || t.carPath.PathPoints[j] == null) continue;
                    if (isPointsNear(t.carPath.PathPoints[j - 1].transform, t.carPath.PathPoints[j].transform))
                        Gizmos.DrawLine(t.carPath.PathPoints[j - 1].Position, t.carPath.PathPoints[j].Position);

                }
            }
        }

        private void OnEnable()
        {
            CubeRotation.OnCubeRotated += TryToGo;
        }

        private void OnDestroy()
        {
            CubeRotation.OnCubeRotated -= TryToGo;
        }

        private void Start()
        {
            transform.position = CurrentPath.FirstPosition;
            transform.SetParent(CurrentPath.transform);
            GoThroughPath(CurrentPath.PathPositions.Where(p => p != CurrentPath.FirstPosition).ToArray());
        }

        private void TryToGo()
        {
            if (!isPathsNear(CurrentPath, NextPath)) return;
            if (isGoing)
            {
                OnCarStoped += TryToGo;
                return;
            }
            LevelScore.IncreaseScore();
            transform.SetParent(NextPath.transform);
            transform.DOLookAt(NextPath.FirstPosition, 0.1f);
            GoThroughPath(NextPath.PathPositions);
            _currentID++;
        }
        
        private void GoThroughPath(IEnumerable<Vector3> path)
        {
            var s = DOTween.Sequence();
            isGoing = true;
            foreach (var point in path)
            {
                s.AppendCallback(() =>
                {
                    var ray = new Ray(transform.position, CurrentPath.transform.position);
                    if (Physics.Raycast(ray, out var hit))
                    {
                        var pieceFace = hit.collider.gameObject.GetComponent<PieceFace>();
                        if (pieceFace != null)
                        {
                            transform.LookAt(point, -pieceFace.transform.forward);
                        }
                    }
                });
                s.Append(transform.DOMove(point, Speed).SetEase(Ease.Linear).SetSpeedBased(true));
            }
            s.AppendCallback(() =>
            {
                isGoing = false;
                OnCarStoped?.Invoke();
            });
        }

        private bool isPathsNear(Path p1, Path p2)
        {
            if (p1 == null || p2 == null) return false;
            return isPointsNear(p1.LastPoint, p2.FirstPoint) || isPointsNear(p1.FirstPoint, p2.LastPoint);
        }

        private bool isPointsNear(Transform p1, Transform p2)
        {
            if (p1 == null || p2 == null) return false;
            return Vector3.Distance(p1.position, p2.position) < NearPoinTreshold;
        }
    }
}
