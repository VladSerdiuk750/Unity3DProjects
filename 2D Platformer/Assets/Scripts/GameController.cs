using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public enum GameState
{
    Play,
    Pause
}

public delegate void InventoryUsedCallback(InventoryUIButton uiButton);

public delegate void UpdateHeroParametersHandler(HeroParameters parameters);

public class GameController : MonoBehaviour
{
    public event UpdateHeroParametersHandler OnUpdateHeroParameters;
    
    [SerializeField]
    private int dragonHitScore = 10;

    [SerializeField] 
    private int dragonKillScore = 50;

    [SerializeField] private int dragonKillExperience;

    [SerializeField] private Audio audioManager;

    public Audio AudioManager
    {
        get => audioManager;
        set => audioManager = value;
    }

    [SerializeField] private HeroParameters hero;

    public HeroParameters Hero
    {
        get => hero;
        set => hero = value;
    }
    
    [SerializeField]
    private List<InventoryItem> inventory;

    public List<InventoryItem> Inventory
    {
        get => inventory;
        set => inventory = value;
    }

    private static GameController _instance;

    public static GameController Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject gameController = Instantiate(Resources.Load("Prefabs/GameController")) as GameObject;
                _instance = gameController.GetComponent<GameController>();
            }

            return _instance;
        }
    }

    [SerializeField]
    private Knight knight;

    public Knight Knight
    {
        get => knight;
        set => knight = value;
    }


    private GameState state;

    public GameState State
    {
        get => state;

        set
        {
            Time.timeScale = value == GameState.Play ? 1.0f : 0.0f;
            state = value;
        }
    }
    
    private int score = 0;
    public int Score
    {
        get => score;
        set
        {
            if (value != score)
            {
                score = value;
                HUD.Instance.SetScore(score.ToString());
            }
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
        
        DontDestroyOnLoad(gameObject);
        
        State = GameState.Play;
        inventory = new List<InventoryItem>();
        InitializeAudioManager();
    }

    public void StartNewLevel()
    {
        HUD.Instance.SetScore(Score.ToString());

        if (OnUpdateHeroParameters != null)
        {
            OnUpdateHeroParameters(hero);
        }

        State = GameState.Play;
    }

    public void Hit(IDestructable victim)
    {
        if (victim.GetType() == typeof(Dragon))
        {
            Score += dragonHitScore;
        }
        else if (victim.GetType() == typeof(Knight))
        {
            HUD.Instance.HealthBar.value = victim.Health;
        }
    }

    public void Killed(IDestructable victim)
    {
        if (victim.GetType() == typeof(Dragon))
        {
            Score += dragonKillScore;
            hero.Experience += dragonKillExperience;
            Destroy((victim as MonoBehaviour).gameObject);
        }

        if (victim.GetType() == typeof(Knight))
        {
            GameOver();
        }
    }
    

    public void AddInventoryItem(InventoryItem itemData)
    {
        InventoryUIButton newUiButton = HUD.Instance.AddNewInventoryItem(itemData);
        InventoryUsedCallback callback = new InventoryUsedCallback(InventoryItemUsed);
        newUiButton.Callback = callback;
        inventory.Add(itemData);
    }

    public void InventoryItemUsed(InventoryUIButton item)
    {
        switch (item.ItemData.CrystallType)
        {
            case CrystallType.Blue:
                hero.Speed += item.ItemData.Quantity / 10f; 
                break;
            case CrystallType.Red:
                hero.Damage += item.ItemData.Quantity / 10f;
                break;
            case CrystallType.Green:
                hero.MaxHealth += item.ItemData.Quantity / 10f;
                break;
            default: 
                Debug.LogError("Wrong crystall type!");
                break;
        }

        inventory.Remove(item.ItemData);
        Destroy(item.gameObject);
        if (OnUpdateHeroParameters != null) OnUpdateHeroParameters(hero);
    }

    public void LevelUp()
    {
        if (OnUpdateHeroParameters != null)
        {
            OnUpdateHeroParameters(hero);
        }
    }

    public void LoadNextLevel()
    {
        State = GameState.Play;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
    }

    public void RestartLevel()
    {
        State = GameState.Play;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void PrincessFound()
    {
        HUD.Instance.ShowLevelWonWindow();
    }

    public void GameOver()
    {
        HUD.Instance.ShowLevelLoseWindow();
    }

    private void InitializeAudioManager()
    {
        audioManager.SourceSfx = gameObject.AddComponent<AudioSource>();
        audioManager.SourceMusic = gameObject.AddComponent<AudioSource>();
        audioManager.SourceRandomPitchSfx = gameObject.AddComponent<AudioSource>();
        gameObject.AddComponent<AudioListener>();
    }
}