using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timer;

    private void Update()
    {
        timer.text = GameManager.Instance.CountDown.ToString("0");
    }
}
