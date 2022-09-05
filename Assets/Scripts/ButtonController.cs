using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    enum ButtonType
    {
        Shot,
        Dash
    }
    [SerializeField] ButtonType type;

    [SerializeField] Image CooltimeImage;

    [Range(0, 15f)] [SerializeField] float cooltime = 0.5f;
    bool isCooltime = false;

    private void OnEnable()
    {
        Debug.Log(GameManager.instance);

        if (GameManager.instance != null)
        {
            switch (type)
            {
                case ButtonType.Shot:
                    {
                        GameManager.instance.Player.onShot += Cooltime;
                        break;
                    }
                case ButtonType.Dash:
                    {
                        GameManager.instance.Player.onDash += Cooltime;
                        break;
                    }
            }
        }
    }

    private void OnDisable()
    {
        if (GameManager.instance != null)
        {
            switch (type)
            {
                case ButtonType.Shot:
                    {
                        GameManager.instance.Player.onShot -= Cooltime;
                        break;
                    }
                case ButtonType.Dash:
                    {
                        GameManager.instance.Player.onDash -= Cooltime;
                        break;
                    }
            }
        }
    }

    void Cooltime()
    {
        if (isCooltime) return;
        Debug.Log("½ÇÇà");
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
