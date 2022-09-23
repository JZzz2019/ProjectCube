using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private int gridSize = 8;

    private float xAxis = 1f;
    private float zAxis = 1f;

    [SerializeField] private Transform playZone;

    [SerializeField] private GameObject mapTile;

    private PlayMap playMap;

    private List<GameObject> shuffledList;
    [SerializeField] private List<GameObject> shuffledList_occupied;

    [SerializeField] private List<GameObject> objectsOnScene;

    [Header("Player prefab, collectibles and obstacles")]
    #region Player prefab and Collectibles and Obstacles
    [SerializeField] private GameObject player;

    [SerializeField] private GameObject[] collectibles;
    [SerializeField] private GameObject obstacle;
    #endregion

    private bool playerExist;

    [SerializeField] private CameraFollow cameraFollow;

    public GameObject Player;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        playMap = new PlayMap(playZone, mapTile);
        shuffledList_occupied = new List<GameObject>();
        objectsOnScene = new List<GameObject>();
    }

    private void Start()
    {
        SettingUpEnvironment();
        SettingUpPlayer();
    }

    private void SettingUpEnvironment()
    {
        playMap.GenerateGrid(gridSize, xAxis, zAxis);
        shuffledList = new List<GameObject>(playMap.ShuffleList());
    }

    private void SettingUpPlayer()
    {
        if (playerExist != true)
        {
            playerExist = true;
            Vector3 spawnPosition = GetUnoccupiedPosition();
            GameObject playerInstance = Instantiate(player, new Vector3(spawnPosition.x, 1f, spawnPosition.z), Quaternion.identity);
            Player = playerInstance;
            cameraFollow.Target = playerInstance.transform;
        }
    }

    private void SpawnCollectible()
    {
        GameObject _collectible = Instantiate(collectibles[Random.Range(0, collectibles.Length)], GetUnoccupiedPosition(), Quaternion.identity);
        objectsOnScene.Add(_collectible);
    }

    private void SpawnObstacle()
    {
        GameObject _obstacle = Instantiate(obstacle, GetUnoccupiedPosition(), Quaternion.identity);
        objectsOnScene.Add(_obstacle);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            SpawnCollectible();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            SpawnObstacle();
        }
        
    }

    public void RemoveFromOccupiedList(GameObject removedObject)
    {
        for (int i = 0; i < shuffledList_occupied.Count; i++)
        {
            if (removedObject.transform.position.x == shuffledList_occupied[i].transform.position.x
                && removedObject.transform.position.z == shuffledList_occupied[i].transform.position.z)
            {
                shuffledList_occupied.RemoveAt(i);
                //Get rid of missing in list
            }
        }
    }

    private void AddToOccupiedList(GameObject _obj)
    {
        shuffledList_occupied.Add(_obj);
    }

    private Vector3 GetUnoccupiedPosition()
    {
        foreach (GameObject obj in shuffledList)
        {
            //If not occupied
            if (!shuffledList_occupied.Contains(obj))
            {
                AddToOccupiedList(obj);
                return new Vector3(obj.transform.position.x, 1f, obj.transform.position.z);
            }
        }
        return Vector3.zero;
    }

    
}
