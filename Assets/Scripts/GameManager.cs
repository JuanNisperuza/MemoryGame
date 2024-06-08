using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<Block> blocks = new List<Block>();
    public Sprite[] sprites;
    private Block firstRevealedBlock;
    private Block secondRevealedBlock;
    private bool canClick = true;
    private string file = "blocks";
    public GameObject blockPrefab;
    public Transform grid;
    public int maxRow = 0;
    public int maxColumn = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }

    private void Start()
    {
        LoadBlocksFromJson(file);
    }

    private void LoadBlocksFromJson(string fileName)
    {
        TextAsset jsonText = Resources.Load<TextAsset>(fileName);
        if (jsonText == null)
        {
            Debug.LogError("No se pudo cargar el archivo JSON desde Resources.");
            return;
        }

        string jsonString = jsonText.text;
        BlockDataList blockDataList = JsonUtility.FromJson<BlockDataList>(jsonString);

        foreach (BlockData data in blockDataList.blocks)
        {
            Vector2 position = new Vector2(data.C * 1.1f, -data.R * 1.1f);
            GameObject blockObject = Instantiate(blockPrefab, position, Quaternion.identity, grid);
            Block block = blockObject.GetComponent<Block>();
            block.row = data.R;
            block.column = data.C;
            block.id = data.number;
            block.frontSprite = sprites[block.id];
            blocks.Add(block);
        }
    }

    public void OnBlockClicked(Block clickedBlock)
    {
        if (!canClick) return;

        if (firstRevealedBlock == null)
        {
            firstRevealedBlock = clickedBlock;
            firstRevealedBlock.ShowBlock();
        }
        else if (secondRevealedBlock == null && clickedBlock != firstRevealedBlock)
        {
            secondRevealedBlock = clickedBlock;
            secondRevealedBlock.ShowBlock();
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        canClick = false;
        yield return new WaitForSeconds(1.0f);

        if (firstRevealedBlock.id == secondRevealedBlock.id)
        {
            Debug.Log("Son iguales!");
        }
        else
        {
            firstRevealedBlock.HideBlock();
            secondRevealedBlock.HideBlock();
        }
        firstRevealedBlock = null;
        secondRevealedBlock = null;
        canClick = true;
    }
}
