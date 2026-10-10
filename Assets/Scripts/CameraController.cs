using UnityEngine;

public class CameraController : MonoBehaviour
{
    //摄像机寻找的目标物体（玩家）
    private Transform target;

    //摄像机移动范围限制的最小点和最大点
    public Transform clampMin;
    public Transform clampMax;

    private Camera cam;
    private float halfHeight;
    private float halfWidth;
    void Start()
    {
        //查找场景中任意一个PlayerController组件，并获取其transform组件（目前游戏中只有一个玩家，所以可以直接使用FindAnyObjectByType）
        target = FindAnyObjectByType<PlayerController>().transform;

        clampMin.SetParent(null);
        clampMax.SetParent(null);

        cam = GetComponent<Camera>();
        halfHeight = cam.orthographicSize;
        halfWidth = cam.orthographicSize * cam.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        //相机跟随玩家移动
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);

        //限制相机移动范围
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, clampMin.position.x + halfWidth, clampMax.position.x - halfWidth);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, clampMin.position.y + halfHeight, clampMax.position.y - halfHeight);
        transform.position = clampedPosition;
    }
}
