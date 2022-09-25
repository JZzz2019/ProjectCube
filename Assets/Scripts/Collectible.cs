using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible
{
    private int reward;
    public int Reward
    {
        get { return reward; }
        set { reward = value; }
    }

    private string collectibleName;
    public string CollectibleName
    {
        get { return collectibleName; }
        set { collectibleName = value; }
    }

    private Color colour;

    public Color Colour
    {
        get { return colour; }
        set { colour = value; }
    }
    public Collectible(int _reward, string _collectibleName, Color _colour)
    {
        this.reward = _reward;
        this.collectibleName = _collectibleName;
        this.colour = _colour;
    }
}
