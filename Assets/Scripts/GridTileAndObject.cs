using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTileAndObject
{
    private GameObject _objectOnTile;
    private GameObject _tileMap;

    public GameObject ObjectOnTile
    {
        get { return _objectOnTile; }
    }

    public GameObject TileMap
    {
        get { return _tileMap; }
    }
}
