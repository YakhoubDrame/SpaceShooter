using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _explosionObject;
    private AudioSource _explosionSound = null;
    [SerializeField]
    private GameObject _powerUpObject;
    private AudioSource _powerUpSound = null;

    // Start is called before the first frame update
    void Start()
    {
        if (_explosionObject == null)
        {
            Debug.Log("The explosion object is null");
        }
        else
        {
            _explosionSound = _explosionObject.GetComponent<AudioSource>();
        }

        if (_powerUpObject == null)
        {
            Debug.Log("The explosion object is null");
        }
        else
        {
            _powerUpSound = _powerUpObject.GetComponent<AudioSource>();
        }
    }


    public void PlayExplosionSound()
    {
        _explosionSound.Play();
    }

    public void PlayPowerUpSound()
    {
        _powerUpSound.Play();
    }
}
