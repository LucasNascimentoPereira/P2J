using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private GameManagerData gameManagerData;
    [SerializeField] private AudioManagerData audioManagerData;
    [SerializeField] private UIManagerData uiManagerData;

    [Header("Game Objects")]
    private GameObject _hitter;
    private GameObject _ball;
    private GameObject _baseGameObject;

    [Header("Level management")]
    [SerializeField] private int _currentGameLevel;

    [Header("Currency management")]
    [SerializeField] private int _coins;
    [SerializeField] private int _gems;
    
    public static GameManager Instance { get; private set; }
    public bool IsPaused { get; private set; } = false;
    private int _currentLevel = 0;

    private GameData _gameData;

    public GameData GameData => _gameData;
    public int CurrentGameLevel { get => _currentGameLevel; set => _currentGameLevel = value; }

    public int Coins { get => _coins; set => _coins = value; }
    public int gems { get => _gems; set => _gems = value; }



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _gameData = SaveSystem.LoadGame();
    }

    private void OnEnable()
    {
        LoadLevel(1);
    }
    

    public void LoadLevel(int levelIndex)
    {
        Debug.Log("Level changed to level: " + levelIndex);
        _currentLevel = levelIndex;
        StartCoroutine(LoadNextLevelAsyc(levelIndex));
    }

    public int GetCurrentLevelByIndex()
    {
        return _currentLevel;
    }

    public void PauseGame(bool paused)
    {
        IsPaused = paused;
        Time.timeScale = paused ? 0.0f : 1.0f;
    }

    private IEnumerator LoadNextLevelAsyc(int level)
    {
        if (IsPaused)
        {
            Time.timeScale = 1.0f;
        }
        //UIManager.Instance.ShowPanel("LevelTransition");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(level);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        RaceConditionAvoider();
        //yield return new WaitForSeconds(0.2f);
    }
   
    public void SaveGame()
    {
        
    }
  
   

    public void StartGame()
    {
        Debug.Log("Game started");
        
    }

    public void RaceConditionAvoider()
    {
        var raceAvoidanceList = FindObjectsByType<Component>(FindObjectsSortMode.None).OfType<IInitializable>().ToList();

        if(raceAvoidanceList == null) return;

        foreach (var item in raceAvoidanceList)
         {
                item.Init();
         }

    }

   

    public void LevelReset()
    {
       
    }

   

  

}
