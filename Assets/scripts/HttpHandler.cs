using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HttpHandler : MonoBehaviour {
    public Scoreboard Scoreboard;
    public string url;
    public string Secret = "test secret";

    // Save a new score. Called by Scoreboard.
    public void SaveNewScore(string newName, int newScore)
    {
        StartCoroutine(SaveScore(newName, newScore));
    }

    // Get scores. Called by Scoreboard.
    public void GetAndSetScores()
    {
        StartCoroutine(GetScores());
    }

    // Async operation to save and wait for response.
    private IEnumerator SaveScore(string newName, int newScore)
    {

        WWWForm data = new WWWForm();
        data.AddField("secret", Secret);
        data.AddField("name", newName);
        data.AddField("score", newScore);
        UnityWebRequest www = UnityWebRequest.Post(url + "/scores", data);
        www.SetRequestHeader("Authorization", "tamk_2018");

        yield return www.SendWebRequest();

        // Update scores after new is saved.
        StartCoroutine(GetScores());
    }

    // Async operation to get scores and wait for response.
    private IEnumerator GetScores()
    {
        UnityWebRequest www = UnityWebRequest.Get(url + "/scores" + "/" + Secret);
        www.SetRequestHeader("Authorization", "tamk_2018");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            string result = www.downloadHandler.text;
            string JSONString = "{\"values\":" + result + "}";
            JSONWrapper scores = JsonUtility.FromJson<JSONWrapper>(JSONString);
            Scoreboard.SetScores(scores.values);
            Debug.Log("HERE");
        }
    }

    // Helper class to handle JSON arrays.
    private class JSONWrapper
    {
        public ScoreObjectHTTP[] values;
    }
}
