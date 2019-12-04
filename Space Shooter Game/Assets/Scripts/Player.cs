using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{

    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private float _speedMultiplier = 2;
    [SerializeField] private float _fireRate = 0.5f;
    private float _canFire = -1;
    [SerializeField] private int _playerLife = 3;
    private int _score;
    private int _highScore;

    [SerializeField] private GameObject _laserPrefab = default;
    [SerializeField] private GameObject _tripleShootPrefab = default;
    [SerializeField] private GameObject _sheildGameobject = default;
    [SerializeField] private GameObject[] _engineFire = default;
    [SerializeField] private Enemy _enemyScript = default;

    private SpawnManager _spawnManager = default;
    private UIManager _uiManager = default;
    private AudioSource _playerAudiSource = default;

    [SerializeField] private AudioClip _laserShootAudioClip = default;

    private bool _isTripleShootPowerupActive = false;
    private bool _isSpeedPowerupActive = false;
    private bool _isSheildPowerupActive = false;


    [SerializeField] private float _tripleShootPowerupLifeTime = 5f;
    [SerializeField] private float _speedPowerupLifeTime = 5f;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _playerAudiSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is Null.");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is Null.");
        }

        if (_playerAudiSource == null)
        {
            Debug.LogError("Player AudioSource is Null.");
        }
        else
        {
            _playerAudiSource.clip = _laserShootAudioClip;
        }

        transform.position = new Vector3(0, -3, 0);
        _highScore = PlayerPrefs.GetInt("HighScore",0);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        FireLaser();
        CheckforHighScore();
        GameOverCheck();
    }

    void PlayerMovement()
    {
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
        float horizontalInput = CrossPlatformInputManager.GetAxis("Horizontal");
        float verticalInput = CrossPlatformInputManager.GetAxis("Vertical");

#elif UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);
#endif
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.9f, 0), 0);

        if (transform.position.x >= 11)
        {
            transform.position = new Vector3(-11, transform.position.y, 0);
        }
        else if (transform.position.x <= -11)
        {
            transform.position = new Vector3(11, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
        if (CrossPlatformInputManager.GetButtonDown("Fire"))
        {
            SpawnLaser();
        }
#endif

#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnLaser();
        }
#endif
    }

    private void SpawnLaser()
    {
        if (Time.time > _canFire && Time.timeScale != 0)
        {
            _canFire = Time.time + _fireRate;
            float _offSetPositionY;
            _offSetPositionY = transform.position.y + 1f;
            if (_isTripleShootPowerupActive)
            {
                Instantiate(_tripleShootPrefab, new Vector3(transform.position.x, _offSetPositionY, transform.position.z), Quaternion.identity);
            }
            else
            {
                Instantiate(_laserPrefab, new Vector3(transform.position.x, _offSetPositionY, transform.position.z), Quaternion.identity);
            }
            _playerAudiSource.Play();
        }
    }

    public void PlayerDamage()
    {
        if (_isSheildPowerupActive == true)
        {
            _isSheildPowerupActive = false;
            _sheildGameobject.SetActive(false);
            return;
        }
        _playerLife -= _enemyScript.damage;
        _uiManager.UpdateLife(_playerLife);


        if (_playerLife == 2)
        {
            _engineFire[UniqueRandomInt(0, 2)].SetActive(true);
        }
        else if (_playerLife == 1)
        {
            _engineFire[UniqueRandomInt(0, 2)].SetActive(true);
        }
    }

    List<int> _usedValues = new List<int>();
    public int UniqueRandomInt(int min, int max)
    {
        int _randomNumber = Random.Range(min, max);
        while (_usedValues.Contains(_randomNumber))
        {
            _randomNumber = Random.Range(min, max);
        }
        _usedValues.Add(_randomNumber);
        return _randomNumber;
    }

    void GameOverCheck()
    {
        if (_playerLife == 0)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        } else if (_playerLife <= 0){
            _playerLife = 0;
        }
    }

    public void TripleShotPowerupActive()
    {
        _isTripleShootPowerupActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(_tripleShootPowerupLifeTime);
        _isTripleShootPowerupActive = false;
    }

    public void SpeedPowerupActive()
    {
        _isSpeedPowerupActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedPowerDownRoutine());
    }

    IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(_speedPowerupLifeTime);
        _isSpeedPowerupActive = false;
        _speed /= _speedMultiplier;
    }

    public void ShieldPowerUpActive()
    {
        if (!_isSheildPowerupActive)
        {
            _isSheildPowerupActive = true;
            _sheildGameobject.SetActive(true);
        }
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    private void CheckforHighScore(){
        if(_score > _highScore){
            _highScore = _score;
            PlayerPrefs.SetInt("HighScore",_highScore);
        }
        _uiManager.UpdateHighScore(_highScore);
    }

}
