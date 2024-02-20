using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private int gridSize = 8;

    private float xAxis = 1f;
    private float zAxis = 1f;

    //parent for maps
    [SerializeField] private Transform playZone;

    [SerializeField] private GameObject mapTile;

    private PlayMap playMap;

    private List<GameObject> shuffledList;
    private List<GameObject> shuffledList_occupied;

    private List<GameObject> objectsOnScene;

    [Header("Player prefab, collectibles and obstacles")]
    #region Player prefab and Collectibles and Obstacles
    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private Transform obstaclesCollectibles_parent;

    [SerializeField] private GameObject[] collectibles;
    [SerializeField] private GameObject obstacle;
    #endregion

    private bool playerExist;

    [SerializeField] private CameraFollow cameraFollow;

    [HideInInspector]public GameObject PlayerInstance;

    #region Score and timer
    private int scorePoint;
    public int ScorePoint
    {
        get { return scorePoint; }
        set { scorePoint = value; }
    }
    private float startCountDown = 10f;
    private float countdown;
    public float CountDown
    {
        get { return countdown; }
        set { countdown = value; }
    }
    #endregion

    private string previousTouchedCollectible;
    private string currentTouchedCollectible;
    public string CurrentTouchedCollectible
    {
        get { return currentTouchedCollectible; }
        set { currentTouchedCollectible = value; }
    }

    [SerializeField] private TextMeshProUGUI lastTouchedCollectible;

    [SerializeField] private int comboModifier = 2;

    [SerializeField] private int level;

    public int Level
    {
        get { return level; }
        set { level = value; }
    }

    [SerializeField] private int levelProgressRequirement;
    private int lastCheckPoint = 0;

    private CameraFollow cam;

    private bool isGamePaused = false;

    [SerializeField] private EndScreen endScreen;

    private SavingSystem savingSystem;

    public int NumOfPushedItems
    {
        get { return numOfPushedItems; }
        set { numOfPushedItems = value; }
    }

    private int numOfPushedItems = 0;
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

        cam = Camera.main.GetComponent<CameraFollow>();
        savingSystem = new SavingSystem();
    }

    private void Start()
    {
        level = 1;
        SettingUpEnvironment();
        SettingUpPlayer();
        countdown = startCountDown;
        SpawnCollectible();
    }

    private void Update()
    {
        if (isGamePaused != false)
        {
            return;
        }

        #region Debug Mode
        if (Input.GetKeyDown(KeyCode.J))
        {
            SpawnCollectible();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            SpawnObstacle();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            ResetPlayZone();
        }
        #endregion

        if (hasEndingConditionMet())
        {
            return;
        }

        if (nextLevel())
        {
            return;
        }

        if (countdown <= 0)
        {
            countdown = startCountDown;
            SpawnObstacle();
        }
        else
        {
            countdown -= 1 * Time.deltaTime;
        }
    }

    private bool hasEndingConditionMet()
    {
        //winning condition
        if (scorePoint >= 400)
        {
            string endingText = "YOU" + " " + "WIN!";
            endScreen.SetUpEndScreen(endingText, scorePoint);

            savingSystem.SaveData(scorePoint, numOfPushedItems);
            savingSystem.LoadDataToDisplay(endScreen.HighestScoreDisplayer, endScreen.NumOfAttemptsDisplayer, endScreen.NumOfPushedItemsDisplayer);
            return true;
        }
        //losing condition, -4 because the player size can only overlap 4 tiles
        else if (shuffledList_occupied.Count >= shuffledList.Count-4)
        {
            string endingText = "YOU" + " " + "LOSE!";
            endScreen.SetUpEndScreen(endingText, scorePoint);

            savingSystem.SaveData(scorePoint, numOfPushedItems);
            savingSystem.LoadDataToDisplay(endScreen.HighestScoreDisplayer, endScreen.NumOfAttemptsDisplayer, endScreen.NumOfPushedItemsDisplayer);
            return true;
        }
        //no condition's met
        else
        {
            return false;
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        isGamePaused = true;
    }

    private void UnPauseGame()
    {
        Time.timeScale = 1;
        isGamePaused = false;
    }
    private bool nextLevel()
    {
        if (scorePoint >= lastCheckPoint + levelProgressRequirement)
        {
            lastCheckPoint += levelProgressRequirement;
            level++;
            if (startCountDown > 3f)
            {
                startCountDown -= 2;
            }
            cam.OffSet = new Vector3(0, cam.OffSet.y + 1, cam.OffSet.z - 1);

            ResetPlayZone();
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ResetPlayZone()
    {
        //Clear list
        #region
        shuffledList.Clear();
        shuffledList_occupied.Clear();
        objectsOnScene.Clear();
        playMap.ClearList();
        #endregion
        //Destroy player
        #region
        Destroy(PlayerInstance);
        playerExist = false;
        #endregion
        //Destroy all objects in playzone and obstacles parent
        #region
        foreach (Transform child in playZone)
        {
            Destroy(child.transform.gameObject);
        }
        foreach (Transform child in obstaclesCollectibles_parent)
        {
            Destroy(child.transform.gameObject);
        }
        #endregion

        //Setting things up
        gridSize += 2;
        SettingUpEnvironment();
        SettingUpPlayer();
        countdown = startCountDown;
        SpawnCollectible();
    }
    private void SettingUpEnvironment()
    {
        playMap.GenerateGrid(gridSize, xAxis, zAxis);
        //has instantiated 
        if (shuffledList != null)
        {
            shuffledList = playMap.ShuffleList();
        }
        else
        {
            shuffledList = new List<GameObject>(playMap.ShuffleList());
        }
    }

    public void ResetPlayerPosition()
    {
        playerExist = false;
        Vector3 respawnPosition = GetUnoccupiedPosition(playerExist);
        PlayerInstance.transform.position = new Vector3(respawnPosition.x, 1f, respawnPosition.z);
        PlayerInstance.transform.rotation = Quaternion.identity;
        playerExist = true;
    }
    private void SettingUpPlayer()
    {
        if (playerExist != true)
        {
            Vector3 spawnPosition = GetUnoccupiedPosition(playerExist);
            GameObject playerInstance = Instantiate(playerPrefab, new Vector3(spawnPosition.x, 1f, spawnPosition.z), Quaternion.identity);
            PlayerInstance = playerInstance;
            cameraFollow.Target = playerInstance.transform;
            playerExist = true;
        }
    }

    public void SpawnCollectible()
    {
        //spawn collectible with random between 1 to 2 times
        int limit = 3;
        //slow down spawning of collectibles
        if (shuffledList_occupied.Count >= shuffledList.Count / 3)
        {
            limit = 2;
        }
        int randomIndex = Random.Range(1, limit);
        for (int i = 0; i < randomIndex; i++)
        {
            Vector3 spawnPosition = GetUnoccupiedPosition(playerExist);
            if (spawnPosition == Vector3.zero)
            {
                return;
            }
            GameObject _collectible = Instantiate(collectibles[Random.Range(0, collectibles.Length)], spawnPosition, Quaternion.identity, obstaclesCollectibles_parent);
            objectsOnScene.Add(_collectible);
        }
    }

    private void SpawnObstacle()
    {
        Vector3 spawnPosition = GetUnoccupiedPosition(playerExist);
        if (spawnPosition == Vector3.zero)
        {
            return;
        }
        GameObject _obstacle = Instantiate(obstacle, spawnPosition, Quaternion.identity, obstaclesCollectibles_parent);
        objectsOnScene.Add(_obstacle);
    }

    public bool RemoveFromOccupiedList(GameObject removedObject)
    {
        for (int i = 0; i < shuffledList_occupied.Count; i++)
        {
            if (removedObject.transform.position.x == shuffledList_occupied[i].transform.position.x
                && removedObject.transform.position.z == shuffledList_occupied[i].transform.position.z)
            {
                shuffledList_occupied.RemoveAt(i);
                objectsOnScene.Remove(removedObject);
                return true;
            }
        }
        return false;
    }

    private void AddToOccupiedList(GameObject _obj)
    {
        shuffledList_occupied.Add(_obj);
    }

    private Vector3 GetUnoccupiedPosition(bool hasPlayerSpawned)
    {
        foreach (GameObject obj in shuffledList)
        {
            //If not occupied
            if (!shuffledList_occupied.Contains(obj))
            {
                if (hasPlayerSpawned == true)
                {
                    AddToOccupiedList(obj);
                }
                shuffledList = playMap.ShuffleList();
                return new Vector3(obj.transform.position.x, 1f, obj.transform.position.z);
            }
        }
        return Vector3.zero;
    }

    private bool GivePunishment()
    {
        if (currentTouchedCollectible == previousTouchedCollectible)
        {
            return true;
        }
        else
        {
            return false;
        }
    } 
    public void AddOrReduceScore(int _scorePoint)
    {
        if (GivePunishment())
        {
            ReduceScore(_scorePoint);
        }
        else
        {
            if (previousTouchedCollectible != null)
            {
                scorePoint += _scorePoint * comboModifier;
            }
            else
            {
                scorePoint += _scorePoint;
            }
        }

        previousTouchedCollectible = currentTouchedCollectible;
    }
    private void ReduceScore(int _scorePoint)
    {
        scorePoint -= _scorePoint*comboModifier;
        if (scorePoint <= 0)
        {
            scorePoint = 0;
        }
    }
    public void UpdateTouchedCollectible(Color _colour)
    {
        lastTouchedCollectible.text = currentTouchedCollectible;
        lastTouchedCollectible.color = _colour;
    }
}
