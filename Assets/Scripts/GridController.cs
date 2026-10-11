using System;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public static GridController instance;
    private void Awake()
    {
        instance = this;
    }

    public Transform minPoint, maxPoint;

    public GrowBlock baseGrowBlock;

    private Vector2Int gridSize;

    public List<BlockRow> blockRows = new List<BlockRow>();

    public LayerMask gridBlockers;
    void Start()
    {
        GenerateGrid();
    }


    void Update()
    {

    }

    void GenerateGrid()
    {
        //将minPoint和maxPoint的坐标四舍五入到整数
        minPoint.position = new Vector3(Mathf.Round(minPoint.position.x), Mathf.Round(minPoint.position.y), 0f);
        maxPoint.position = new Vector3(Mathf.Round(maxPoint.position.x), Mathf.Round(maxPoint.position.y), 0f);

        Vector3 startpoint = minPoint.position + new Vector3(0.5f, 0.5f, 0f);

        //Instantiate(baseGrowBlock, startpoint, Quaternion.identity);

        //计算网格的大小
        gridSize = new Vector2Int(Mathf.RoundToInt(maxPoint.position.x - minPoint.position.x), Mathf.RoundToInt(maxPoint.position.y - minPoint.position.y));

        //循环生成网格
        for (int y = 0; y < gridSize.y; y++)
        {
            blockRows.Add(new BlockRow());
            for (int x = 0; x < gridSize.x; x++)
            {
                Vector3 spawnPos = startpoint + new Vector3(x, y, 0f);
                GrowBlock newBlock = Instantiate(baseGrowBlock, spawnPos, Quaternion.identity);
                newBlock.transform.SetParent(transform);
                newBlock.thSR.sprite = null;

                blockRows[y].blocks.Add(newBlock);

                if (Physics2D.OverlapBox(newBlock.transform.position, new Vector2(0.9f, 0.9f), 0f, gridBlockers))
                {
                    newBlock.thSR.sprite = null;
                    newBlock.prevenetUse = true;
                }
            }
        }

        baseGrowBlock.gameObject.SetActive(false);
    }

    /// <summary>
    /// 根据坐标获取格子
    /// </summary>
    public GrowBlock GetBlock(float x, float y)
    {
        x = Mathf.RoundToInt(x);
        y = Mathf.RoundToInt(y);

        x -= Mathf.RoundToInt(minPoint.position.x);
        y -= Mathf.RoundToInt(minPoint.position.y);

        int intX = Mathf.RoundToInt(x);
        int intY = Mathf.RoundToInt(y);

        if (intX < gridSize.x && intY < gridSize.y)
        {
            return blockRows[intY].blocks[intX];
        }
        return null;

    }

}
[Serializable]
public class BlockRow
{
    public List<GrowBlock> blocks = new List<GrowBlock>();
}
