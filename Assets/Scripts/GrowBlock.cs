using UnityEngine;
using UnityEngine.InputSystem;

public class GrowBlock : MonoBehaviour
{
    public enum GrowthState
    {
        /// <summary>
        /// 荒地
        /// </summary>
        barren,
        /// <summary>
        /// 翻耕
        /// </summary>
        ploughed,
        /// <summary>
        /// 播种
        /// </summary>
        planted,
        /// <summary>
        /// 生长1
        /// </summary>
        growing1,
        /// <summary>
        /// 生长2
        /// </summary>
        growing2,
        /// <summary>
        /// 成熟
        /// </summary>
        ripe
    }

    //当前生长状态
    public GrowthState currentState;

    public SpriteRenderer thSR;
    public Sprite soilTilled, soilWatered;

    public SpriteRenderer cropSR;
    public Sprite cropPlanted, cropGrowing1, cropGrowing2, cropRipe;

    public bool isWatered;

    void Start()
    {

    }


    void Update()
    {
        // if(Keyboard.current.eKey.wasPressedThisFrame)
        // {
        //     AdvanceStage();
        //     SetSoilSprite();
        // }

        if(Keyboard.current.nKey.wasPressedThisFrame)
        {
            AdvanceCrop();
        }
    }

    /// <summary>
    /// 推进生长阶段
    /// </summary>
    public void AdvanceStage()
    {
        currentState = currentState + 1;
        if ((int)currentState >= 6)
        {
            currentState = GrowthState.barren;
        }
    }

    /// <summary>
    /// 设置为翻耕状态
    /// </summary>
    public void SetSoilSprite()
    {
        //荒地不显示图片
        if (currentState == GrowthState.barren)
        {
            thSR.sprite = null;
        }
        else
        {
            if (isWatered)
            {
                thSR.sprite = soilWatered;
            }
            else
            {
                thSR.sprite = soilTilled;
            }
        }
    }

    /// <summary>
    /// 翻耕土地
    /// </summary>
    public void PloughSoil()
    {
        if (currentState == GrowthState.barren)
        {
            currentState = GrowthState.ploughed;
            SetSoilSprite();
        }
    }
    /// <summary>
    /// 浇水
    /// </summary>
    public void WaterSoil()
    {
        isWatered = true;

        SetSoilSprite();
    }

    /// <summary>
    /// 播种
    /// </summary>
    public void PlantCrop()
    {
        if (currentState == GrowthState.ploughed && isWatered == true)
        {
            currentState = GrowthState.planted;
            UpdateCropSprite();
        }
    }

    /// <summary>
    /// 更新作物的精灵图片
    /// </summary>
    void UpdateCropSprite()
    {
        switch (currentState)
        {
            case GrowthState.planted:
                cropSR.sprite = cropPlanted;
                break;
            case GrowthState.growing1:
                cropSR.sprite = cropGrowing1;
                break;
            case GrowthState.growing2:
                cropSR.sprite = cropGrowing2;
                break;
            case GrowthState.ripe:
                cropSR.sprite = cropRipe;
                break;
        }
    }

    /// <summary>
    /// 推进作物生长阶段
    /// </summary>
    public void AdvanceCrop()
    {
        if (isWatered)
        {
            if (currentState == GrowthState.planted || currentState == GrowthState.growing1 || currentState == GrowthState.growing2)
            {
                //AdvanceStage();
                currentState++;
                isWatered = false;
                SetSoilSprite();
                UpdateCropSprite();
            }
        }
    }

    /// <summary>
    /// 收获作物
    /// </summary>
    public void HarvestCrop()
    {
        if (currentState == GrowthState.ripe)
        {
            currentState = GrowthState.ploughed;
            SetSoilSprite();
            cropSR.sprite = null;
        }
    }
}
