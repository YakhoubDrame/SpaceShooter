using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    // Create a handle to text private GameObject
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _livesImage;
    [SerializeField]
    private Sprite[] _livesSprites;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    private GameManager _gameManager = null;


    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score : " + 0;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if(_gameManager == null)
        {
            Debug.Log("Game Manager is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void UpdateScore(int score)
    {
        _scoreText.text = "Score : " + score;
    }

    public void UpdateLives(int currentLives)
    {
        _livesImage.sprite = _livesSprites[currentLives];

        if(currentLives == 0)
        {
            _gameManager.GameOver();
            StartCoroutine(FlickrGameOverTextRoutine());
        }
    }
    private IEnumerator FlickrGameOverTextRoutine()
    {
        while (true)
        {
            _gameOverText.gameObject.SetActive(true);
            _restartText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(false);
            _restartText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
