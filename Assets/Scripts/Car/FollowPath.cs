using System;
using System.Collections.Generic;
using CubeRub.Controls.CubeRub;
using DG.Tweening;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    [SerializeField] private List<Transform> Points;
    [SerializeField] private float Speed;
    [SerializeField] private float NearPoinTreshold;

    private int _currentID = 0;
    private Transform CurrentPoint => Points[_currentID];

    private Transform NextPoint
    {
        get
        {
            if (_currentID + 1 < Points.Count)
            {
                return Points[_currentID + 1];
            }
            else
            {
                return null;
            }
        }
    }

    private void OnDrawGizmos()
    {
        
        
        for (int i = 0; i < Points.Count; i++)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(Points[i].position,NearPoinTreshold/2);
            if(i == Points.Count-1) return;
            Gizmos.color = Color.black;
            if(isPointsNear(Points[i],Points[i+1]))
                Gizmos.DrawLine(Points[i].position,Points[i+1].position);
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
        transform.position = CurrentPoint.position;
        TryToGo();
    }

    private void TryToGo()
    {
        var s = DOTween.Sequence();
        while (isPointsNear(CurrentPoint,NextPoint))
        {
            //s.AppendCallback(() => { transform.LookAt(NextPoint); });
            s.Append(transform.DOMove(NextPoint.position, Speed).SetSpeedBased());
            _currentID++;
        }
    }
    private bool isPointsNear(Transform p1, Transform p2)
    {
        if (p1 == null || p2 == null) return false;
        return Vector3.Distance(p1.position, p2.position) < NearPoinTreshold;
    }
}
