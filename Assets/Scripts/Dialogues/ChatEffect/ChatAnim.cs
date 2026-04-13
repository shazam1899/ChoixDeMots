using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;

[RequireComponent(typeof(CanvasGroup))]
public class UIManager : MonoBehaviour
{
    public float duration = 0.4f;
    public float offsetX = 100f;
    public float offsetY = -30f;
    public RectTransform Rect;
    public CanvasGroup Bubble;

    private void Awake()
    {
        Rect = GetComponent<RectTransform>();
        Bubble = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        PlayAnimation();
    }

    public void PlayAnimation()
    {
        Rect.DOKill();
        Bubble.DOKill();

        Bubble.alpha = 0f;
        Vector2 endPos = Rect.anchoredPosition;
        float direction = MathF.Sign(endPos.x);

        if(direction == 0)
        {
            direction = 1;
        }

        Vector2 startPos = endPos + new Vector2(offsetX * direction, offsetY);
        Rect.anchoredPosition = startPos;
        Sequence seq = DOTween.Sequence();
        seq.Append(Bubble.DOFade(1f, duration));
        seq.Join(Rect.DOAnchorPos(endPos, duration).SetEase(Ease.OutCubic));
    }
}