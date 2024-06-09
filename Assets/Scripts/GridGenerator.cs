using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] private string jsonFileName = "blocks";
    [SerializeField] private Sprite[] blockSprites;
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private Camera gridCamera;
    private List<Block> blocks = new List<Block>();
    private List<Vector2> positions= new List<Vector2>();
    public int totalPairs;

    public void LoadBlocksFromJson()
    {
        string jsonString = LoadJsonFromFile(jsonFileName);
        if (string.IsNullOrEmpty(jsonString)) return;

        BlockDataList blockDataList = ParseJson(jsonString);
        if (blockDataList == null) return;

        (int gridX, int gridY) = GetGridDimensions(blockDataList);
        if (!ValidateGridSize(gridX, gridY)) return;

        CreateBlocks(blockDataList);
    }

    private string LoadJsonFromFile(string fileName)
    {
        TextAsset jsonText = Resources.Load<TextAsset>(fileName);
        if (jsonText == null)
        {
            Debug.LogError("No se pudo cargar el archivo JSON desde Resources.");
            return null;
        }
        return jsonText.text;
    }

    private BlockDataList ParseJson(string jsonString)
    {
        return JsonUtility.FromJson<BlockDataList>(jsonString);
    }

    private (int, int) GetGridDimensions(BlockDataList blockDataList)
    {
        int gridX = 0;
        int gridY = 0;
        foreach (BlockData data in blockDataList.blocks)
        {
            if (data.C > gridX) gridX = data.C;
            if (data.R > gridY) gridY = data.R;
        }
        totalPairs = (gridX * gridY) / 2;
        return (gridX, gridY);
    }

    private bool ValidateGridSize(int gridX, int gridY)
    {
        if (gridX < 2 || gridX > 8 || gridY < 2 || gridY > 8)
        {
            Debug.LogError("La cuadrícula debe tener un tamaño entre 2x2 y 8x8");
            return false;
        }
        return true;
    }

    private void CreateBlocks(BlockDataList blockDataList)
    {
        foreach (BlockData data in blockDataList.blocks)
        {
            Vector2 position = new Vector2(data.C * 1.1f, -data.R * 1.1f);
            GameObject blockObject = Instantiate(blockPrefab, position, Quaternion.identity, transform);
            positions.Add(position);
            Block block = blockObject.GetComponent<Block>();
            if (data.number < 0 || data.number > 9)
            {
                Debug.LogError("El valor (number) tiene que estar entre 0 - 9");
                return;
            }
            block.row = data.R;
            block.column = data.C;
            block.id = data.number;
            block.frontSprite = blockSprites[block.id];
            blocks.Add(block);
        }
        Vector2 middlePoint = GetMiddlePoint();
        gridCamera.transform.position = new Vector3(middlePoint.x, middlePoint.y, gridCamera.transform.position.z);
    }

    public  Vector2 GetMiddlePoint()
    {
        if (positions == null || positions.Count == 0)
        {
            throw new System.ArgumentException("Array of positions cannot be null or empty.");
        }

        Vector2 sum = Vector2.zero;
        foreach (Vector2 pos in positions)
        {
            sum += pos;
        }

        return sum / positions.Count;
    }
}
