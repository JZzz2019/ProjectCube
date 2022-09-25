using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreCounter;

    private void Update()
    {
        scoreCounter.text = GameManager.Instance.ScorePoint.ToString();
    }
}
