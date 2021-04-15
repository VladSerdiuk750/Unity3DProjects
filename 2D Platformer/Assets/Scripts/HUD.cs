using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HUD : MonoBehaviour
{
    private static HUD _instance;

    [SerializeField]
    private Text scoreLabel;

    [SerializeField]
    private Slider healthBar;

    [SerializeField] private GameObject inventoryWindow;

    [SerializeField] private GameObject victoryWindow;

    [SerializeField] private GameObject LevelLoseWindow;

    [SerializeField] private GameObject InGameWindow;

    [SerializeField] private GameObject optionsWindow;

    [SerializeField] private InventoryUIButton inventoryUiButtonPrefab;

    [SerializeField] private Transform inventoryContainer;

    [SerializeField]
    private Text damageValue;

    public Text DamageValue
    {
        get => damageValue;
        set => damageValue = value;
    }
    
    [SerializeField]
    private Text healthValue;

    public Text HealthValue
    {
        get => healthValue;
        set => healthValue = value;
    }
    
    [SerializeField]
    private Text speedValue;

    public Text SpeedValue
    {
        get => speedValue;
        set => speedValue = value;
    }
    
    public Slider HealthBar
    {
        get => healthBar;
        set => healthBar = value;
    }

    public static HUD Instance => _instance;

    void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        LoadInventory();

        GameController.Instance.OnUpdateHeroParameters += HandleOnUpdateHeroParameters;
        
        GameController.Instance.StartNewLevel();
    }

    public void SetScore(string scoreValue)
    {
        scoreLabel.text = scoreValue;
    }

    public void ShowWindow(GameObject window)
    {
        window.GetComponent<Animator>().SetBool("Open", true);
        GameController.Instance.State = GameState.Pause;
    }

    public void HideWindow(GameObject window)
    {
        window.GetComponent<Animator>().SetBool("Open", false);
        GameController.Instance.State = GameState.Play;
    }

    public InventoryUIButton AddNewInventoryItem(InventoryItem itemData)
    {
        InventoryUIButton newUiButton = Instantiate(inventoryUiButtonPrefab) as InventoryUIButton;
        newUiButton.transform.SetParent(inventoryContainer);
        newUiButton.ItemData = itemData;
        return newUiButton;
    }

    public void UpdateCharacterValues(float newHealth, float newSpeed, float newDamage)
    {
        healthValue.text = newHealth.ToString();
        speedValue.text = newSpeed.ToString();
        damageValue.text = newDamage.ToString();
    }

    public void LoadInventory()
    {
        InventoryUsedCallback callback = new InventoryUsedCallback(GameController.Instance.InventoryItemUsed);
        for (int i = 0; i < GameController.Instance.Inventory.Count; i++)
        {
            InventoryUIButton newItem = AddNewInventoryItem(GameController.Instance.Inventory[i]);
            newItem.Callback = callback;
        }
    }

    private void HandleOnUpdateHeroParameters(HeroParameters parameters)
    {
        HealthBar.maxValue = parameters.MaxHealth;
        HealthBar.value = parameters.MaxHealth;
        UpdateCharacterValues(parameters.MaxHealth, parameters.Speed, parameters.Speed);
    }

    private void OnDestroy()
    {
        GameController.Instance.OnUpdateHeroParameters -= HandleOnUpdateHeroParameters;    
    }


    public void SetSoundVolume(Slider slider)
    {
        GameController.Instance.AudioManager.SfxVolume = slider.value;
    }

    public void SetMusicVolume(Slider slider)
    {
        GameController.Instance.AudioManager.MusicVolume = slider.value;    
    }
    
    public void ButtonNext()
    {
        GameController.Instance.LoadNextLevel();
    }

    public void ButtonRestart()
    {
        GameController.Instance.RestartLevel();
    }

    public void ButtonMainMenu()
    {
        GameController.Instance.LoadMainMenu();
    }

    public void ShowLevelWonWindow()
    {
        ShowWindow(victoryWindow);
    }

    public void ShowLevelLoseWindow()
    {
        ShowWindow(LevelLoseWindow);
    }
}
