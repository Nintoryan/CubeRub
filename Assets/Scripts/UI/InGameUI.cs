using CubeRub.Car;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{
    private static bool isInMenu = true;
    
    [SerializeField] private RectTransform _levelCompleteWindow;
    [SerializeField] private RectTransform _gameplayWindow;
    [SerializeField] private RectTransform _menuWindow;
    [SerializeField] private TMP_Text _levelName;


    private void Start()
    {
        _gameplayWindow.gameObject.SetActive(!isInMenu);
        _levelName.text = $"LEVEL {LevelProgressHandler.CurrentLevelID + 1}";
        _menuWindow.gameObject.SetActive(isInMenu);
        if (!isInMenu)
        {
            TapToPlay();    
        }
    }

    public void OpenLevelCompleteWindow()
    {
        _gameplayWindow.gameObject.SetActive(false);

        _levelCompleteWindow.gameObject.SetActive(true);
    }

    public void NextLevel()
    {
        LevelProgressHandler.CurrentLevelID++;
        SceneManager.LoadScene(LevelProgressHandler.CurrentLevelID%(SceneManager.sceneCountInBuildSettings-1)+1);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void TapToPlay()
    {
        _menuWindow.gameObject.SetActive(false);
        _gameplayWindow.gameObject.SetActive(true);
        FindObjectOfType<PathFollower>().GoFirstPath();
        isInMenu = false;
    }

    public void OpenMenu()
    {
        isInMenu = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
