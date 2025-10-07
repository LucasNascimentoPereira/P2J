using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }


    [Header("Data Assets")]
    [SerializeField] private UIManagerData uiManagerData;


    [Header("Menu Panels")]    
    [SerializeField] private List<GameObject> panelsList = new();
    private Dictionary<string, GameObject> _panelDictionary = new();

    [Header("Necessary Buttons")]
    [SerializeField] private Button startLevelButton;
    
    private GameObject _currentMenu = null;
    private GameObject _previousMenu = null;

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
    private List<Image> heartImages = new();



    public bool DeviceVibration => deviceVibration;
    public UIManagerData UIManagerData => uiManagerData;
    public GameObject CurrentMenu => _currentMenu;

    public TMP_Text Coins => coins;

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
        foreach (var panel in panelsList)
        {
            _panelDictionary.Add(panel.name, panel);
        }
    }

    private void Start()
    {
        ChangeVersionNumber();
        heartImages = heartsContainer.GetComponentsInChildren<Image>(true).ToList();
        foreach (var image in heartImages)
        {
            image.sprite = UIManagerData.HealthImages[0];
        }
    }

    private void Update()
    {
        fpsCounter.text = (1 / Time.deltaTime).ToString("F1");
        //ChangeHealth(true, 2);
        //GameManager.Instance.AddCoins();
    }

    public void ShowPanel(string menuName)
    {
        Debug.Log(menuName);
        
        if (menuName == "NoMenu") _currentMenu.SetActive(false);
        if (menuName == "ShowPrevious")
        {
            _currentMenu.SetActive(false);
            if (_previousMenu)
            {
                _previousMenu.SetActive(true);
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

    public void ChangeHealth(bool damage, int index)
    {
        heartImages[index - 1].sprite = damage ? uiManagerData.HealthImages[1] : uiManagerData.HealthImages[0];
    }

    public void IncreaseMaxHealth(int index)
    {
        heartImages[index - 1].gameObject.SetActive(true);
    }
    
}


