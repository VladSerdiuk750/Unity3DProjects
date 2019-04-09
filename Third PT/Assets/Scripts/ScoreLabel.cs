using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreLabel : MonoBehaviour
{
    private static int score;

    public static int Score {

        get
        {
            return score;
        }
        set
        {
            score = value;
            label.text = score.ToString();
        }
    }

    private static Text label;

    // Start is called before the first frame update
    void Start()
    {
        label = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        label.text = score.ToString();
    }
}
