using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProgressHandler : MonoBehaviour
{
    public static int CurrentLevelID
    {
        get => PlayerPrefs.GetInt("CurrentLevelID");
        set => PlayerPrefs.SetInt("CurrentLevelID", value);
    }
}
