using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    TextMeshProUGUI Text;
    Transform targetEnemy;

    Vector3 targetPos;

    Vector3 offset = new Vector3(0, 75f, 0);

    public void Initialize(Transform target, bool isCritical = false)
    {
        if (Text == null) Text = GetComponent<TextMeshProUGUI>();

        targetEnemy = target;
        targetPos = Camera.main.WorldToScreenPoint(targetEnemy.position) + offset;

        var targetScale = 1.1f;
        if (!isCritical)
        {
            Text.color = new Color(255, 255, 255, 1);
            targetScale = 1.1f;
        } else
        {
            Text.color = new Color(255, 0, 0, 1);
            targetScale = 1.25f;
        }
        transform.localScale = new Vector3(1, 1, 1);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(Text.DOFade(0, 0.5f));
        sequence.Join(transform.DOScale(targetScale, 0.125f));
        sequence.Insert(0.125f, transform.DOScale(1f, 0.125f));

        sequence.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (targetEnemy != null)
        {
            transform.position = (targetPos);
        }
    }
}
