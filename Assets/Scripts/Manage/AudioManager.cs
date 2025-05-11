using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioMixer audioMixer;
    [Header("Audio Sources")]
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;
    private int currentBGMIndex = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            
        }
        else
        {
            Destroy(gameObject);
        }
        if (bgm.Length <= 0)
            return;

        InvokeRepeating(nameof(PlayMusicIfNeed), 0, 2);
    }
    private void Start()
    {
        LoadSettings();
    }
    public void PlaySFX(int sfxPlay,bool randomPitch=true)
    {
        if (sfxPlay >= sfx.Length)
            return;
        
        if (randomPitch)
        {
            sfx[sfxPlay].pitch = Random.Range(0.8f, 1.2f);
        }
        sfx[sfxPlay].Play();
    }
    public void StopSFX(int sfxStop)
    {
        if (sfxStop >= sfx.Length)
            return;
        sfx[sfxStop].Stop();
    }
    public void PlayRandomBGM() { 
        currentBGMIndex = Random.Range(0, bgm.Length);
        PlayBGM(currentBGMIndex);
    }
    public void PlayMusicIfNeed()
    {
        if(bgm[currentBGMIndex].isPlaying==false)
            PlayRandomBGM();
    }
    public void PlayBGM(int bgmPlay)
    {
        if (bgm.Length <= 0)
        {
            Debug.LogWarning("No BGM found");
            return;
        }
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
        currentBGMIndex = bgmPlay;
        bgm[bgmPlay].Play();
    }
    public void LoadSettings()
    {
        audioMixer.SetFloat("sfx", GetVolumeValue(PlayerPrefs.GetFloat("sfx", 1)));
        audioMixer.SetFloat("bgm", GetVolumeValue(PlayerPrefs.GetFloat("bgm", 1)));
    }
    private float GetVolumeValue(float value) {
        return Mathf.Log10(value) * 25;
    }
}
