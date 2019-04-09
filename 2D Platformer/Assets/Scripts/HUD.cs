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

    public void SetScore(string scoreValue)
    {
        scoreLabel.text = scoreValue;
    }
}
