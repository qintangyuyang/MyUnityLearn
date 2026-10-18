using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DayEndController : MonoBehaviour
{
    public TMP_Text dayText;
    public string wakeUpScene;
    void Start()
    {
        if (TimeController.instance != null)
        {
            dayText.text = "- Day" + TimeController.instance.currentDay + " -";
        }

        AudioManager.instance.PauseMusic();

        AudioManager.instance.PlaySFX(1);
    }

    void Update()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)
        {
            TimeController.instance.StartDay();

            AudioManager.instance.ResumeMusic();

            SceneManager.LoadScene(wakeUpScene);
        }
    }
}
