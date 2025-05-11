using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_Settings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [Header("SFX Settings")]
    [SerializeField] private Slider sfxslider;
    [SerializeField] private TextMeshProUGUI sfxText;   

    [Header("BGM Settings")]
    [SerializeField] private Slider bgmslider;
    [SerializeField] private TextMeshProUGUI bgmText;
    public void sfxSliderValue(float value)
    {
        sfxText.text = Mathf.RoundToInt(value * 100) + "%";
        PlayerPrefs.SetFloat("sfx", sfxslider.value);
        AudioManager.instance?.LoadSettings();
    }
    public void bgmSliderValue(float value)
    {
        bgmText.text = Mathf.RoundToInt(value * 100) + "%";
        PlayerPrefs.SetFloat("bgm", bgmslider.value);
        AudioManager.instance?.LoadSettings();
    }
    private void OnEnable()
    {
        sfxslider.value = PlayerPrefs.GetFloat("sfx", 1);
        bgmslider.value = PlayerPrefs.GetFloat("bgm", 1);
    }


}
