using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D theRB;
    public float moveSpeed;

    //Unity中的输入系统 需要引用命名空间UnityEngine.InputSystem
    public InputActionReference moveInput;

    public Animator anim;

    void Start()
    {
        
    }

    void Update()
    {
        //用刚体上的API，取到物体上的线性速度
        //theRB.linearVelocity = new Vector2(moveSpeed, 0f);
        theRB.linearVelocity = moveInput.action.ReadValue<Vector2>().normalized * moveSpeed;


        //玩家左右翻转
        if(theRB.linearVelocity.x < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if(theRB.linearVelocity.x > 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        //动画控制器的参数设置（状态切换）
        anim.SetFloat("Speed", theRB.linearVelocity.magnitude);
    }
}
