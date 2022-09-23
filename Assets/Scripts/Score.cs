using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score
{
    private int reward;

    public int Reward
    {
        get { return reward; }
        set { reward = value; }
    }

    public Score(int _reward)
    {
        this.reward = _reward;
    }
}
