using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject optionsWindow;
    
    private void Start()
    {
        Time.timeScale = 1f;
        GameController.Instance.AudioManager.PlayMusic(true);
    }

    public void PlayButton()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
        GameController.Instance.AudioManager.PlayMusic(false);
    }

    public void OptionsButton()
    {
        
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
        GameController.Instance.AudioManager.PlayMusic(true);
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
    
    public void SetSoundVolume(Slider slider)
    {
        GameController.Instance.AudioManager.SfxVolume = slider.value;
    }

    public void SetMusicVolume(Slider slider)
    {
        GameController.Instance.AudioManager.MusicVolume = slider.value;    
    }
}
