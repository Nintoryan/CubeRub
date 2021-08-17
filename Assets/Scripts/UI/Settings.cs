using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [Header("Settings icon")] 
    [SerializeField] private RectTransform SettingsButton;
    [SerializeField] private RectTransform SettingsSmallPanel;
    private bool isOpened;
    
    [Header("Sound")] 
    [SerializeField] private Image Sound;
    [SerializeField] private Sprite SoundOn;
    [SerializeField] private Sprite SoundOff;
    [SerializeField] private AudioMixer _mainMixer;

    [Header("Vibration")] 
    [SerializeField] private Image Vibration;
    [SerializeField] private Sprite VibrationOn;
    [SerializeField] private Sprite VibrationOff;

    public static bool isSoundOn
    {
        get => PlayerPrefs.GetInt("isSoundOn") == 0;
        set => PlayerPrefs.SetInt("isSoundOn", value ? 0 : 1);
    }

    public static bool isVibrationOn
    {
        get => PlayerPrefs.GetInt("isVibrationOn") == 0;
        set => PlayerPrefs.SetInt("isVibrationOn", value ? 0 : 1);
    }

    private void Start()
    {
        _mainMixer.SetFloat("Volume", isSoundOn ? 0 : -80f);
        Sound.sprite = isSoundOn ? SoundOn : SoundOff;
        Vibration.sprite = isVibrationOn ? VibrationOn : VibrationOff;
    }

    public void OpenSmallSettingsPanel()
    {
        if (!isOpened)
        {
            SettingsButton.DORotate(new Vector3(0,0,180),0.5f, RotateMode.FastBeyond360).SetRelative();
            SettingsSmallPanel.DOAnchorPosX(-80, 0.5f);
        }
        else
        {
            SettingsButton.DORotate(new Vector3(0,0,-180),0.5f, RotateMode.FastBeyond360).SetRelative();
            SettingsSmallPanel.DOAnchorPosX(80, 0.5f);
        }

        isOpened = !isOpened;

    }
    public void SwitchSound()
    {
        isSoundOn = !isSoundOn;
        _mainMixer.SetFloat("Volume", isSoundOn ? 0 : -80f);
        Sound.sprite = isSoundOn ? SoundOn : SoundOff;
    }

    public void SwitchVibration()
    {
        isVibrationOn = !isVibrationOn;
        Vibration.sprite = isVibrationOn ? VibrationOn : VibrationOff;
    }
}
