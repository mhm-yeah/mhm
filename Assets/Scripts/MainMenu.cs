using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
    public void Settings()
    {
        SceneManager.LoadScene("Settings");
        Debug.Log("Settings");
    }
    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
        Debug.Log("GoToMainMenu");
    }
}