using System.Collections;
using CubeRub.Controls.CubeRub;
using PathCreation;
using Score;
using UnityEngine;
using UnityEngine.Events;

namespace CubeRub.Car
{
    public class PathFollower : MonoBehaviour
    {
        public PathCreator[] pathCreators;
        public float Speed;
        private float distanceTravelled;
        private int pathID;
        
        private PathCreator currentPath => pathCreators.Length > 0 ? pathCreators[pathID] : null;
        private PathCreator nextPath => pathID+1 < pathCreators.Length ? pathCreators[pathID+1] : null;

        private UnityAction OnPathDone;

        private void Start()
        {
            if(currentPath == null) return;

            CubeRotation.OnCubeRotated += TryNextPath;
            OnPathDone += TryNextPath;

            GoThroughPath(currentPath);
        }

        private void TryNextPath()
        {
            if(nextPath == null) return;
            Debug.Log(Vector3.Distance(nextPath.path.GetClosestPointOnPath(transform.position),transform.position));
            if (VectorTools.isPointsNear(nextPath.path.GetClosestPointOnPath(transform.position),transform.position))
            {
                LevelScore.IncreaseScore();
                distanceTravelled = 0;
                pathID++;
                transform.SetParent(currentPath.transform);
                GoThroughPath(currentPath);
                
            }
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
