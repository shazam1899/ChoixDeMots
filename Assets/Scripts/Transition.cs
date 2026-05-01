using UnityEngine;
using DG.Tweening;
using System;
using System.Collections;

public class Transition : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float defaultDuration = 1f;

    private Tween currentTween;
    private bool isPlaying;

    private void Awake()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    private void Kill()
    {
        canvasGroup.DOKill();

        if (currentTween != null && currentTween.IsActive())
            currentTween.Kill();
    }

    private IEnumerator Fade(float target, float duration)
    {
        Debug.Log($"FADE target={target} alpha before={canvasGroup.alpha}");
        Kill();

        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;

        currentTween = canvasGroup.DOFade(target, duration);

        yield return currentTween.WaitForCompletion();

        if (target == 0f)
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }
    }

    public IEnumerator Play(Action action, float duration = -1f)
    {
        if (isPlaying) yield break;

        isPlaying = true;

        float d = duration > 0 ? duration : defaultDuration;

        // fade in noir
        yield return Fade(1f, d);

        // action (teleport, load, etc.)
        action?.Invoke();

        yield return null; // évite glitch visuel Unity

        // fade out
        yield return Fade(0f, d);

        isPlaying = false;
    }
}