using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMap : MonoBehaviour
{
    private Transform parent;
    private GameObject mapTile;

    private List<GameObject> mapTileList = new List<GameObject>();

    public PlayMap(Transform _parent, GameObject _mapTile)
    {
        this.parent = _parent;
        this.mapTile = _mapTile;
    }
    public void GenerateGrid(int gridSize, float xAxis, float zAxis)
    {
        if (parent.childCount > 0)
        {
            Debug.LogWarning("Grid's been generated");
        }
        else
        {
            GenerationImplementation(gridSize, xAxis, zAxis);
        }
    }
    private void GenerationImplementation(int _gridSize, float _xAxis, float _zAxis)
    {
        for (int i = 0; i < _gridSize*_gridSize; i++)
        {
            GameObject obj = Instantiate(mapTile, new Vector3(_xAxis + (_xAxis * (i % _gridSize)), 0, _zAxis + (_zAxis * (i / _gridSize))), Quaternion.identity, parent);
            mapTileList.Add(obj);
        }
    }

    public void DestroyGrid()
    {
        foreach(GameObject child in mapTileList)
        {
            Destroy(child);
        }
        mapTileList.Clear();
    }

    //Shuffle the list randomly to be used for random generation of all entities
    public List<GameObject> ShuffleList()
    {
        for (int i = 0; i < mapTileList.Count; i++)
        {
            int randomIndex = Random.Range(i, mapTileList.Count);
            GameObject tempObject = mapTileList[randomIndex];
            mapTileList[randomIndex] = mapTileList[i];
            mapTileList[i] = tempObject;
        }
        return mapTileList;
    }


}
