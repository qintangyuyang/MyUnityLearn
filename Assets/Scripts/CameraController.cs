using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform target;
    void Start()
    {
        //查找场景中任意一个PlayerController组件，并获取其transform组件（目前游戏中只有一个玩家，所以可以直接使用FindAnyObjectByType）
        target = FindAnyObjectByType<PlayerController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        //相机跟随玩家移动
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
    }
}
