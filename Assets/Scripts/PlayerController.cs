using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
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

    void Start()
    {
        
    }

    void Update()
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
        //动画控制器的参数设置（状态切换）
        anim.SetFloat("Speed", theRB.linearVelocity.magnitude);

        //工具使用
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            currentTool++;
            if ((int)currentTool >= 4)
            {
                currentTool = ToolType.plough;
            }
        }
        //工具切换
        if(Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            currentTool = ToolType.plough;
        }
        if(Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            currentTool = ToolType.wateringcan;
        }
        if(Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            currentTool = ToolType.seeds;
        }
        if(Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            currentTool = ToolType.basket;
        }

        //工具使用
        if (actionInput.action.WasPressedThisFrame())
        {
            UseTool();
        }
    }
    
    void UseTool()
    {
        GrowBlock block = null;
        block = FindFirstObjectByType<GrowBlock>();
        //block.PloughSoil();

        if (block != null)
        {
            //根据当前工具类型，调用不同的功能
            switch (currentTool)
            {
                case ToolType.plough:
                    block.PloughSoil();
                    break;
                case ToolType.wateringcan:
                    break;
                case ToolType.seeds:
                    break;
                case ToolType.basket:
                    break;
            }
        }
    }
}
