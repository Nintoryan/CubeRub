using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Controls.CubeRub
{
    [Serializable]
    public class CubeBigFace
    {
        public List<GameObject> FaceCubePieces = new List<GameObject>();
        public Vector3 similar;

        public CubeBigFace(List<GameObject> _pieces)
        {
            FaceCubePieces = _pieces;
            foreach (var piece in _pieces)
            {
                piece.GetComponent<CubePiece>().Planes.Add(this);
            }
            //similar = _similar;
        }
    }
}