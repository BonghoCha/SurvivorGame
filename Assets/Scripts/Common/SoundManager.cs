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
    private Sequence _playSequence;
    private Sequence _stopSequence;

    #region ### LiftCycles ###
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(this.gameObject);
            return;
        }

        Initialize();
    }
    #endregion

    
    #region ### Actions ###

    private void Initialize()
    {
        if (BGM == null)
        {
            BGM = transform.Find("BGM").GetComponent<AudioSource>();
        }
        if (Effect == null)
        {
            Effect = transform.Find("Effect").GetComponent<AudioSource>();
        }
        
        _playSequence = DOTween.Sequence();
        _stopSequence = DOTween.Sequence();
    }
    
    /// <summary>
    /// 사운드 재생
    /// </summary>
    /// <param name="name">오디오 클립 이름</param>
    /// <param name="immediately">즉시 재생할지 여부(BGM)</param>
    public void SoundPlay(string name)
    {
        var immediately = false;
        if (!clipDictionary.TryGetValue(name, out var clipData))
        {
            var clip = Resources.Load<AudioClip>($"Clips/{name}");
            clipData = new ClipData(clip);
            
            clipDictionary.Add(name, clipData);
        }

        if (name.Contains("BGM"))
        {

            var fadeDelay = 0f;
            if (BGM.isPlaying && !immediately)
            {
                fadeDelay = 0.5f;
            }

            _playSequence = DOTween.Sequence();
            _playSequence.Append(BGM.DOFade(0, fadeDelay).OnComplete(() =>
            {
                BGM.clip = clipData.audioClip;
                BGM.Play();
            }));
            _playSequence.Append(BGM.DOFade(1, fadeDelay));
            _playSequence.Play();
        }
        else
        {
            Effect.clip = clipData.audioClip;
            Effect.Play();
        }
    }

    /// <summary>
    /// 사운드 정지
    /// </summary>
    /// <param name="name"></param>
    /// <param name="immediately"></param>
    public void SoundStop(string name, bool immediately)
    {
        if (name.Contains("BGM"))
        {
            if (BGM.clip.name.Equals(name) && BGM.isPlaying)
            {
                if (immediately)
                {
                    BGM.Stop();    
                }
                else
                {
                    var fadeDelay = 0.5f;

                    _stopSequence = DOTween.Sequence();
                    _stopSequence.Append(BGM.DOFade(0, fadeDelay));
                    _stopSequence.Play();
                }
            }
        }
        else
        {
            if (Effect.clip.name.Equals(name) && Effect.isPlaying)
            {
                Effect.Stop();
            }
        }
    }
    
    /// <summary>
    /// 사운드 일시정지
    /// </summary>
    /// <param name="name"></param>
    public void SoundPause(string name)
    {
        if (name.Contains("BGM"))
        {
            if (BGM.clip.name.Equals(name) && BGM.isPlaying)
            {
                BGM.Pause();
            }
        }
        else
        {
            if (Effect.clip.name.Equals(name) && Effect.isPlaying)
            {
                Effect.Pause();
            }
        }
    }

    /// <summary>
    /// 사운드 다시 재생
    /// </summary>
    /// <param name="name"></param>
    public void SoundResume(string name)
    {
        if (name.Contains("BGM"))
        {
            if (BGM.clip.name.Equals(name))
            {
                BGM.UnPause();
            }
        }
        else
        {
            if (Effect.clip.name.Equals(name))
            {
                Effect.UnPause();
            }
        }
    }
    
    public void Clear()
    {
        if (_playSequence.IsPlaying())
        {
            _playSequence.Kill();
        }
        
        if (_stopSequence.IsPlaying())
        {
            _stopSequence.Kill();
        }
        
        BGM.Stop();
        Effect.Stop();
    }
    #endregion
}
