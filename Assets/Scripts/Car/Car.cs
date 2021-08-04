using PathCreation;
using Score;
using UnityEngine;

namespace CubeRub.Car
{
    public class Car : MonoBehaviour
    {
        public PathCreator[] pathCreators;
        public float Speed;
        private float distanceTravelled;
        private int pathID;

        private PathCreator currentPath => pathCreators[pathID];
        private PathCreator nextPath => pathID+1 < pathCreators.Length ? pathCreators[pathID+1] : null;

        private void Start()
        {
            transform.SetParent(currentPath.transform);
        }

        private void Update()
        {
            if (distanceTravelled < currentPath.path.length-0.05f)
            {
                distanceTravelled += Speed * Time.deltaTime;
                transform.position = currentPath.path.GetPointAtDistance(distanceTravelled);
                transform.rotation = currentPath.path.GetRotationAtDistance(distanceTravelled);
            }
            else
            {
                if(nextPath == null) return;
                if (VectorTools.isPointsNear(nextPath.path.GetClosestPointOnPath(transform.position),transform.position))
                {
                    LevelScore.IncreaseScore();
                    distanceTravelled = 0;
                    pathID++;
                    transform.SetParent(currentPath.transform);
                }
            }
        }
    }
}
