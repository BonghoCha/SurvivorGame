using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private AudioSource BGM;
    [SerializeField] private AudioSource Effect;

    private Dictionary<string, ClipData> clipDictionary = new Dictionary<string, ClipData>();

    public void SoundPlay(string name)
    {
        if (!clipDictionary.TryGetValue(name, out var clipData))
        {
            var clip = Resources.Load<AudioClip>($"Clips/{name}");
            clipData = new ClipData(clip);
            
            clipDictionary.Add(name, clipData);
        }

        Effect.clip = clipData.audioClip;
        
        Effect.Play();
    }
}
