using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject gameUiContainer;
    public GameObject pauseContainer;
    public GameObject optionsContainer;
    public GameObject victoryContainer;


    private bool isPaused = false;
    private float timeScaleKeeper;
    public static bool canMove = true;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (!optionsContainer.activeSelf)
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                timeScaleKeeper = Time.timeScale;
                Time.timeScale = 0;
                gameUiContainer.SetActive(false);
                pauseContainer.SetActive(true);
            }
            else
            {
                Time.timeScale = timeScaleKeeper;
                gameUiContainer.SetActive(true);
                pauseContainer.SetActive(false);
            }
        }
        else
        {
            optionsContainer.SetActive(false);
            pauseContainer.SetActive(true);
        }
    }

    public void CompleteLevel()
    {
        victoryContainer.SetActive(true);
        gameUiContainer.SetActive(false);
        canMove = false;
    }
}
