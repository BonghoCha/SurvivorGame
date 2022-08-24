using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    TextMeshProUGUI Text;
    Transform targetEnemy;

    Vector3 offset = new Vector3(0, 100f, 0);

    public void Initialize(Transform target)
    {
        if (Text == null) Text = GetComponent<TextMeshProUGUI>();

        targetEnemy = target;

        Text.color = new Color(255, 255, 255, 1);
        transform.localScale = new Vector3(1, 1, 1);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(Text.DOFade(0, 0.5f));
        sequence.Join(transform.DOScale(1.1f, 0.125f));
        sequence.Insert(0.125f, transform.DOScale(1f, 0.125f));

        sequence.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (targetEnemy != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(targetEnemy.position);
            transform.position += offset;
        }
    }
}
