using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField] //0 = Triple Shot, 1 = Speed, 2 = Shield
    private int _powerUpId = -1;
    private AudioManager _audioManager;
    [SerializeField]
    private AudioClip _clip;

    private void Start()
    {
        _audioManager = GameObject.Find("Audio_Manager").GetComponent<AudioManager>();
        if (_audioManager == null)
        {
            Debug.Log("The audioManager is null in Power Up");
        }
    }
    void Update()
    {
        CalculateMovement();
        if (transform.position.y < -7.0f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player _player = other.transform.GetComponent<Player>();

            if (_player != null)
            {
                switch(_powerUpId){
                    case 0:
                        _player.TripleShotActive();
                        break;
                    case 1:
                        _player.SpeedBoostActive();
                        break;
                    case 2:
                        _player.ShieldActive();
                        break;
                    default:
                        Debug.Log("Nothing");
                        break;
                }
            }
            else
            {
                Debug.Log("Player is null");
                
            }
            /* if (_audioManager != null)
             {
                 _audioManager.PlayPowerUpSound();
             }  */
            if (_clip != null)
            {
                AudioSource.PlayClipAtPoint(_clip, transform.position, 3.0f);
            }
            Destroy(this.gameObject);
        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }
}
