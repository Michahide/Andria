using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Transactions;
using UnityEngine.SocialPlatforms;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public enum BattleState { START, HEROTURN, ENEMYTURN, WON, LOST };
    public BattleState state;

    public TMP_Text battleText;
    public TMP_Text battleAffinityText;
    public TMP_Text battleEnemyText;
    public TMP_Text battlePlayerText;

    public LoadBasicScene loadBasicScene;
    public GameObject hero;
    public GameObject enemy;

    public GameMode gameMode;
    public GameObject heroPrefab;
    public GameObject enemyPrefab;

    public Transform heroStation;
    public Transform enemyStation;

    FighterStats currentFighterStats;
    FighterStats currentEnemyStats;

    [SerializeField] GameObject ActionMainElementalPanel;
    [SerializeField] GameObject ActionMainNonElementalPanel;
    [SerializeField] GameObject ItemPanel;
    [SerializeField] GameObject SkillElementalPanel;
    [SerializeField] GameObject SkillNonElementalPanel;

    public BattleHUD heroHUD;
    public BattleHUD enemyHUD;

    void Awake()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.Stop("Menu");
        if (AudioManager.Instance != null) AudioManager.Instance.Play("Battle");

        // hero = Instantiate(heroPrefab, heroStation);
        currentFighterStats = hero.GetComponent<FighterStats>();

        // enemy = Instantiate(enemyPrefab, enemyStation);
        currentEnemyStats = enemy.GetComponent<FighterStats>();
    }
    void Start()
    {
        state = BattleState.START;
        ActionMainElementalPanel.SetActive(false);
        ActionMainNonElementalPanel.SetActive(false);
        ItemPanel.SetActive(false);
        SkillElementalPanel.SetActive(false);
        SkillNonElementalPanel.SetActive(false);
        loadBasicScene = GetComponent<LoadBasicScene>();

        gameMode = GameObject.Find("GameModeManager") ? GameObject.Find("GameModeManager").GetComponent<GameMode>() : null;

        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        battleText.gameObject.SetActive(false);
        battleAffinityText.gameObject.SetActive(false);
        battleEnemyText.gameObject.SetActive(false);
        battlePlayerText.gameObject.SetActive(false);

        heroHUD.SetHUD(currentFighterStats);
        enemyHUD.SetHUD(currentEnemyStats);

        battleText.gameObject.SetActive(true);
        battleText.text = "Kamu bertemu Ratu Rubah!";
        // yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(0.1f);
        battleText.gameObject.SetActive(false);

        state = BattleState.HEROTURN;

        if (gameMode != null)
        {

            if (gameMode.isUsingElement == false)
            {
                currentFighterStats.elementWeakness = new string[] { };
                currentFighterStats.elementResistance = new string[] { };
                currentFighterStats.elementBlock = new string[] { };
                currentEnemyStats.elementWeakness = new string[] { };
                currentEnemyStats.elementResistance = new string[] { };
                currentEnemyStats.elementBlock = new string[] { };
            }
        }
        // HeroTurn();
        StartCoroutine(HeroTraining());
    }

    public IEnumerator HeroAttack()
    {
        SkillElementalPanel.SetActive(false);
        SkillNonElementalPanel.SetActive(false);
        enemyHUD.SetHP(currentEnemyStats.health);
        heroHUD.SetMP(currentFighterStats.magic);

        bool isDead = currentEnemyStats.GetDead();

        // yield return new WaitForSeconds(2f);
        yield return new WaitForSeconds(0.1f);

        if (isDead)
        {
            state = BattleState.WON;
            // EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    public IEnumerator HeroUseItem()
    {
        ItemPanel.SetActive(false);
        heroHUD.SetHP(currentFighterStats.health);

        // yield return new WaitForSeconds(2f);
        yield return new WaitForSeconds(0.1f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    public IEnumerator EnemyTurn()
    {
        battleText.gameObject.SetActive(false);
        battleAffinityText.gameObject.SetActive(false);
        battleEnemyText.gameObject.SetActive(false);
        battlePlayerText.gameObject.SetActive(false);

        ActionMainElementalPanel.SetActive(false);
        ActionMainNonElementalPanel.SetActive(false);
        ItemPanel.SetActive(false);
        SkillElementalPanel.SetActive(false);
        SkillNonElementalPanel.SetActive(false);


        if (GameMode.instance != null)
        {
            if (GameMode.instance.isUsingMLAgent)
            {
                if (currentEnemyStats != null) currentEnemyStats.GetComponent<EnemyAIAgent>().RequestDecision();
            }
            else
            {
                if (GameMode.instance.isUsingElement)
                {
                    currentEnemyStats.GetComponent<EnemyAIAgent>();
                    int intActionType = Random.Range(0, 6);
                    string actionType;
                    if (intActionType == 1)
                    {
                        actionType = "guard";
                    }
                    else if (intActionType == 2)
                    {
                        actionType = "iceStorm";
                    }
                    else if (intActionType == 3)
                    {
                        actionType = "stomp";
                    }
                    else if (intActionType == 4)
                    {
                        actionType = "windSlash";
                    }
                    else if (intActionType == 5)
                    {
                        actionType = "ramuanMujarab";
                    }
                    else if (intActionType == 6)
                    {
                        actionType = "ramuanPemula";
                    }
                    else
                    {
                        actionType = "melee";
                    }
                    if (currentEnemyStats != null) currentEnemyStats.GetComponent<FighterAction>().SelectAction(actionType);
                }
                else
                {
                    currentEnemyStats.GetComponent<EnemyAIAgent>();
                    int intActionType = Random.Range(0, 2);
                    string actionType;
                    if (intActionType == 1)
                    {
                        actionType = "guard";
                    }
                    else if (intActionType == 2)
                    {
                        actionType = "hempasanRatu";
                    }
                    else
                    {
                        actionType = "melee";
                    }
                    if (currentEnemyStats != null) currentEnemyStats.GetComponent<FighterAction>().SelectAction(actionType);
                }
            }
        }
        else
        {
            if (currentEnemyStats != null) currentEnemyStats.GetComponent<EnemyAIAgent>().RequestDecision();
        }
        bool isDead = currentFighterStats.GetDead();

        yield return new WaitForSeconds(0.1f);

        heroHUD.SetHP(currentFighterStats.health);
        enemyHUD.SetHP(currentEnemyStats.health);
        enemyHUD.SetMP(currentEnemyStats.magic);

        // yield return new WaitForSeconds(2f);
        yield return new WaitForSeconds(0.1f);

        if (isDead)
        {
            state = BattleState.LOST;

            currentFighterStats.health = 300;
            currentFighterStats.magic = 100;
            currentEnemyStats.health = 300;
            currentEnemyStats.magic = 100;
            heroHUD.SetHP(currentFighterStats.health);
            heroHUD.SetMP(currentFighterStats.magic);
            enemyHUD.SetHP(currentEnemyStats.health);
            enemyHUD.SetMP(currentEnemyStats.magic);
            currentEnemyStats.removeDeadStatus();
            currentFighterStats.removeDeadStatus();

            // EndBattle();
        }
        else
        {
            state = BattleState.HEROTURN;
            // HeroTurn();
            StartCoroutine(HeroTraining());
        }
    }

    public void HeroTurn()
    {
        battleText.gameObject.SetActive(false);
        battleEnemyText.gameObject.SetActive(false);
        battlePlayerText.gameObject.SetActive(false);

        bool isDead = currentFighterStats.GetDead();
        if (!isDead)
        {
            battleAffinityText.gameObject.SetActive(true);
            battleAffinityText.text = "Giliranmu!";

            if (GameMode.instance.isUsingElement)
            {
                ActionMainElementalPanel.SetActive(true);
            }
            else
            {
                ActionMainNonElementalPanel.SetActive(true);
            }
            ItemPanel.SetActive(false);
            SkillElementalPanel.SetActive(false);
            SkillNonElementalPanel.SetActive(false);
        }
    }

    public IEnumerator HeroTraining()
    {
        battleText.gameObject.SetActive(false);
        battleAffinityText.gameObject.SetActive(false);
        battleEnemyText.gameObject.SetActive(false);
        battlePlayerText.gameObject.SetActive(false);

        ActionMainElementalPanel.SetActive(false);
        ActionMainNonElementalPanel.SetActive(false);
        ItemPanel.SetActive(false);
        SkillElementalPanel.SetActive(false);
        SkillNonElementalPanel.SetActive(false);

        if (GameMode.instance != null)
        {
            if (GameMode.instance.isUsingElement)
            {
                int intActionType = Random.Range(0, 6);
                string actionType;
                if (intActionType == 1)
                {
                    actionType = "guard";
                }
                else if (intActionType == 2)
                {
                    actionType = "fireball";
                }
                else if (intActionType == 3)
                {
                    actionType = "chainLightning";
                }
                else if (intActionType == 4)
                {
                    actionType = "waterSlash";
                }
                else if (intActionType == 5)
                {
                    actionType = "ramuanMujarab";
                }
                else if (intActionType == 6)
                {
                    actionType = "ramuanPemula";
                }
                else
                {
                    actionType = "melee";
                }
                if (currentFighterStats != null) currentFighterStats.GetComponent<FighterAction>().SelectAction(actionType);
            }
            else
            {
                int intActionType = Random.Range(0, 2);
                string actionType;
                if (intActionType == 1)
                {
                    actionType = "guard";
                }
                else if (intActionType == 2)
                {
                    actionType = "magicBurst";
                }
                else
                {
                    actionType = "melee";
                }
                if (currentFighterStats != null) currentFighterStats.GetComponent<FighterAction>().SelectAction(actionType);
            }
        }
        else
        {
            int intActionType = Random.Range(0, 6);
            string actionType;
            if (intActionType == 1)
            {
                actionType = "guard";
            }
            else if (intActionType == 2)
            {
                actionType = "fireball";
            }
            else if (intActionType == 3)
            {
                actionType = "chainLightning";
            }
            else if (intActionType == 4)
            {
                actionType = "waterSlash";
            }
            else if (intActionType == 5)
            {
                actionType = "ramuanMujarab";
            }
            else if (intActionType == 6)
            {
                actionType = "ramuanPemula";
            }
            else
            {
                actionType = "melee";
            }
            if (currentFighterStats != null) currentFighterStats.GetComponent<FighterAction>().SelectAction(actionType);
        }

        bool isDead = currentEnemyStats.GetDead();

        yield return new WaitForSeconds(0.1f);

        heroHUD.SetHP(currentFighterStats.health);
        heroHUD.SetMP(currentFighterStats.magic);
        enemyHUD.SetHP(currentEnemyStats.health);

        // yield return new WaitForSeconds(2f);
        yield return new WaitForSeconds(0.1f);

        if (isDead)
        {
            state = BattleState.WON;

            currentFighterStats.health = 300;
            currentFighterStats.magic = 100;
            currentEnemyStats.health = 300;
            currentEnemyStats.magic = 100;
            heroHUD.SetHP(currentFighterStats.health);
            heroHUD.SetMP(currentFighterStats.magic);
            enemyHUD.SetHP(currentEnemyStats.health);
            enemyHUD.SetMP(currentEnemyStats.magic);
            currentEnemyStats.removeDeadStatus();
            currentFighterStats.removeDeadStatus();

            // EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    void MainMenu()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.Stop("Battle");
        if (AudioManager.Instance != null) AudioManager.Instance.Play("Menu");
        loadBasicScene.ChangeToScene("MainMenu");
    }

    public void Credit()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.Stop("Battle");
        if (AudioManager.Instance != null) AudioManager.Instance.Play("Menu");
        loadBasicScene.ChangeToScene("Credit");
    }

    public void Win()
    {
        // Debug.Log("Player Win");
        if (AudioManager.Instance != null) AudioManager.Instance.Play("Win");
    }

    public void Lose()
    {
        // Debug.Log("Player Lose");
        if (AudioManager.Instance != null) AudioManager.Instance.Play("Lose");
    }

    public void RestartScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void EndBattle()
    {
        ActionMainElementalPanel.SetActive(false);
        ActionMainNonElementalPanel.SetActive(false);
        ItemPanel.SetActive(false);
        SkillElementalPanel.SetActive(false);
        SkillNonElementalPanel.SetActive(false);
        battleText.gameObject.SetActive(true);
        if (state == BattleState.WON)
        {
            battleText.text = "Kamu Menang!";
            Debug.Log("Player Win!");
            // Invoke("Win", 2);
            // Invoke("Credit", 5);
            // Invoke("RestartScene", 0.1f);
        }
        else if (state == BattleState.LOST)
        {
            battleText.text = "Kamu Kalah.";
            Debug.Log("Player Lose!");
            // Invoke("Lose", 2);
            // Invoke("MainMenu", 5);
            // Invoke("RestartScene", 0.1f);
        }
    }
}
