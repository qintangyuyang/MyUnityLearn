using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject[] toolbarActivatorIcons;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    /// <summary>
    /// 切换工具栏图标
    /// </summary>
    public void SwitchTool(int selected)
    {
        foreach (var icon in toolbarActivatorIcons)
        {
            icon.SetActive(false);
        }

        toolbarActivatorIcons[selected].SetActive(true);
    }
}
