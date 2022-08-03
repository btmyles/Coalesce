using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreKeeper : MonoBehaviour
{
    private int score = 0;
    public SaveLoad saveLoad;
    public TextMeshProUGUI scoreTextBox;
    public TextMeshProUGUI highScore;

    public Lose gameOverUI;

    public void Start()
    {
        saveLoad = Instantiate(saveLoad);
        Data.score = 0;
        scoreTextBox.text = "Score: " + Data.score;
        highScore.text = "High Score: "+ PlayerPrefs.GetInt("HighScore", 0);

        if (PlayerPrefs.GetInt("load") == 1)
        {
            LoadGame();
        }
    }

    // TODO: Stop accesing PlayerPrefs each frame
    public void Update()
    {
        if (score != Data.score)
        {
            score = Data.score;
            scoreTextBox.text = "Score: " + score;

            if (score > PlayerPrefs.GetInt("HighScore", 0))
            {
                PlayerPrefs.SetInt("HighScore", score);
                highScore.text = "High Score: "+score;
            }
        }
    }

    public void AddScore(int addition)
    {
        score += addition;
        scoreTextBox.text = "Score: " + score;
    }

    public void SaveGame()
    {
        saveLoad.SaveGame();
    }
    public void LoadGame()
    {
        // Load scene then load game
        saveLoad.LoadGame();
    }

    public void LoseGame()
    {
        Debug.Log("Game over");
        saveLoad.DeleteSave();
        gameOverUI.Setup();
    }

    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Level Loaded. Resetting static variables");

        Data.score = 0;
        Data.maxLevel = 2;

        Circle.grid = new Circle[Data.width, Data.height];
        Circle.allCircles = new List<Circle>();

    }
}
