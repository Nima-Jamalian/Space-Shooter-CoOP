using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float _speed = 4.5f;
    public int damage = 1;

    private Player _player;
    private Animator _animation;
    private AudioSource _enemyAudioSource;
    private float _fireRate = 3.0f;
    private float _canFire = -1;

    [SerializeField] GameObject _enemyLaserPrefab = default;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("The Player is Null");
        }
        _animation = GetComponent<Animator>();
        _enemyAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        EnemeyMovement();
        FireLaser();
    }

    private void FireLaser()
    {
        if(Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            for(int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    private void EnemeyMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -5.3)
        {
            transform.position = new Vector3(Random.Range(-9.1f, 9.1f), 6.8f, transform.position.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (_player != null)
            {
                _player.PlayerDamage();
            }
            _animation.SetTrigger("OnEnemyDeath");
            _speed = 0.5f;
            _enemyAudioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2f);
        }
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(10);
            }
            _animation.SetTrigger("OnEnemyDeath");
            _speed = 0.5f;
            _enemyAudioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2f);
        }
    }
}
