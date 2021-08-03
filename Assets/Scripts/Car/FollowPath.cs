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
        private static float NearPoinTreshold = 0.2f;
        
        [SerializeField] private List<CubePiece> _cubePieces;
        [SerializeField] private float Speed;
        

        private int _currentID;
        private Path CurrentPath => _cubePieces[_currentID].carPath;

        private Path NextPath => _currentID + 1 < _cubePieces.Count ? _cubePieces[_currentID + 1].carPath : null;
        
        private bool isGoing;
        private UnityAction OnCarStoped;

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
            GoThroughPath(CurrentPath.PathPoints.Where(p => p != CurrentPath.PathPoints.First()).ToArray());
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
            GoThroughPath(NextPath.PathPoints);
            _currentID++;
        }
        
        private void GoThroughPath(IEnumerable<PathPoint> path)
        {
            var s = DOTween.Sequence();
            isGoing = true;
            foreach (var point in path)
            {
                s.AppendCallback(() =>
                {
                    transform.LookAt(point.Position, transform.up);
                });
                s.Append(transform.DOMove(point.Position, Speed).SetEase(Ease.Linear).SetSpeedBased(true));
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

        public static bool isPointsNear(Transform p1, Transform p2)
        {
            if (p1 == null || p2 == null) return false;
            return Vector3.Distance(p1.position, p2.position) < NearPoinTreshold;
        }

        
    }
}
