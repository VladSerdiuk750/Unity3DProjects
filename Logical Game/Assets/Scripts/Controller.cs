using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private static Controller m_instance;

    public static Controller Instance
    {
        get
        {
            if (m_instance == null)
            {
                var controller = Instantiate(Resources.Load("Prefabs/Controller")) as GameObject;
                m_instance = controller.GetComponent<Controller>();
            }

            return m_instance;
        }
    }

    [SerializeField] private Color[] m_tokenColors;

    private List<List<Token>> m_tokensByTypes;
    
    private Field m_field;

    [SerializeField] private LevelParameters m_level;

    private int m_currentLevel;

    [SerializeField]
    private Score m_score; 
    
    [SerializeField]
    private Audio m_audio = new Audio();

    public List<List<Token>> TokensByTypes
    {
        get => m_tokensByTypes;
        set => m_tokensByTypes = value;
    }

    public int FieldSize => m_level.FieldSize;

    public int TokenTypes => m_level.TokenTypes;

    public Score Score => m_score;
    
    public Color[] TokenColors
    {
        get => m_tokenColors;
        set => m_tokenColors = value;
    }
    public Field Field
    {
        get => m_field;
        set => m_field = value;
    }

    public int CurrentLevel
    {
        get => m_currentLevel;
        set => m_currentLevel = value;
    }
    public Audio Audio => m_audio;

    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (m_instance != this)
            {
                Destroy(gameObject);
            }
        }
        
        DontDestroyOnLoad(gameObject);
        
        Audio.SourceMusic = gameObject.AddComponent<AudioSource>();
        Audio.SourceRandomPitchSfx = gameObject.AddComponent<AudioSource>();
        Audio.SourceSfx = gameObject.AddComponent<AudioSource>();
        
        DataStore.LoadOptions();
    }

    private void Start()
    {
        DataStore.LoadGame();
        InitializeLevel();
        Audio.PlayMusic(true);
        
    }

    public void InitializeLevel()
    {
        m_level = new LevelParameters(m_currentLevel);

        TokenColors = new Color[]
        {
            new Color(1f, 0.2156863f, 0.2156863f, 1f),
            new Color(1f, 0.3960784f, 0.1568628f, 1f),
            new Color(1f, 0.9568627f, 0.3098039f, 1f),
            new Color(0.4941176f, 1f, 0.3960784f, 1f),
            new Color(0.6862745f, 0.98f, 1f, 1f),
            new Color(0.3803922f, 0.3176471f, 1f, 1f),
            new Color(1f, 0.4f, 0.7372549f, 1f),
            new Color(0.627451f, 0.3f, 1f, 1f),
        };

        TokensByTypes = new List<List<Token>>();

        for (int i = 0; i < m_level.TokenTypes; i++)
        {
            TokensByTypes.Add(new List<Token>());
        }

        m_field = Field.Create(m_level.FieldSize, m_level.FreeSpace);
    }
    // private Color[] MakeColors(int count)
    // {
    //     Color[] result = new Color[count];

    //     float colorStep = 1f / (count + 1);

    //     float hue = 0f;
    //     float saturation = 0.5f;
    //     float value = 1f;

    //     for (int i = 0; i < count; i++)
    //     {
    //         float newHue = hue + (colorStep * i);

    //         result[i] = Color.HSVToRGB(newHue, saturation, value);
    //     }

    //     return result;
    // }

    public void TurnDone()
    {
        Audio.PlaySound("Drop");
        if (IsAllTokensConnected())
        {
            Debug.Log("Win!");
            Audio.PlaySound("Victory");
            Score.AddLevelBonus();
            m_currentLevel++;
            Hud.Instance.CountScore(m_level.Turns);
            Destroy(m_field.gameObject);    
        }
        else
        {
            Debug.Log("Continue...");
            if (m_level.Turns > 0)
            {
                m_level.Turns--;
            }
        }
    }

    public bool IsAllTokensConnected()
    {
        //TODO: 
        //Optimize: check only those type, token of which was moved

        for (int i = 0; i < TokensByTypes.Count; i++)
        {
            if (IsAllTokensConnected(TokensByTypes[i]) == false)
            {
                return false;
            }
        }

        return true;
    }

    private bool IsAllTokensConnected(List<Token> tokens)
    {
        if (tokens.Count == 0)
        {
            return true;
        }
        
        List<Token> connectedTokens = new List<Token>();
        connectedTokens.Add(tokens[0]);
        bool moved = true;

        while (moved)
        {
            moved = false;

            for (int i = 0; i < connectedTokens.Count; i++)
            {
                for (int j = 0; j < tokens.Count; j++)
                {
                    if (IsTokensNear(tokens[j], connectedTokens[i]))
                    {
                        if (connectedTokens.Contains((tokens[j])) == false)
                        {
                            connectedTokens.Add(tokens[j]);
                            moved = true;
                        }
                    }
                }
            }
        }

        if (tokens.Count == connectedTokens.Count)
        {
            return true;
        }

        return false;
    }

    private bool IsTokensNear(Token first, Token second)
    {
        if ((int) first.transform.position.x == (int) second.transform.position.x + 1 ||
            (int) first.transform.position.x == (int) second.transform.position.x - 1)
        {
            if ((int) first.transform.position.y == (int) second.transform.position.y)
            {
                return true;
            }
        }

        if ((int) first.transform.position.y == (int) second.transform.position.y + 1 ||
            (int) first.transform.position.y == (int) second.transform.position.y - 1)
        {
            if ((int) first.transform.position.x == (int) second.transform.position.x)
            {
                return true;
            }
        }

        return false;
    }

    public void Reset()
    {
        CurrentLevel = 1;
        Score.CurrentScore = 1;
        Destroy(m_field.gameObject);
        DataStore.SaveGame();
        InitializeLevel();
    }
}
