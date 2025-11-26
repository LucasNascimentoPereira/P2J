using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Data Assets")]
    [SerializeField] private UIManagerData uiManagerData;


    [Header("Menu Panels")]
    [SerializeField] private GameObject panelsGameObject;
    [SerializeField] private List<GameObject> panelsList = new();
    private Dictionary<string, GameObject> _panelDictionary = new();
    [SerializeField] private Canvas canvas;

    [Header("Necessary Buttons")]
    [SerializeField] private Button startLevelButton;
    
    private GameObject _currentMenu = null;
    private GameObject _previousMenu = null;

    [SerializeField] EventSystem _eventSystem;

    [Header("Notification Texts")]
    [SerializeField] private TMP_Text notificationsText;
    [SerializeField] private TMP_Text notificationsType;

    [Header("Device Utilization")]
    [SerializeField] private bool deviceVibration;

    [Header("Game Information")]
    [SerializeField] private TMP_Text versionNumber;
    [SerializeField] private TMP_Text fpsCounter;
    [SerializeField] private TMP_Text coins;
    [SerializeField] private GameObject heartsContainer;
    [SerializeField] private GameObject abilityImagesContainer;
    private List<Image> heartImages = new();
    private Dictionary<string, Image> abilityImages = new();
    private Coroutine _abilityImageCoroutine;

    InputAction esc;

    [SerializeField] private UnityEvent onPlaySound = new();



    public bool DeviceVibration => deviceVibration;
    public UIManagerData UIManagerData => uiManagerData;
    public GameObject CurrentMenu => _currentMenu;
    public GameObject PreviousMenu { get => _previousMenu; set => _previousMenu = value; }
    
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        for (int i = 0; i < panelsGameObject.transform.childCount; ++i)
        {
            _panelDictionary.Add(panelsGameObject.transform.GetChild(i).name, panelsGameObject.transform.GetChild(i).gameObject);
        }
        foreach (var image in abilityImagesContainer.GetComponentsInChildren<Image>(true))
        {
            abilityImages.Add(image.gameObject.name, image);
        }
        heartImages = heartsContainer.GetComponentsInChildren<Image>(true).ToList();
        foreach (var image in heartImages)
        {
            image.sprite = UIManagerData.HealthImages[0];
        }

    }

    private void Start()
    {
        ChangeVersionNumber();
        esc = InputSystem.actions.FindAction("Pause");
    }

    private void Update()
    {
        fpsCounter.text = (1 / Time.deltaTime).ToString("F1");
        if (esc.WasPressedThisFrame())
        {
            ShowPanel("ShowPrevious");
            onPlaySound.Invoke();
        }
    }

    public void ShowPanel(string menuName)
    {
        Debug.Log(menuName);
        
        if (menuName == "NoMenu") _currentMenu.SetActive(false);
        if (menuName == "ShowPrevious")
        {
            if (_previousMenu && _previousMenu != _currentMenu)
            {
                _currentMenu.SetActive(false);
                GameObject menu = _currentMenu;
                _currentMenu = _previousMenu;
                _previousMenu = menu;
                _currentMenu.SetActive(true);
            }
            if (_currentMenu.name == "PauseMenu" && !GameManager.Instance.IsPaused)
            {
                GameManager.Instance.PauseGame(true);
            }
        }

        else
        {
            if (_currentMenu)
            {
                _previousMenu = _currentMenu;
                _currentMenu.SetActive(false);
                _currentMenu = _panelDictionary.GetValueOrDefault(menuName);
                if (_currentMenu)
                {
                    _currentMenu.SetActive(true);
                }
            }
            else
            {
                _currentMenu = _panelDictionary.GetValueOrDefault(menuName);
                Debug.Log(_currentMenu);
                Debug.Log(_currentMenu);
                if (_currentMenu)
                {
                    _currentMenu.SetActive(true);
                }
            }
        }
        if (_currentMenu != null) 
        {
            _eventSystem.SetSelectedGameObject(_currentMenu.transform.GetChild(0).gameObject);
        }
        if (menuName == "Skilles")
        {
            if(_currentMenu.TryGetComponent(out Skilles skilles))
            {
                skilles.UpdateMenu();
            }
        }
        Debug.Log("Changed to menu: " + _panelDictionary.GetValueOrDefault(menuName));
    }

    public void HandleAmbiguousButtons(bool isBack)
    {
        if (isBack)
        {
            switch (GameManager.Instance.GetCurrentLevelByIndex())
            {
                case 0:
                    ShowPanel("MainMenu");
                    break;
                case 1:
                    ShowPanel("Pause");
                    break;
                default:
                    Debug.Log("Index out of bounds or ShowPanel() error");
                    break;
            }
        }
        else
        {
            switch (GameManager.Instance.GetCurrentLevelByIndex())
            {
                case 0:
                    QuitGame();
                    break;
                case 1:
                    GameManager.Instance.LoadLevel(0);
                    break;
                default:
                    Debug.Log("Index out of bounds or GameManager LoadLevel() error");
                    break;
            }
        }

        Debug.Log("Ambiguous Button Handled");
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenLink(int index)
    {
        if (uiManagerData.Links == null) Debug.Log("List does not exist or has not been initialized");
        if (uiManagerData.Links[index] == null) Debug.Log("Link does not exist in current context");
        Application.OpenURL(uiManagerData.Links[index]);
    }

    public void ChangeDeviceVibration(bool value)
    {
        deviceVibration = value;
    }

    public void NotificationHandler(int notification, int notificationType)
    {
        
        //panelsList[^1].SetActive(true);
        notificationsText.text = uiManagerData.Notifications[notification];
        notificationsType.text = uiManagerData.NotificationType[notificationType];
        StartCoroutine(AppearText());
    }

    private IEnumerator AppearText()
    {
        var occurredTime = 0.0f;
        var interval = 1 / (uiManagerData.NotificationAppearTime * 100) * (uiManagerData.NotificationInterval * 100);
        
        while (occurredTime < uiManagerData.NotificationAppearTime)
        {
            notificationsText.alpha = Mathf.Clamp(notificationsText.alpha + interval, 0.0f, 1.0f);
            notificationsType.alpha = Mathf.Clamp(notificationsType.alpha + interval, 0.0f, 1.0f);
            occurredTime += uiManagerData.NotificationInterval;
            yield return new WaitForSeconds(uiManagerData.NotificationInterval);
        }

        yield return new WaitForSeconds(uiManagerData.NotificationTime);
        StartCoroutine(DisappearText());
        StopCoroutine(AppearText());
    }

    private IEnumerator DisappearText()
    {
        var occurredTime = 0.0f;
        var interval = 1 / (uiManagerData.NotificationDisappearTime * 100) * (uiManagerData.NotificationInterval * 100);
        
        while (occurredTime < uiManagerData.NotificationDisappearTime)
        {
            notificationsText.alpha = Mathf.Clamp(notificationsText.alpha - interval, 0.0f, 1.0f);
            notificationsType.alpha = Mathf.Clamp(notificationsType.alpha - interval, 0.0f, 1.0f);
            occurredTime += uiManagerData.NotificationInterval;
            yield return new WaitForSeconds(uiManagerData.NotificationInterval);
        }
        yield return null;
        panelsList[^1].SetActive(false);
        StopCoroutine(DisappearText());
    }

    public IEnumerator AppearImage(string image)
    {
        if (!abilityImages.TryGetValue(image, out var result)) StopCoroutine(AppearImage(image));
        result.enabled = true;
        while (result.color.a < 1)
        {
            result.color = new Color(1.0f, 1.0f, 1.0f, Mathf.Clamp(result.color.a + uiManagerData.AppearImageInterval, 0.0f, 1.0f));
            yield return new WaitForSeconds(uiManagerData.AppearImageTimeInterval);
        }
    }

    public IEnumerator DisappearImage(string image)
    {
        if (!abilityImages.TryGetValue(image, out var result)) StopCoroutine(DisappearImage(image));
        while(result.color.a > 0)
        {
            result.color = new Color(1.0f, 1.0f, 1.0f, Mathf.Clamp(result.color.a - uiManagerData.DisappearImageInterval, 0.0f, 1.0f));
            yield return new WaitForSeconds(uiManagerData.DisappearImageTimeInterval);
        }
        result.enabled = false;
    }

    public void ChangeResolution(int index)
    {
        Screen.SetResolution(uiManagerData.Resx[index], uiManagerData.Resy[index], Screen.fullScreen);
    }

    public void ChangeFullScreen(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
    }

    public void ChangeVerticalSync(bool verticalSync)
    {
        QualitySettings.vSyncCount = verticalSync ? 1 : 0;
    }

    public void ChangeGamma(float changedGamma)
    {
        float clampedGamma = Mathf.Clamp(changedGamma, 0.1f, 1.0f);
        RenderSettings.ambientLight = new Color(clampedGamma, clampedGamma, clampedGamma, 1.0f);
    }

    private void ChangeVersionNumber()
    {
        versionNumber.text = "Version " + Application.version;
    }

    public void ChangeFps(int index)
    {
        if (index == uiManagerData.FpsLimit.Count - 1)
        {
            Application.targetFrameRate = 0;
        }
        
        Application.targetFrameRate = int.Parse(uiManagerData.FpsLimit[index]);
    }

    public void ChangeHealth()
    {
        var currHealth = GameManager.Instance.HealthPlayer.CurrentHealth;
        var maxHealth = GameManager.Instance.HealthPlayer.MaxHealth;
        for(int i = 0; i < currHealth; ++i)
        {
            heartImages[i].sprite = uiManagerData.HealthImages[0];
        }
        for(int i = (int)currHealth; i < maxHealth; ++i)
        {
            heartImages[i].sprite = uiManagerData.HealthImages[1];
        }
    }

    public void IncreaseMaxHealth(int index)
    {
        heartImages[index - 1].gameObject.SetActive(true);
    }

    public void ChangeCoins()
    {
        coins.text = GameManager.Instance.Coins.ToString();
    }

    public void CameraReference(Camera camera)
    {
        canvas.worldCamera.gameObject.SetActive(false);
        canvas.worldCamera = camera;
        canvas.worldCamera.gameObject.SetActive(true);
    }
    
}


