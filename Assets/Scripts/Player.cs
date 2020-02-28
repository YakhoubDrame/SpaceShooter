using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject _tripleLaserPrefab = null;

    [SerializeField]
    private GameObject _laserPrefab = null;

    [SerializeField]
    private GameObject _shield;

    [SerializeField]
    private GameObject _rightEngine, _leftEngine;

    [SerializeField]
    private GameObject _laserShot = null;
    private AudioSource _laserShotSound = null;

    private UIManager _uiManager = null;
    private SpawnManager _spawnManager;

    [SerializeField]
    private float _speed = 5.0f;
    private float _speedMultiplier = 2f;

    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;

    [SerializeField]
    private int _lives = 3;
    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;

    [SerializeField]
    private float _tripleShotDelay = 5.0f;
    private IEnumerator coroutine;

    //Variable to store the audio clip

    [SerializeField]
    private int _score = 0;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.Log("The SpawnManager is null");
        }
        if (_shield == null)
        {
            Debug.Log("The Shield is null");
        }
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.Log("The UI Manager is null");
        }
        if (_rightEngine == null)
        {
            Debug.Log("The right engine is null");
        }
        if (_leftEngine == null)
        {
            Debug.Log("The left engine is null");
        }
        if (_laserShot == null)
        {
            Debug.Log("The laser shot sound is null");
        }
        else
        {
            _laserShotSound = _laserShot.GetComponent<AudioSource>();    
        }
        coroutine = TripleShotPowerDownRoutine(_tripleShotDelay);
    }

    void Update()
    {
        CalculateMovement();

        #if UNITY_ANDROID
        if ((Input.GetKeyDown(KeyCode.Space) || CrossPlatformInputManager.GetButtonDown("Fire")) && Time.time > _canFire)
        {
            FireLaser();
        }
        #else
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && Time.time > _canFire)
        {
            FireLaser();
        }
        #endif

    }

    void CalculateMovement()
    {
        float horizontalInput = CrossPlatformInputManager.GetAxis("Horizontal");// Input.GetAxis("Horizontal");
        float verticalInput = CrossPlatformInputManager.GetAxis("Vertical");//Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0));


        if (transform.position.x > 11)
        {
            transform.position = new Vector3(-11, transform.position.y, 0);

        }
        else if (transform.position.x < -11)
        {
            transform.position = new Vector3(11, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_isTripleShotActive)
        {
            Instantiate(_tripleLaserPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }

        if(_laserShotSound != null)
        {
            _laserShotSound.Play();
        }
        //Play the laser audio clip
    }

    public void Damage()
    {
        if(_isShieldActive == true)
        {
            _isShieldActive = false;
            if(_shield != null)
            {
                _shield.SetActive(false);
            }
            return;
        }

        _lives--;
        this.UpdateEngines();
        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            if (_spawnManager)
            {
                _spawnManager.OnPlayerDeath();
            }
            Destroy(this.gameObject);
        }
    }
    public void UpdateEngines()
    {
        if(_lives == 2 && _rightEngine != null)
        {
            _rightEngine.SetActive(true);
            
        }else if(_lives == 1 && _leftEngine != null)
        {
            _leftEngine.SetActive(true);
        }
    }
    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine(5.0f));
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostDownRoutine(5.0f));
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        if(_shield != null)
        {
            _shield.SetActive(true);
        }
    }

    public void AddScore(int points)
    {
        _score += points;
        if (_uiManager != null)
        {
            _uiManager.UpdateScore(_score);
        }else
        {
            Debug.Log("Couldn't find the manager bish");    
        }
    }

    private IEnumerator TripleShotPowerDownRoutine(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _isTripleShotActive = false;
    }

    private IEnumerator SpeedBoostDownRoutine(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }
}
