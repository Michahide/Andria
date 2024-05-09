using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Transactions;
using UnityEngine.SocialPlatforms;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using TMPro;

public class GameController : MonoBehaviour
{
    public enum BattleState { START, HEROTURN, ENEMYTURN, WON, LOST };
    public BattleState state;
    // private List<FighterStats> fighterStats;

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
        // Academy.Instance.AutomaticSteppingEnabled = false;
        // fighterStats = new List<FighterStats>();
        // hero = GameObject.FindGameObjectWithTag("Hero");
        AudioManager.Instance.Stop("Menu");
        AudioManager.Instance.Play("Battle");
        hero = Instantiate(heroPrefab, heroStation);
        currentFighterStats = hero.GetComponent<FighterStats>();

        // currentFighterStats.CalculateNextTurn(0);
        // fighterStats.Add(currentFighterStats);

        // enemy = GameObject.FindGameObjectWithTag("Enemy");
        enemy = Instantiate(enemyPrefab, enemyStation);
        currentEnemyStats = enemy.GetComponent<FighterStats>();
        // currentEnemyStats.CalculateNextTurn(0);
        // fighterStats.Add(currentEnemyStats);

        // fighterStats.Sort();
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

        gameMode = GameObject.Find("GameModeManager").GetComponent<GameMode>();

        // fighterStats = new List<FighterStats>();

        // fighterStats.Sort();
        StartCoroutine(SetupBattle());
        // NextTurn();
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
        yield return new WaitForSeconds(1f);
        battleText.gameObject.SetActive(false);

        state = BattleState.HEROTURN;

        if (gameMode.isUsingElement == false)
        {
            currentFighterStats.elementWeakness = new string[] { };
            currentFighterStats.elementResistance = new string[] { };
            currentFighterStats.elementBlock = new string[] { };
            currentEnemyStats.elementWeakness = new string[] { };
            currentEnemyStats.elementResistance = new string[] { };
            currentEnemyStats.elementBlock = new string[] { };
        }
        HeroTurn();
    }

    public IEnumerator HeroAttack()
    {
        SkillElementalPanel.SetActive(false);
        SkillNonElementalPanel.SetActive(false);
        enemyHUD.SetHP(currentEnemyStats.health);
        heroHUD.SetMP(currentFighterStats.magic);

        bool isDead = currentEnemyStats.GetDead();

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
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

        yield return new WaitForSeconds(2f);

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

        if (GameMode.instance.isUsingMLAgent)
        {
            currentEnemyStats.GetComponent<EnemyAIAgent>().RequestDecision();
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
                currentEnemyStats.GetComponent<FighterAction>().SelectAction(actionType);
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
                currentEnemyStats.GetComponent<FighterAction>().SelectAction(actionType);
            }
        }

        bool isDead = currentFighterStats.GetDead();

        yield return new WaitForSeconds(0.1f);

        heroHUD.SetHP(currentFighterStats.health);
        enemyHUD.SetHP(currentEnemyStats.health);
        enemyHUD.SetMP(currentEnemyStats.magic);

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.HEROTURN;
            HeroTurn();
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

    void MainMenu()
    {
        AudioManager.Instance.Stop("Battle");
        AudioManager.Instance.Play("Menu");
        loadBasicScene.ChangeToScene("MainMenu");
    }

    public void Credit()
    {
        AudioManager.Instance.Stop("Battle");
        AudioManager.Instance.Play("Menu");
        loadBasicScene.ChangeToScene("Credit");
    }

    public void Win()
    {
        AudioManager.Instance.Play("Win");
    }

    public void Lose()
    {
        AudioManager.Instance.Play("Lose");
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
            Invoke("Win", 2);
            Invoke("Credit", 5);
        }
        else if (state == BattleState.LOST)
        {
            battleText.text = "Kamu Kalah.";
            Invoke("Lose", 2);
            Invoke("MainMenu", 5);
        }
    }
}
