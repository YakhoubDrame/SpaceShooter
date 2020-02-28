using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _speed = 20.0f;
    [SerializeField]
    private GameObject _explosionPrefab;
    private SpawnManager _spawnManager;
    private AudioManager _audioManager;


    // Update is called once per frame
    void Update()
    {
        //Rotate along the Z axis
        this.transform.Rotate(Vector3.forward * _speed * Time.deltaTime, Space.Self);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.Log("The Spawn_Manager is null");
        }
        _audioManager = GameObject.Find("Audio_Manager").GetComponent<AudioManager>();
        if (_audioManager == null)
        {
            Debug.Log("The audioManager is null");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            Instantiate(_explosionPrefab, this.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            if(_spawnManager != null)
            {
                _spawnManager.StartSpawning();
            }
            if (_audioManager != null)
            {
                _audioManager.PlayExplosionSound();
            }  
            Destroy(this.gameObject, 0.25f);
        }
    }
    //Check for laser collision of type trigger
    //Instantiate explosion at the position of the asteroid
    //Clean up the explosion, destroy it after 3 seconds
}
