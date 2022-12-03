using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    #region ### Actions ####
    public Action onShot;
    public Action onDash;
    #endregion

    float timeLimit = 5f;
    IEnumerator SynchronizedOnActions()
    {
        float time = 0f;
        while (time < timeLimit)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                switch (buttons[i].type)
                {
                    case ButtonController.ButtonType.Shot:
                        {
                            if (onShot == null)
                            {
                                onShot += buttons[i].onClick;
                            }
                            break;
                        }
                    case ButtonController.ButtonType.Dash:
                        {
                            if (onDash == null)
                            {
                                onDash += buttons[i].onClick;
                            }
                            break;
                        }
                }
            }
            if (onShot != null && onDash != null) break;

            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        onShot += Shot;
        onDash += Dash;
    }

    void RemoveActions()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            switch (buttons[i].type)
            {
                case ButtonController.ButtonType.Shot:
                    {
                        onShot -= buttons[i].onClick;
                        break;
                    }
                case ButtonController.ButtonType.Dash:
                    {
                        onDash -= buttons[i].onClick;
                        break;
                    }
            }
        }
        if (onShot != null)
        {
            onShot -= Shot;
        }

        if (onDash != null)
        {
            onDash -= Dash;
        }
    }

    public void OnShot()
    {
        if (onShot != null)
        {
            onShot();
        }
    }

    public void OnDash()
    {
        if (onShot != null)
        {
            onDash();
        }
    }

    void Shot()
    {
        if (!_init) return;
        if (buttons[(int)ButtonController.ButtonType.Shot].isCooltime) return;

        GameObject goMissile = MissileManager.instance.GetMissile(missileType);
        goMissile.transform.SetPositionAndRotation(_shootPosition.position, Quaternion.identity);

        goMissile.GetComponent<Rigidbody>().AddForce(_direction * missileSpeed);
    }

    void Dash()
    {
        if (!_init) return;
        if (buttons[(int)ButtonController.ButtonType.Dash].isCooltime) return;

        Sequence sequence = DOTween.Sequence();
        sequence.OnStart(() =>
        {
            _collider.enabled = false;
        });
        sequence.Append(DOTween.To(() => _speed, _ => _speed = _, 1000f, 0.125f));
        sequence.Append(DOTween.To(() => _speed, _ => _speed = _, _initialSpeed, 0.125f));
        sequence.OnComplete(() =>
        {
            _collider.enabled = true;
        });

        sequence.Play();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnShot();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            OnDash();
        }
    }
#endif
}
