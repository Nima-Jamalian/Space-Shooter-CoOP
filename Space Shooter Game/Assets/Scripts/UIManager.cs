using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField] private Text _scoreText = default;
    [SerializeField] private Image _livesImage = default;
    [SerializeField] private Text _gameOverText = default;
    [SerializeField] private Sprite[] _livesSprites = default;
    [SerializeField] private GameObject _restartButtonGameobject = default;

    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _restartButtonGameobject.SetActive(false);
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        if(_gameManager == null)
        {
            Debug.LogError("GameManager is Null.");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
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

