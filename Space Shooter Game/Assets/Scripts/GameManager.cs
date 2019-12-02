using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _isGameOver = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //ReloadTheGame();
        QuitApplication();
    }

    private void ReloadTheGame()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        {
            SceneManager.LoadScene(1);//Game Scene
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
    }

    public void ButtonLoadGame(int SceneIntValue)
    {
        SceneManager.LoadScene(SceneIntValue);
    }

    public void ButtonRestartGame()
    {
        SceneManager.LoadScene(1);//Game Scene
    }
}
