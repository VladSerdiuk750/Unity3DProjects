using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Play,
    Pause
}

public class GameController : MonoBehaviour
{
    [SerializeField]
    private int dragonHitScore = 10;

    [SerializeField] 
    private int dragonKillScore = 50;
    
    private static GameController _instance;
    public static GameController Instance => _instance; 

    private GameState state;

    private float maxHealth = 100f;

    public float MaxHealth
    {
        get => maxHealth;
        set => maxHealth = value;
    }

    private int score;
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
        state = GameState.Play;
        Score = 0;
        _instance = this;
    }

    private void Start()
    {
        HUD.Instance.HealthBar.maxValue = maxHealth;
        HUD.Instance.HealthBar.value = maxHealth;
        HUD.Instance.SetScore(Score.ToString());
    }

    public void Hit(IDestructable victim)
    {
        if (victim.GetType() == typeof(Dragon))
        {
            Score += victim.Health > 0 ? dragonHitScore : dragonKillScore;
        }
        else if (victim.GetType() == typeof(Knight))
        {
            HUD.Instance.HealthBar.value = victim.Health;
        }
    }
}