using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [SerializeField] CinemachineVirtualCamera cinemacine;
    public Action<float> onCameraEffect;

    public PlayerController Player;

    float _exp = 0;
    float _maxExp = 100;
    [SerializeField] Slider expSlider;

    public float extraSpeed = 200f;

    [SerializeField] Transform damageText;
    int damageCount = 0;

    public void SetDamage(float num, Transform enemy, bool isCritical = false)
    {
        var damage = damageText.GetChild(damageCount).GetComponent<TextMeshProUGUI>();
        damage.text = "-" + Math.Floor(num);
        damage.transform.position = Camera.main.WorldToScreenPoint(enemy.position);

        damage.GetComponent<FloatingText>().Initialize(enemy, isCritical);

        damage.gameObject.SetActive(true);

        damageCount++;
        if (damageCount >= damageText.childCount)
        {
            damageCount = 0;
        }
    }

    void CameraNoise(float num)
    {
        var noise = cinemacine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        Sequence sequence = DOTween.Sequence();
        sequence.OnStart(() =>
        {
            noise.m_AmplitudeGain = num;
        });
        sequence.Append(DOTween.To(() => noise.m_FrequencyGain, gain => noise.m_FrequencyGain = gain, 5f, 0.125f));
        sequence.Append(DOTween.To(() => noise.m_FrequencyGain, gain => noise.m_FrequencyGain = gain, 0f, 0.125f));
        sequence.OnComplete(() =>
        {
            noise.m_AmplitudeGain = 1;
        });

        sequence.Play();
    }

    public void GetEXP()
    {
        _exp += 10f;

        if (_exp >= _maxExp)
        {
            expSlider.value = 1;
            _exp = 0;
            Debug.Log("레벨업");
        }

        expSlider.value = _exp / _maxExp;
    }

    private void OnEnable()
    {
        onCameraEffect += CameraNoise;
    }

    private void OnDisable()
    {
        onCameraEffect -= CameraNoise;
    }

    private void Awake()
    {
        if (instance == null)
        {
            var count = FindObjectsOfType<GameManager>();
            if (count.Length > 1) return;

            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
