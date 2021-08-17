using System;
using UnityEngine;

public class Guide1 : MonoBehaviour
{
    private void FixedUpdate()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Destroy(gameObject);
        }
    }
}
