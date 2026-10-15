using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridInfo : MonoBehaviour
{
    public static GridInfo instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool hasGrid;
    public List<InfoRow> theGrid;

    public void CreateGrid()
    {
        hasGrid = true;

        for (int y = 0; y < GridController.instance.blockRows.Count; y++)
        {
            theGrid.Add(new InfoRow());
            for (int x = 0; x < GridController.instance.blockRows[y].blocks.Count; x++)
            {
                theGrid[y].blocks.Add(new BlockInfo());
            }
        }
    }

    public void UpdateInfo(GrowBlock theBlock, int xPos, int yPos)
    {
        theGrid[yPos].blocks[xPos].currentStage = theBlock.currentState;
        theGrid[yPos].blocks[xPos].isWatered = theBlock.isWatered;
    }

    /// <summary>
    /// 推进作物生长阶段
    /// </summary>
    public void GrowCrop()
    {
        for (int y = 0; y < theGrid.Count; y++)
        {
            for (int x = 0; x < theGrid[y].blocks.Count; x++)
            {
                if (theGrid[y].blocks[x].isWatered == true)
                {
                    switch (theGrid[y].blocks[x].currentStage)
                    {
                        case GrowBlock.GrowthState.planted:
                            theGrid[y].blocks[x].currentStage = GrowBlock.GrowthState.growing1;
                            break;
                        case GrowBlock.GrowthState.growing1:
                            theGrid[y].blocks[x].currentStage = GrowBlock.GrowthState.growing2;
                            break;
                        case GrowBlock.GrowthState.growing2:
                            theGrid[y].blocks[x].currentStage = GrowBlock.GrowthState.ripe;
                            break;
                    }
                    theGrid[y].blocks[x].isWatered = false;
                }
            }
        }
    }

    

}
[System.Serializable]
public class BlockInfo
{
    public bool isWatered;
    public GrowBlock.GrowthState currentStage;
}
[System.Serializable]
public class InfoRow
{
    public List<BlockInfo> blocks = new List<BlockInfo>();
}
