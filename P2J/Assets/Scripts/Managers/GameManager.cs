using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
 

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private GameManagerData gameManagerData;
    [SerializeField] private AudioManagerData audioManagerData;
    [SerializeField] private UIManagerData uiManagerData;

    [Header("Events")]
    private UnityEvent _onCoins = new();
    private UnityEvent _onLevelReset = new();

    [Header("Game Objects")]
    private HealthPlayerBase healthPlayer;
    private PlayerController playerController;
    private GameObject _ball;
    private GameObject _baseGameObject;
    private GameObject interactable;

    [Header("Level management")]
    [SerializeField] private int _currentGameLevel;

    [Header("Currency management")]
    [SerializeField] private int _coins;
    [SerializeField] private int _gems;
    private Dictionary<string, UpgradeData> prices = new();
    [Header("Purchase Values")]
    [SerializeField] private List<UpgradeData> upgradeData;

    private GameObject currentSpawnPoint;
    private Background background;

    public static GameManager Instance { get; private set; }
    public bool IsPaused { get; private set; } = false;
    private int _currentLevel = 0;

    private GameData _gameData;

    public GameData GameData => _gameData;
    public int CurrentGameLevel { get => _currentGameLevel; set => _currentGameLevel = value; }

    public int Coins => _coins;
    public int gems { get => _gems; set => _gems = value; }
    public GameObject CurrentSpawnPoint { get => currentSpawnPoint; set => currentSpawnPoint = value; }
    public HealthPlayerBase HealthPlayer {  get => healthPlayer; set => healthPlayer = value; }
    public PlayerController PlayerController { get => playerController; set => playerController = value; }
    public GameObject Interactable => interactable;
    public Dictionary<string, UpgradeData> Prices => prices;
    public List<UpgradeData> UpgradeData => upgradeData;
    public Background Background { get => background; set => background = value; }




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
        //_gameData = SaveSystem.LoadGame();
        _onCoins?.AddListener(UIManager.Instance.ChangeCoins);
        _onLevelReset?.AddListener(UIManager.Instance.ChangeHealth);
        for (int i = 0; i < upgradeData.Count; ++i)
        {
            prices.Add(upgradeData[i].UpgradeName, upgradeData[i]);
        }
    }

    private void OnEnable()
    {
        //LoadLevel(1);
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
        UIManager.Instance.ShowPanel(IsPaused ? "PauseMenu" : "NoMenu");
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
        //RaceConditionAvoider();
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
        if (!currentSpawnPoint)  return;
        if (!healthPlayer) return;
        healthPlayer.transform.position = currentSpawnPoint.transform.position;
        healthPlayer.TakeDamage(gameObject, false, -healthPlayer.MaxHealth);
        _onLevelReset.Invoke();
        background.ResetPos();
    }

    public void AddCoins()
    {
        _coins += 1;
        _onCoins.Invoke();
        //UIManager.Instance.Coins.text = _coins.ToString();
    }

    public Vector3 GetPlayerPosition()
    {
        return Vector3.zero;
    }

    public void UnlockAbility()
    {
        if(!interactable) return;
        UIManager.Instance.ShowPanel("Skilles");
        StartCoroutine(UIManager.Instance.DisappearImage(interactable.name));
        Destroy(interactable);
        interactable = null;
    }

    public void PurchaseAbilityUpgrade(string upgrade)
    {
        if(prices.TryGetValue(upgrade, out var price))
        {
            if (_coins >= price.UpgradeValue)
            {
                _coins -= price.UpgradeValue;
                if (!playerController) return;
                playerController.AbilityUnlock(upgrade);
            }
        }
    }
    public void RegisterInteractable(GameObject envInteractable)
    {
        interactable = envInteractable;
    }

}
