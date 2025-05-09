using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefs : MonoBehaviour
{
    public static PlayerPrefs current;

    private void Awake()
    {
        if (current == null)
            current = this;

        DontDestroyOnLoad(this.gameObject);
    }

    public float mouseSensitivity;

    public float masterVolume;
    public float bgmVolume;
    public float sfxVolume;
}