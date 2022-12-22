using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ClipData
{
    public AudioClip audioClip;
    public string name;

    public ClipData(AudioClip _audioClip, string _name = "")
    {
        audioClip = _audioClip;
        name = string.IsNullOrEmpty(_name)? audioClip.name : _name;
    }
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;
    
    [SerializeField] private AudioSource BGM;
    [SerializeField] private AudioSource Effect;

    private Dictionary<string, ClipData> clipDictionary = new Dictionary<string, ClipData>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }else
        {
            Destroy(this.gameObject);
        }
    }

    public void SoundPlay(string name)
    {
        if (!clipDictionary.TryGetValue(name, out var clipData))
        {
            var clip = Resources.Load<AudioClip>($"Clips/{name}");
            clipData = new ClipData(clip);
            
            clipDictionary.Add(name, clipData);
        }

        if (name.Contains("BGM"))
        {
            Sequence bgmSequence = DOTween.Sequence();

            var fadeDelay = 0f;
            if (BGM.isPlaying)
            {
                fadeDelay = 0.5f;
            }

            bgmSequence.Append(BGM.DOFade(0, fadeDelay).OnComplete(() =>
            {
                BGM.clip = clipData.audioClip;
                BGM.Play();
            }));
            bgmSequence.Append(BGM.DOFade(1, fadeDelay));

            bgmSequence.Play();
        }
        else
        {
            Effect.PlayOneShot(clipData.audioClip);
        }
    }

    public void Clear()
    {
        BGM.Stop();
        Effect.Stop();
        DOTween.KillAll();
    }
}
