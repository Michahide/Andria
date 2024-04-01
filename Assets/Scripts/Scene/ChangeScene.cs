using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] GameObject SettingsPanel;
    [SerializeField] GameObject MenuPanel;
    [SerializeField] GameObject GameModePanel;
    // private static ChangeScene instance;

    // void Awake()
    // {
    //     DontDestroyOnLoad(this);
    //     if (instance == null)
    //     {
    //         instance = this;
    //     }
    //     else
    //     {
    //         Destroy(gameObject);
    //     }
    // }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            SettingsPanel = GameObject.Find("PengaturanPanel");
            MenuPanel = GameObject.Find("MenuUtamaPanel");
            GameModePanel = GameObject.Find("GameModePanel");
            SettingsPanel.SetActive(false);
            MenuPanel.SetActive(true);
            GameModePanel.SetActive(false);
        }
    }
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

    public void GameMode()
    {
        if (GameModePanel.activeInHierarchy)
        {
            GameModePanel.SetActive(false);
            MenuPanel.SetActive(true);
        }
        else if (!GameModePanel.activeInHierarchy)
        {
            GameModePanel.SetActive(true);
            MenuPanel.SetActive(false);
        }
    }

    public void ChangeToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        // if (SceneManager.GetActiveScene().name == "MainMenu")
        // {
        //     SettingsPanel = GameObject.Find("PengaturanPanel");
        //     MenuPanel = GameObject.Find("MenuUtamaPanel");
        //     GameModePanel = GameObject.Find("GameModePanel");

        //     SettingsPanel.SetActive(false);
        //     MenuPanel.SetActive(true);
        //     GameModePanel.SetActive(false);
        // }
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
