﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _isGameOver = false;
    [SerializeField] private AudioSource _audioSource;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //ReloadTheGame();
        //QuitApplication();
        // if(Input.GetKeyDown(KeyCode.Escape)){
        //     Time.timeScale = 0;
        // } else {
        //     Time.timeScale = 1;
        // }
    }

    private void ReloadTheGame()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        {
            SceneManager.LoadScene(1);//Game Scene
            _audioSource.volume = 0.3f;
        }
    }

    private void QuitApplication()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    
    public void GameIsOver()
    {
        _isGameOver = true;
        _audioSource.volume = 0;
    }

    public void ButtonLoadScene(int SceneIntValue)
    {
        SceneManager.LoadScene(SceneIntValue);
    }

    public void ResumeGame(){
        Time.timeScale = 1;
    }
}
