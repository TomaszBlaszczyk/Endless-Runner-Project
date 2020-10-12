using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public AudioSource musicAudioSource;
    public AudioSource soundsAudioSource;

    public Image musicIcon;
    public Image soundsIcon;

    public bool paused = false;

    [SerializeField]
    private Sprite musicON;
    [SerializeField]
    private Sprite musicOFF;
    [SerializeField]
    private Sprite soundsON;
    [SerializeField]
    private Sprite soundsOFF;

    private bool musicActive = true;
    private bool soundsActive = true;

    private void Start()
    {
        if(PlayerPrefs.HasKey("Music"))
        {
            musicActive = Convert.ToBoolean(PlayerPrefs.GetInt("Music"));
        }

        if (PlayerPrefs.HasKey("Sounds"))
        {
            soundsActive = Convert.ToBoolean(PlayerPrefs.GetInt("Sounds"));
        }

        MusicAndSounds();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.GetComponent<Canvas>().enabled = !gameObject.GetComponent<Canvas>().enabled;

            if(gameObject.GetComponent<Canvas>().enabled)
            {
                Time.timeScale = 0;
                PlatformGenerator.instance.mainCanvas.enabled = false;
                paused = true;
            }
            else
            {
                Time.timeScale = 1;
                paused = false;

                if(!PlatformGenerator.gameStarted)
                    PlatformGenerator.instance.mainCanvas.enabled = true;
            }
        }
    }

    public void MusicButton() //Function used by Music Button in Menu window
    {
        musicActive = !musicActive;
        MusicAndSounds();
    }

    public void SoundsButton() //Function used by Sounds Button in Menu window
    {
        soundsActive = !soundsActive;
        MusicAndSounds();
    }

    public void MusicAndSounds() //Changing icons in menu and mute/unmute audio.
    {                            //Saving info in PlayerPrefs to remember our settings preferences
        if (musicActive == true)
        {
            musicAudioSource.mute = false;
            musicIcon.sprite = musicON;
            PlayerPrefs.SetInt("Music", 1);
        }
        else
        {
            musicAudioSource.mute = true;
            musicIcon.sprite = musicOFF;
            PlayerPrefs.SetInt("Music", 0);
        }

        if (soundsActive == true)
        {
            soundsAudioSource.mute = false;
            soundsIcon.sprite = soundsON;
            PlayerPrefs.SetInt("Sounds", 1);
        }
        else
        {
            soundsAudioSource.mute = true;
            soundsIcon.sprite = soundsOFF;
            PlayerPrefs.SetInt("Sounds", 0);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
