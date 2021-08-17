using CubeRub.Controls.CubeRub;
using UnityEngine;

public class Guide : MonoBehaviour
{
    private void OnEnable()
    {
        CubeRotation.OnCubeRotated += Deactivate;
    }

    private void OnDisable()
    {
        CubeRotation.OnCubeRotated -= Deactivate;
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
    

}
