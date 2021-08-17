using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CubeRub.Controls.CubeRub;
using PathCreation;
using Score;
using UnityEngine;
using UnityEngine.Events;

namespace CubeRub.Car
{
    public class PathFollower : MonoBehaviour
    {
        [SerializeField] private PathCreator _startPath;
        [SerializeField] private float Speed;
        private List<PathCreator> pathCreators = new List<PathCreator>();
        private float distanceTravelled;
        private PathCreator currentPath;
        private UnityAction OnPathDone;

        [SerializeField]private Transform _scorePoint;

        private void Awake()
        {
            pathCreators = FindObjectsOfType<PathCreator>().ToList();
        }

        public void GoFirstPath()
        {
            if(_startPath == null) return;
            
            currentPath = _startPath;
            CubeRotation.OnCubeRotated += TryNextPath;
            OnPathDone += TryNextPath;

            GoThroughPath(currentPath);
        }

        private void TryNextPath()
        {
            var searchingPool = pathCreators;
            searchingPool.Remove(currentPath);
            searchingPool = searchingPool.FindAll(p => p != null);

            var nextPath = searchingPool.Find(p =>
                VectorTools.isPointsNear(p.path.GetPointAtDistance(0), transform.position));
            if(nextPath == null) return;
            
            currentPath = nextPath;
            
            LevelScore.IncreaseScore(_scorePoint.position);
            #if UNITY_ANDROID
            if(Settings.isVibrationOn)
                Handheld.Vibrate();
            #endif
            distanceTravelled = 0;
            transform.SetParent(currentPath.transform);
            GoThroughPath(currentPath);
        }

        private void GoThroughPath(PathCreator path)
        {
            transform.SetParent(path.transform);
            StartCoroutine(IEFollowPath(path));
        }
        
        private IEnumerator IEFollowPath(PathCreator path)
        {
            if(path == null) yield break;
            while (distanceTravelled < currentPath.path.length-0.05f)
            {
                distanceTravelled += Speed * Time.fixedDeltaTime;
                transform.position = currentPath.path.GetPointAtDistance(distanceTravelled);
                transform.rotation = currentPath.path.GetRotationAtDistance(distanceTravelled);
                yield return new WaitForFixedUpdate();
            }
            OnPathDone?.Invoke();
        }
    }
}
