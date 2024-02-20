using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI endingTextDisplayer;
    [SerializeField] private TextMeshProUGUI scoreDisplayer;
    public TextMeshProUGUI HighestScoreDisplayer;
    public TextMeshProUGUI NumOfAttemptsDisplayer;
    public TextMeshProUGUI NumOfPushedItemsDisplayer;

    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void SetUpEndScreen(string _endingText, int _currentScore)
    {
        gameObject.SetActive(true);

        endingTextDisplayer.text = _endingText;
        scoreDisplayer.text = _currentScore.ToString();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
