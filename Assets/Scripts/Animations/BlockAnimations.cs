using DG.Tweening;
using UnityEngine;

public class BlockAnimations : MonoBehaviour
{
    private Tween idleTween;
    public Block block;
    [SerializeField] private AudioClip hoverSound, clickSound;

    void Start()
    {
        StartIdleAnimation();
    }

    void OnMouseEnter()
    {
        if (!block.IsMatched)
        {
            idleTween.Kill();
            HoverAnimation();
        }

    }

    void OnMouseExit()
    {
        if (!block.IsMatched)
        {
            ResetAnimation();
            StartIdleAnimation();
        }
    }
    void StartIdleAnimation()
    {
        idleTween = transform.DOScale(1f, 3f).SetLoops(-1, LoopType.Yoyo);
    }

    void HoverAnimation()
    {
        transform.DOScale(1.1f, 0.2f);
        SoundManager.Instance.PlaySound(hoverSound);
    }

    void ResetAnimation()
    {
        transform.DOScale(1f, 0.2f).OnComplete(() =>
        {
            StartIdleAnimation();
        });
    }

    public void ClickAnimation()
    {
        SoundManager.Instance.PlaySound(clickSound);
        Sequence clickSequence = DOTween.Sequence();
        clickSequence.Append(transform.DORotate(new Vector3(0, 360, 0), 0.5f, RotateMode.LocalAxisAdd));
        clickSequence.Append(transform.DOScale(1.1f, 0.2f).SetLoops(2, LoopType.Yoyo));
    }
}
