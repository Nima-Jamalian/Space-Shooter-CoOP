using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText = default;
    [SerializeField] private Text _highScoreText = default;
    [SerializeField] private Image _livesImage = default;
    [SerializeField] private Text _gameOverText = default;
    [SerializeField] private Sprite[] _livesSprites = default;
    [SerializeField] private GameObject _restartButtonGameobject = default;
    [SerializeField] private GameObject _pauseMenuPanel = default;
    private Animator _pauseAnimator;
    private GameManager _gameManager;

    bool _displayInGameMenuPanel = false;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _restartButtonGameobject.SetActive(false);
         _pauseMenuPanel.SetActive(false);
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _pauseAnimator = _pauseMenuPanel.GetComponent<Animator>();
        _pauseAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        if (_gameManager == null)
        {
            Debug.LogError("GameManager is Null.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        DisplayPauseMenu();
    }

    private void DisplayPauseMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_displayInGameMenuPanel)
            {
                _pauseMenuPanel.SetActive(true);
                _pauseAnimator.SetBool("isPaused",true);
                 Time.timeScale = 0;
                _displayInGameMenuPanel = true;
            }
            else
            {
                Time.timeScale = 1;          
            }
        }
        if (Time.timeScale == 1)
        {
            _pauseMenuPanel.SetActive(false);
            _displayInGameMenuPanel = false;
        }
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }
    
    public void UpdateHighScore(int playerHighscore){
        _highScoreText.text = "HighScore: " + playerHighscore.ToString();
    }

    public void UpdateLife(int playerLife)
    {
        _livesImage.sprite = _livesSprites[playerLife];
        if (playerLife == 0)
        {
            DisplayGameover();
        }
    }

    void DisplayGameover()
    {
        _gameManager.GameIsOver();
        _gameOverText.gameObject.SetActive(true);
        _restartButtonGameobject.SetActive(true);
        StartCoroutine(TextOnAndOff());
    }

    IEnumerator TextOnAndOff()
    {
        while (true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }

}

