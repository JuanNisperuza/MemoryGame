using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour
{
    public int row;
    public int column;
    public int id;
    private bool isShowed = false; 
    public SpriteRenderer blockImage;
    public Sprite frontSprite, backSprite;

    private void Awake()
    {
        HideBlock();
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
        GameManager.instance.OnBlockClicked(this);
    }
}
