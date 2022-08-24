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
    public Action onCameraEffect;

    [SerializeField] PlayerController Player;

    int _score = 0;
    [SerializeField] Text scoreText;

    public float extraSpeed = 200f;

    [SerializeField] Transform damageText;
    int damageCount = 0;

    public void SetDamage(int num, Transform enemy)
    {
        var damage = damageText.GetChild(damageCount).GetComponent<TextMeshProUGUI>();
        damage.text = num + "";
        damage.transform.position = Camera.main.WorldToScreenPoint(enemy.position);

        damage.GetComponent<FloatingText>().Initialize(enemy);

        damage.gameObject.SetActive(true);

        damageCount++;
        if (damageCount >= damageText.childCount)
        {
            damageCount = 0;
        }
    }

    void CameraNoise()
    {
        var noise = cinemacine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        Sequence sequence = DOTween.Sequence();

        sequence.Append(DOTween.To(() => noise.m_FrequencyGain, gain => noise.m_FrequencyGain = gain, 5f, 0.125f));
        sequence.Append(DOTween.To(() => noise.m_FrequencyGain, gain => noise.m_FrequencyGain = gain, 0f, 0.125f));

        sequence.Play();
    }

    public void GetScore()
    {
        _score += 100;
        scoreText.text = _score.ToString();

        //Player.SetSpeed(extraSpeed);
    }

    private void OnEnable()
    {
        onCameraEffect += CameraNoise;
    }

    private void OnDisable()
    {
        onCameraEffect -= CameraNoise;
    }

    // Start is called before the first frame update
    void Start()
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
