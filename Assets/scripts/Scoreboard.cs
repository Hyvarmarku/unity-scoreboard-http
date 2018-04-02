using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoreboard : MonoBehaviour {
    public HttpHandler HttpHandler;
    public GameObject NextButton;
    public GameObject PrevButton;

    private ScoreObjectUI[] _scoreModels;
    private int _currentPage = 0;
    private ScoreObjectHTTP[] _scores;

    // Use this for initialization
    void Start()
    {
        // Get all ScoreModel child objects.
        _scoreModels = GetComponentsInChildren<ScoreObjectUI>();

        // Set all scores inactive as default
        foreach (ScoreObjectUI scoreModel in _scoreModels)
        {
            scoreModel.gameObject.SetActive(false);
        }

        // Get and set scores from HttpHandler
        HttpHandler.GetAndSetScores();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            HttpHandler.SaveNewScore("TestName11", 1000);
        }
    }

    // Called by HttpHandler. Sets and updates scores after server responses
    public void SetScores(ScoreObjectHTTP[] scores)
    {
        this._scores = scores;
        UpdateScores();
    }

    // Go to next page
    public void NextScorePage()
    {
        this._currentPage++;
        UpdateScores();
    }

    // Go to previous page
    public void PreviousScorePage()
    {
        this._currentPage--;
        UpdateScores();
    }

    // Update scores to UI. Display scores based on the current page.
    private void UpdateScores()
    {
        int index = 0 + (10 * _currentPage);
        foreach (ScoreObjectUI scoreModel in _scoreModels)
        {
            if (index < _scores.Length)
            {
                ScoreObjectHTTP score = _scores[index];
                scoreModel.Placement.text = (index + 1).ToString();
                scoreModel.Name.text = score.name;
                scoreModel.Score.text = score.score.ToString();
                scoreModel.gameObject.SetActive(true);
            }
            else
            {
                scoreModel.gameObject.SetActive(false);
            }
            index++;
        }
        UpdateButtonsState();
    }

    // Update state of button.
    private void UpdateButtonsState()
    {
        // Display next button if there is more content on the next page.
        if (10 * (_currentPage + 1) > _scores.Length)
        {
            this.NextButton.SetActive(false);
        } else
        {
            this.NextButton.SetActive(true);
        }

        // Display prev button if current page is not 0.
        if (_currentPage == 0)
        {
            this.PrevButton.SetActive(false);
        }
        else
        {
            this.PrevButton.SetActive(true);
        }
    }
}
