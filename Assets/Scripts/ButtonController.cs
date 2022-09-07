using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public enum ButtonType
    {
        Shot,
        Dash
    }
    public ButtonType type;

    [SerializeField] Image CooltimeImage;

    [Range(0, 15f)] [SerializeField] float cooltime = 0.5f;
    public bool isCooltime = false;

    public Action onClick;

    private void OnEnable()
    {        
        onClick += Cooltime;
    }

    private void OnDisable()
    {
        onClick -= Cooltime;
    }

    void Cooltime()
    {
        if (isCooltime) return;

        Sequence sequence = DOTween.Sequence();
        sequence.OnStart(() =>
        {
            isCooltime = true;

            CooltimeImage.fillAmount = 1f;
            CooltimeImage.gameObject.SetActive(true);
        });
        sequence.Append(DOTween.To(() => CooltimeImage.fillAmount, amount => CooltimeImage.fillAmount = amount, 0, cooltime));
        sequence.OnComplete(() =>
        {
            CooltimeImage.gameObject.SetActive(false);
            isCooltime = false;
        });

        sequence.Play();
    }
}
