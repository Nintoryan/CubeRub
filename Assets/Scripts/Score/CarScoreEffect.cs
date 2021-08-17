using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class CarScoreEffect : MonoBehaviour
{
    [SerializeField] private List<TMP_Text> _texts = new List<TMP_Text>();

    private Transform camTransform;
    private void OnEnable()
    {
        camTransform = Camera.main.transform;
        foreach (var _text in _texts)
        {
            var s = DOTween.Sequence();
            var rt = _text.GetComponent<RectTransform>();
            var RandomDelay = Random.Range(0, 0.5f);
            s.AppendInterval(RandomDelay);
            s.Append(_text.DOFade(_texts.IndexOf(_text) == 0 ? 1:0.5f, 0.1f));
            s.Join(rt.DOAnchorPosY(rt.anchoredPosition.y+0.2f, 0.25f).SetEase(Ease.OutBack));
            s.AppendInterval(1f);
            s.Append(_text.DOFade(0, 0.75f));
            s.Join(rt.DOAnchorPosY(rt.anchoredPosition.y+1,0.75f));
            s.AppendCallback(()=>Destroy(gameObject));
        }
        
    }

    private void FixedUpdate()
    {
        transform.LookAt(camTransform);
    }
}
