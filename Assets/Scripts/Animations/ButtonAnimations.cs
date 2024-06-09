using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonAnimations : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button button;
    private Tween idleTween;

    void Start()
    {
        StartIdleAnimation();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        idleTween.Kill();
        HoverAnimation();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ResetAnimation();
        StartIdleAnimation();
    }

    void StartIdleAnimation()
    {
        idleTween = button.transform.DOScale(1.05f, 1f).SetLoops(-1, LoopType.Yoyo);
    }

    void HoverAnimation()
    {
        button.transform.DOScale(1.2f, 0.2f);
    }

    void ResetAnimation()
    {
        button.transform.DOScale(1f, 0.2f);
    }
}
