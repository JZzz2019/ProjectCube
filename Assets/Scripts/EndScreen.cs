using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI endingTextDisplayer;
    [SerializeField] private TextMeshProUGUI scoreDisplayer;
    public TextMeshProUGUI highestScoreDisplayer;
    public TextMeshProUGUI numOfAttemptsDisplayer;
    public TextMeshProUGUI numOfPushedItemsDisplayer;

    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void SetUpEndScreen(string _endingText, int _currentScore)
    {
        gameObject.SetActive(true);

        endingTextDisplayer.text = _endingText;
        scoreDisplayer.text = _currentScore.ToString();
        //highestScoreDisplayer.text = _highestScore.ToString();
        //numOfAttemptsDisplayer.text = _numOfAttempts.ToString();
        //numOfPushedItemsDisplayer.text = _numOfPushedItems.ToString();
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
