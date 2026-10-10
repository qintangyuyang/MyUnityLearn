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
    public Sprite soilTilled,soilWatered;

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
}
