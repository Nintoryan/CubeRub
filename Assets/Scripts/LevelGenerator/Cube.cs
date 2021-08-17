using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CubeRub.LevelGenerator
{
    public abstract class Cube : MonoBehaviour
    {
        [Header("Size of the cube")] [SerializeField] protected Vector3Int _size;
        [Header("Center")] [SerializeField] protected Transform _centerPivot;
        protected readonly List<GameObject> _cubePartsList = new List<GameObject>();

        public List<GameObject> CubePartsList => _cubePartsList;
        public Transform CenterPiece => _centerPivot;

        public event UnityAction OnCalculated;

        protected void InvokeOnCalculated()
        {
            OnCalculated?.Invoke();
        }
        
        private void Start()
        {
            Calculate();
        }

        protected virtual void Calculate()
        {
            _centerPivot.position = GetCenterPosition();
        }

        private Vector3 GetCenterPosition()
        {
            return new Vector3((float) -_size.x / 2 + 0.5f, (float) -_size.y / 2 + 0.5f, (float) _size.z / 2);
        }

        public void SetSize(Vector3Int newSize)
        {
            if (!Application.isEditor)
            {
                Debug.LogError("Cannot change size on runtime!");
            }

            _size = newSize;
        }
    }
}