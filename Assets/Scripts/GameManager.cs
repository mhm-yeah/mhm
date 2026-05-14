using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI;
    public TextMeshProUGUI finalScoreText;
    public AudioManager audioManager;
    public bool isGameOver = false;
    public float totalScore = 0f;

    void Start()
    {
        Time.timeScale = 1f;
        isGameOver = false;
        totalScore = 0f;
    }

    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0;
        finalScoreText.text = "Final Score: " + Mathf.RoundToInt(totalScore).ToString();
        audioManager.StopMusic();
        audioManager.PlaySFX(audioManager.gameOver);
        gameOverUI.SetActive(true);
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Restart");
        Time.timeScale = 1;
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Debug.Log("MainMenu");
    }
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    public void Victory()
    {
        Invoke(nameof(GameOver), 3);
    }
}
