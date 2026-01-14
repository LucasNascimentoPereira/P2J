using System;
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
    [SerializeField] private List<GameObject> panelsList = new();
    
    private GameObject _currentMenu = null;
    private GameObject _previousMenu = null;

    [SerializeField] private Canvas _currentCanvas;
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

    [SerializeField] private GameObject mapUnlock;

    InputAction esc;

    [SerializeField] private UnityEvent onPlaySound = new();

    private MenusBaseState _menusBaseState = null;
    private string binding;
    private Coroutine _timerCoroutine;
    private bool sceneIsPlayed;

    public bool SceneIsPlayed {	get => sceneIsPlayed; set => sceneIsPlayed = value; }

    public enum menusState
    {
        MAINMENU,
        PAUSEMENU,
        HUD,
        OPTIONSPAUSE,
        OPTIONSMAINMENU,
        SKILLES,
        AREYOUSUREPAUSE,
        AREYOUSUREEXIT,
        CREDITSMENU,
        FADEMENU,
        INPUTMENU,
        MAPMENU,
	LEVELTMENU,
        NONE
    }

    private menusState menuState;


    public bool DeviceVibration => deviceVibration;
    public UIManagerData UIManagerData => uiManagerData;
    public GameObject CurrentMenu { get => _currentMenu ;set => _currentMenu = value;}
    public GameObject PreviousMenu { get => _previousMenu; set => _previousMenu = value; }
    public Coroutine AbilityImageUnlock { get => _abilityImageCoroutine; set => _abilityImageCoroutine = value; }
    public menusState MenuState => menuState;
    public List<GameObject> PanelsList => panelsList;
    public string Binding => binding;
    public int MapImages => mapUnlock.transform.childCount;
    
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
        ShowPanelEnum(menusState.MAINMENU);
    }

    private void Update()
    {
        fpsCounter.text = (1 / Time.deltaTime).ToString("F1");
        if (esc.WasPressedThisFrame() && _menusBaseState != null && menuState != menusState.LEVELTMENU)
        {
            _menusBaseState.ExitState();
            onPlaySound.Invoke();
        }
    }

    public void ShowPanelString(string name)
    {
        ShowPanelEnum(Enum.Parse<menusState>(name));
    }

    public void ShowPanelEnum(menusState menusState)
    {
        _menusBaseState = null;
        switch (menusState) 
        {
            case menusState.NONE:
                _menusBaseState = new MenuNone();
                break;
            case menusState.MAINMENU:
                _menusBaseState = new MainMenu();
                break;
            case menusState.PAUSEMENU:
                _menusBaseState = new PauseMenu();
                break;
            case menusState.HUD:
                _menusBaseState = new MainLevel();
                break;
            case menusState.OPTIONSPAUSE:
                _menusBaseState = new Options();
                break;
            case menusState.OPTIONSMAINMENU:
                _menusBaseState = new OptionsMainMenu();
                break;
            case menusState.SKILLES:
                _menusBaseState = new Skilles();
                break;
            case menusState.AREYOUSUREPAUSE:
                _menusBaseState = new AreYouSurePause();
                break;
            case menusState.AREYOUSUREEXIT:
                _menusBaseState = new AreYouSureExit();
                break;
            case menusState.CREDITSMENU:
                _menusBaseState = new CreditsMenu();
                break;
            case menusState.FADEMENU:
                _menusBaseState = new FadeMenu();
                break;
            case menusState.INPUTMENU:
                _menusBaseState = new InputMenu();
                break;
            case menusState.MAPMENU:
                _menusBaseState = new MapMenu();
                break;
	    case menusState.LEVELTMENU:
		_menusBaseState = new LevelTMenu();
		break;
            default: Debug.LogError("No menu by that ID"); 
                break;
        }
        menuState = menusState;
        _menusBaseState.BeginState(this);
    }

    public void UpdateState()
    {
        _menusBaseState.UpdateState();
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenLink(int index)
    {
        if (uiManagerData.Links == null) return;
        if (uiManagerData.Links[index] == null) return;
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

    private IEnumerator DisappearImage(string image)
    {
        if (!abilityImages.TryGetValue(image, out var result)) StopCoroutine(_abilityImageCoroutine);
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

    public void ActivateDisappearImage(string image)
    {
        _abilityImageCoroutine = StartCoroutine(DisappearImage(image));
    }

    public void CameraReference()
    {
        _currentCanvas.worldCamera = Camera.main;
    }

    public void KeyBinding(string inputAction)
    {
        binding = inputAction;
        _menusBaseState.UpdateState();
    }

    public void UnlockMap(int index)
    {
	    Debug.Log("MapUnlock" + index);
	for (int i = 0; i < mapUnlock.transform.childCount; ++i)
	{
		Debug.Log("MapUnlock");
		if (i <= index)
		{
			mapUnlock.transform.GetChild(i).gameObject.SetActive(false);
		}
	}
    }
    private IEnumerator IdleTime(float time)
    {
	    yield return new WaitForSeconds(time);
	    _timerCoroutine = null;
	    if (_menusBaseState != null)
	    {
		    _menusBaseState.ExitState();
	    }
    }

    public void BeginIdleTime(float time)
    {
	    EndIdleTime();
	    _timerCoroutine = StartCoroutine(IdleTime(time));
    }
    public void EndIdleTime()
    {
	    if (_timerCoroutine != null)
	    {
		    StopCoroutine(_timerCoroutine);
		    _timerCoroutine = null;
	    }
    }
    
}


