using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lose : MonoBehaviour
{
    public void Setup()
    {
        Debug.Log("Setting up lose screen");
        gameObject.SetActive(true);
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void MenuButton() {
        SceneManager.LoadScene("MenuScene");
    }
}
