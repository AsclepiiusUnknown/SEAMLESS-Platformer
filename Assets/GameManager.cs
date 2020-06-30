using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject gameUiContainer;
    public GameObject pauseContainer;
    public GameObject optionsContainer;

    private bool isPaused = false;
    private float timeScaleKeeper;

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
}
