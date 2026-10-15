using UnityEngine;

public class TimeController : MonoBehaviour
{
    public static TimeController instance;
    void Awake()
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
    public float currentTime;
    public float dayStart, dayEnd;
    public float timeSpeed = 0.25f;
    
    void Start()
    {
        currentTime = dayStart;
    }

    void Update()
    {
        currentTime += Time.deltaTime * timeSpeed;
        if (currentTime > dayEnd)
        {
            currentTime = dayEnd;
        }

        if (UIController.instance != null)
        {
            UIController.instance.UpdateTimeText(currentTime);
        }
    }
}
