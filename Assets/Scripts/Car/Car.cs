using CubeRub.Car;
using UnityEngine;

public class Car : MonoBehaviour
{
    public TriggersDetector _Detector;
    [SerializeField] private InGameUI _inGameUI;
    

    private void Awake()
    {
        if (_inGameUI == null)
        {
            _inGameUI = FindObjectOfType<InGameUI>();
            if (_inGameUI == null)
            {
                Debug.LogError("No inGameUI detected!");
            }
        }

        if (_Detector == null)
        {
            _Detector = GetComponentInChildren<TriggersDetector>();
        }
    }

    private void OnEnable()
    {
        _Detector.OnFinishReached += _inGameUI.OpenLevelCompleteWindow;
    }

    private void OnDestroy()
    {
        _Detector.OnFinishReached -= _inGameUI.OpenLevelCompleteWindow;
    }
}
