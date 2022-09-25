using System.IO;
using UnityEngine;
using TMPro;

public class SavingSystem
{
    private int previousHighestScore;
    private int previousNumOfPushedItems;
    private int previousNumOfAttempt;
    public SavingSystem()
    {
        if (File.Exists(Application.dataPath + "/SavedData.json"))
        {
            loadDataForComparison();
            Debug.Log("Existing!!!!!!!!!!!!!!!");
        }
    }
    public void SaveData(int _score, int _numOfPushedItems)
    {
        SavedData data = new SavedData();
        if (_score > previousHighestScore)
        {
            data.HighestScoreInRecord = _score;
        }
        else
        {
            data.HighestScoreInRecord = previousHighestScore;
        }
        data.NumberOfAttempts = previousNumOfAttempt + 1;
        data.NumberOfPushedItems = previousNumOfPushedItems + _numOfPushedItems;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.dataPath + "/SavedData.json", json);
    }

    public void LoadDataToDisplay(TextMeshProUGUI highestScoreText, TextMeshProUGUI numOfAttemptsText, TextMeshProUGUI numOfPushedItemsText)
    {
        string json = File.ReadAllText(Application.dataPath + "/SavedData.json");
        SavedData data = JsonUtility.FromJson<SavedData>(json);

        highestScoreText.text = data.HighestScoreInRecord.ToString();
        numOfAttemptsText.text = data.NumberOfAttempts.ToString();
        numOfPushedItemsText.text = data.NumberOfPushedItems.ToString();

    }

    //compare record in history to find the highest value
    private void loadDataForComparison()
    {
        string json = File.ReadAllText(Application.dataPath + "/SavedData.json");
        SavedData data = JsonUtility.FromJson<SavedData>(json);

        previousHighestScore = data.HighestScoreInRecord;
        previousNumOfPushedItems = data.NumberOfPushedItems;
        previousNumOfAttempt = data.NumberOfAttempts;
    }
}
