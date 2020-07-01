using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public void RestartLevel()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        ChangeScene(sceneName);
    }

    public void ExitToMenu(string menuScene)
    {
        ChangeScene(menuScene);
    }

    public void QuitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
