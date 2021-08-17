using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace CubeRub.LevelGenerator
{
    public class Prebuilt : Cube
    {
        [Header("List of cube parts in X->Y->Z increment order!")] [SerializeField]
        
        private List<GameObject> _cubePieces = new List<GameObject>();
        protected override void Calculate()
        {
            base.Calculate();
            foreach (var cubePiece in _cubePieces)
            {
                _cubePartsList.Add(cubePiece);
            }
            InvokeOnCalculated();
            
        }

        public void AssignPieces(List<GameObject> cubePieces)
        {
            if (!Application.isEditor)
            {
                Debug.LogError("Cannot assign pieces on runtime!");
            }
            _cubePieces = cubePieces;
        }
    }
}
