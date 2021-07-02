using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Controls.CubeRub
{
    [Serializable]
    public class CubeBigFace
    {
        public List<GameObject> FaceCubePieces = new List<GameObject>();
        public Similar Similar;

        public CubeBigFace(List<GameObject> _pieces, Similar _similar)
        {
            FaceCubePieces = _pieces;
            foreach (var piece in _pieces)
            {
                piece.GetComponent<CubePiece>().Planes.Add(this);
            }
            Similar = _similar;
        }
    }

    public class Similar
    {
        public Axis _similarAxis;
        public int _axisPosition;

        public Similar(Axis axis, int pos)
        {
            _similarAxis = axis;
            _axisPosition = pos;
        }
    }

    public enum Axis
    {
        x,y,z
    }
}