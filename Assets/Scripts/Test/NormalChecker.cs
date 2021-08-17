using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalChecker : MonoBehaviour
{
    [SerializeField] private Transform Center;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position,0.2f);
        var ray = new Ray(transform.position,Center.position-transform.position);
        Gizmos.DrawLine(transform.position,Center.position);
        if (Physics.Raycast(ray, out var hit))
        {
            Debug.Log(hit.collider.gameObject.name);
            var position = transform.position;
            Gizmos.DrawLine(position,position+hit.normal);
        }
        else
        {Debug.Log("Nothing intersects");
            
        }
    }
}
