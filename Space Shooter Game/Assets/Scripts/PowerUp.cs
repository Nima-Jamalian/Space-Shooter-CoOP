using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private float _speed = 3;
    [SerializeField] private int _powerupID = default;
    [SerializeField] private AudioClip _powerUpAudioClip = default;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -5.7f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player player = collision.transform.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(_powerUpAudioClip,transform.position);
            if(player != null)
            {
                switch (_powerupID)
                {
                    case 0:
                        player.TripleShotPowerupActive();
                        break;
                    case 1:
                        player.SpeedPowerupActive();
                        break;
                    case 2:
                        player.ShieldPowerUpActive();
                        break;
                    default:
                        print("Default Value");
                        break;
                }
            }
            Destroy(this.gameObject);
        }
    }

}
