using UnityEngine;
using UnityEngine.SceneManagement;

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
    private bool timeActive;
    public int currentDay = 1;
    public string dayEndScene;
    void Start()
    {
        currentTime = dayStart;
        timeActive = true;
    }

    void Update()
    {
        if (timeActive)
        {
            currentTime += Time.deltaTime * timeSpeed;
            if (currentTime > dayEnd)
            {
                currentTime = dayEnd;
                EndDay();
            }

            if (UIController.instance != null)
            {
                UIController.instance.UpdateTimeText(currentTime);
            }
        }

    }
    public void EndDay()
    {
        timeActive = false;

        currentDay += 1;

        GridInfo.instance.GrowCrop();

        PlayerPrefs.SetString("Transition", "Wake Up");

        //StartDay();
        SceneManager.LoadScene(dayEndScene);
    }
    public void StartDay()
    {
        timeActive = true;
        currentTime = dayStart;
    }
}
