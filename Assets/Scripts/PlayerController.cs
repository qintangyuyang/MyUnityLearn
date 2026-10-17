using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
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

    public Rigidbody2D theRB;
    public float moveSpeed;

    //Unity中的输入系统 需要引用命名空间UnityEngine.InputSystem
    public InputActionReference moveInput;

    public InputActionReference actionInput;

    public Animator anim;

    public enum ToolType
    {
        /// <summary>
        /// 耕地工具
        /// </summary>
        plough,
        /// <summary>
        /// 浇水壶
        /// </summary>
        wateringcan,
        /// <summary>
        /// 种子
        /// </summary>
        seeds,
        /// <summary>
        /// 篮子
        /// </summary>
        basket
    }

    public ToolType currentTool;

    public float toolWaitTime = 0.5f;
    private float toolWaitCounter;

    public Transform toolIndicator;
    public float toolRange = 3f;

    public CropController.CropType seedCropType;

    void Start()
    {
        UIController.instance.SwitchTool((int)currentTool);

        UIController.instance.SwitchSeed(seedCropType);
    }

    void Update()
    {
        if (UIController.instance != null)
        {
            if (UIController.instance.theIC != null || UIController.instance.theShop != null)
            {
                if (UIController.instance.theIC.gameObject.activeSelf || UIController.instance.theShop.gameObject.activeSelf)
                {
                    theRB.linearVelocity = Vector2.zero;
                    return;
                }
            }
        }
        if (UIController.instance.pauseScreen != null)
        {
            if (UIController.instance.pauseScreen.gameObject.activeSelf)
            {
                theRB.linearVelocity = Vector2.zero;
                return;
            }
        }

        if (toolWaitCounter > 0)
        {
            toolWaitCounter -= Time.deltaTime;
            theRB.linearVelocity = Vector2.zero;
        }
        else
        {
            //用刚体上的API，取到物体上的线性速度
            //theRB.linearVelocity = new Vector2(moveSpeed, 0f);
            theRB.linearVelocity = moveInput.action.ReadValue<Vector2>().normalized * moveSpeed;

            //玩家左右翻转
            if (theRB.linearVelocity.x < 0)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else if (theRB.linearVelocity.x > 0)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }

        bool hasSwitchedTool = false;
        //工具使用
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            currentTool++;
            if ((int)currentTool >= 4)
            {
                currentTool = ToolType.plough;
            }

            hasSwitchedTool = true;
        }
        //工具切换
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            currentTool = ToolType.plough;

            hasSwitchedTool = true;
        }
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            currentTool = ToolType.wateringcan;

            hasSwitchedTool = true;
        }
        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            currentTool = ToolType.seeds;

            hasSwitchedTool = true;
        }
        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            currentTool = ToolType.basket;

            hasSwitchedTool = true;
        }

        if (hasSwitchedTool)
        {
            UIController.instance.SwitchTool((int)currentTool);
        }

        //动画控制器的参数设置（状态切换）
        anim.SetFloat("Speed", theRB.linearVelocity.magnitude);

        if (GridController.instance != null)
        {
            //工具使用
            if (actionInput.action.WasPressedThisFrame())
            {
                UseTool();
            }

            //限制鼠标上工具指示的位置，离玩家距离不超过toolRange
            toolIndicator.position = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            toolIndicator.position = new Vector3(toolIndicator.position.x, toolIndicator.position.y, 0f);

            if (Vector3.Distance(toolIndicator.position, transform.position) > toolRange)
            {
                Vector2 direction = toolIndicator.position - transform.position;
                direction = direction.normalized * toolRange;
                toolIndicator.position = transform.position + new Vector3(direction.x, direction.y, 0f);
            }

            toolIndicator.position = new Vector3(Mathf.FloorToInt(toolIndicator.position.x) + 0.5f, Mathf.FloorToInt(toolIndicator.position.y) + 0.5f, 0f);
        }
        else
        {
            toolIndicator.position = new Vector3(0f, 0f, -20f);
        }

    }

    void UseTool()
    {
        GrowBlock block = null;
        //block = FindFirstObjectByType<GrowBlock>();
        //block.PloughSoil();
        block = GridController.instance.GetBlock(toolIndicator.position.x - 0.5f, toolIndicator.position.y - 0.5f);

        //工具使用间隔
        toolWaitCounter = toolWaitTime;

        if (block != null)
        {
            //根据当前工具类型，调用不同的功能
            switch (currentTool)
            {
                case ToolType.plough:
                    block.PloughSoil();
                    anim.SetTrigger("usePlough");
                    break;
                case ToolType.wateringcan:
                    block.WaterSoil();
                    anim.SetTrigger("useWateringCan");
                    break;
                case ToolType.seeds:
                    if (CropController.instance.GetCropInfo(seedCropType).seedAmount > 0)
                    {
                        block.PlantCrop(seedCropType);
                    }

                    break;
                case ToolType.basket:
                    block.HarvestCrop();
                    break;
            }
        }
    }
    public void SwitchSeed(CropController.CropType newSeed)
    {
        seedCropType = newSeed;
    }
}
