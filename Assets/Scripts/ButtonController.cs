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

    [Range(0, 15f)] [SerializeField] float _cooltime = 0.5f;
    public bool isCooltime = false;

    public Action onClick;

    private void Awake()
    {
        if (type.Equals(ButtonType.Shot))
        {
            SetCooltime(PlayerInfo.Firerate);
        } else if (type.Equals(ButtonType.Dash))
        {
            SetCooltime(PlayerInfo.Firerate + 0.5f);
        }
    }

    private void OnEnable()
    {        
        onClick += Cooltime;
    }

    private void OnDisable()
    {
        onClick -= Cooltime;
    }

    public void SetCooltime(float cooltime)
    {
        _cooltime = cooltime;
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
        sequence.Append(DOTween.To(() => CooltimeImage.fillAmount, amount => CooltimeImage.fillAmount = amount, 0, _cooltime));
        sequence.OnComplete(() =>
        {
            CooltimeImage.gameObject.SetActive(false);
            isCooltime = false;
        });

        sequence.Play();
    }
}
