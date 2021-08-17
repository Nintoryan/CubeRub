using UnityEngine;
using UnityEngine.SceneManagement;

public class Initializer : MonoBehaviour
{
    private void Awake()
    {
        SceneManager.LoadScene(LevelProgressHandler.CurrentLevelID%(SceneManager.sceneCountInBuildSettings-1)+1);
    }
}
