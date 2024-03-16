using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] GameObject SettingsPanel;
    [SerializeField] GameObject MenuPanel;
    public void Options()
    {
        if (SettingsPanel.activeInHierarchy)
        {
            SettingsPanel.SetActive(false);
            MenuPanel.SetActive(true);
        }
        else if (!SettingsPanel.activeInHierarchy)
        {
            SettingsPanel.SetActive(true);
            MenuPanel.SetActive(false);
        }
    }
    public void ChangeToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadPreviousScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void LoadSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void AfterBattleScene()
    {
        GameObject[] remainingEnemyUnits = GameObject.FindGameObjectsWithTag("Enemy");
        if (remainingEnemyUnits.Length == 0)
        {
            SceneManager.LoadScene("Credit");
        }

        GameObject[] remainingPlayerUnits = GameObject.FindGameObjectsWithTag("Hero");
        if (remainingPlayerUnits.Length == 0)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void NewGame()
    {
        SceneManager.LoadScene("BattleScene");
    }
}
