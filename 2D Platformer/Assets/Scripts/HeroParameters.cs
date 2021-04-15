using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

[System.Serializable]
public class HeroParameters
{
    #region Private_Variables

    [SerializeField] private float maxHealth = 100f;

    [SerializeField] private float damage = 20f;

    [SerializeField] private float speed = 5f;

    [SerializeField] private int experience = 0;

    private int nextExperienceLevel = 100;

    private int previousExperienceLevel = 0;

    private int level = 1;

    #endregion

    public float MaxHealth
    {
        get => maxHealth;
        set => maxHealth = value;
    }

    public float Damage
    {
        get => damage;
        set => damage = value;
    }

    public float Speed
    {
        get => speed;
        set => speed = value;
    }

    public int Experience
    {
        get => experience;
        set
        {
            experience = value;
            CheckExperienceLevel();
        } 

    }

    private void CheckExperienceLevel()
    {
        if (experience > nextExperienceLevel)
        {
            level++;

            int addition = previousExperienceLevel;
            previousExperienceLevel = nextExperienceLevel;
            nextExperienceLevel += addition;

            switch (Random.Range(0,3))
            {
                case 0: maxHealth++;
                    break;
                case 1: damage++;
                    break;
                case 2: speed++;
                    break;
            }

            GameController.Instance.LevelUp();
        }
        
        
    }
}
