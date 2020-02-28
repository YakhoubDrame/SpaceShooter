using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;

    private Player _player = null;
    private Animator _animator = null;

    private AudioManager _audioManager;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.Log("The Player wasn't found hun");
        }
        _animator = this.GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.Log("The Animator wasn't found");
        }

        _audioManager = GameObject.Find("Audio_Manager").GetComponent<AudioManager>();
        if(_audioManager == null)
        {
            Debug.Log("The audioManager is null"); 
        }
    }
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        float randomX = Random.Range(-8f, 8f);
        if(transform.position.y < -7f)
        {
            transform.position = new Vector3(randomX, 7f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hit " + other.transform.name);
        if(other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if(player != null)
            {
                player.Damage();
            }
            //Trigger animation
            if(_animator != null)
            {
                Debug.Log("Animation Set to destroy, let's go");
                _animator.SetTrigger("OnEnemyDeath");
            }
            _speed = 0;

            if (_audioManager != null)
            {
                _audioManager.PlayExplosionSound();
            }            
            Destroy(this.gameObject, 2.8f);
        }

        if (other.tag == "Laser")
        {
            if(_player != null)
            {
                _player.AddScore(10);
            }
            //Add 10 to score
            //Trigger Animation
            if (_animator != null)
            {
                Debug.Log("Animation Set to destroy, let's go");
                _animator.SetTrigger("OnEnemyDeath") ;
            }
            _speed = 0;
            StartCoroutine(DestroyEnemy());
            if (_audioManager != null)
            {
                _audioManager.PlayExplosionSound();
            }
            Destroy(GetComponent<Collider2D>());
            Destroy(other.gameObject);

        }
    }

    private IEnumerator DestroyEnemy()
    {
        yield return new WaitForSeconds(2.8f);
        Destroy(this.gameObject);
    }
}
