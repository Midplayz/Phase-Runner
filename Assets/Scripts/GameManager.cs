using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TextMeshProUGUI scoreText;
    private int score = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); 
            return;
        }
    }


    private void Start()
    {
        score = 0;
        scoreText.text = "Score: " + score.ToString();
    }

    public void ChangeScore()
    {
        score = score + 1;
        scoreText.text = "Score: " + score.ToString();
    }

    public void RestartGame()
    {
        score = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        scoreText.text = "Score: " + score.ToString();
    }
}
