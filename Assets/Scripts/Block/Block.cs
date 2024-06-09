using UnityEngine;
using UnityEngine.EventSystems;
public class Block : MonoBehaviour
{
    public int row;
    public int column;
    public int id;
    private bool isShowed = false;
    private bool isMatched = false;
    public SpriteRenderer blockImage, blockBackground;
    public Sprite frontSprite, backSprite;
    public BlockAnimations blockAnimations;
    public bool IsMatched
    {
        get { return isMatched; }
        set
        {
            isMatched = value;
            blockBackground.color = new Color(0.33f, 0.27f, 0.74f);
        }
    }
    private void Awake()
    {
        HideBlock();
    }

    private void Start()
    {
        blockAnimations = GetComponent<BlockAnimations>();
    }

    public void ShowBlock()
    {
        blockImage.sprite = frontSprite;
        isShowed = true;
    }

    public void HideBlock()
    {
        blockImage.sprite = backSprite;
        isShowed = false;
    }

    public bool IsShowed()
    {
        return isShowed;
    }

    private void OnMouseDown()
    {
        if (!isMatched)
            GameManager.Instance.OnBlockClicked(this);
    }
}
