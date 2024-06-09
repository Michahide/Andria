using System.Collections;
using UnityEngine;
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
    public bool battleEnded;
    public bool winLoseML;

    void Awake()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.Stop("Menu");
            AudioManager.Instance.Play("Battle");
        }

        // hero = Instantiate(heroPrefab, heroStation);
        currentFighterStats = hero.GetComponent<FighterStats>();

        // enemy = Instantiate(enemyPrefab, enemyStation);
        currentEnemyStats = enemy.GetComponent<FighterStats>();
    }
    void Start()
    {
        Debug.Log("Game Start!");
        battleEnded = false;
        winLoseML = false;
        state = BattleState.START;
        DisableAllPanels();

        loadBasicScene = GetComponent<LoadBasicScene>();
        gameMode = GameObject.Find("GameModeManager") ? GameObject.Find("GameModeManager").GetComponent<GameMode>() : null;

        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        DisableBattleTexts();

        heroHUD.SetHUD(currentFighterStats);
        enemyHUD.SetHUD(currentEnemyStats);

        battleText.gameObject.SetActive(true);
        battleText.text = "Kamu bertemu Ratu Rubah!";
        // yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(0.1f);
        battleText.gameObject.SetActive(false);

        state = BattleState.HEROTURN;

        if (gameMode != null && !gameMode.isUsingElement)
        {
            ClearElementData(currentFighterStats);
            ClearElementData(currentEnemyStats);
        }
        // HeroTurn();
        StartCoroutine(HeroTraining());
    }
    public IEnumerator HeroAttack()
    {
        DisableAllPanels();
        UpdateHUDs();

        bool isDead = currentEnemyStats.GetDead();

        // yield return new WaitForSeconds(2f);
        yield return new WaitForSeconds(0.1f);

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

        // yield return new WaitForSeconds(2f);
        yield return new WaitForSeconds(0.1f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    public IEnumerator EnemyTurn()
    {
        DisableBattleTexts();
        DisableAllPanels();


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
                    currentEnemyStats?.GetComponent<FighterAction>().SelectAction(actionType);
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
                        actionType = "hempasanRatu";
                    }
                    else
                    {
                        actionType = "melee";
                    }
                    currentEnemyStats?.GetComponent<FighterAction>().SelectAction(actionType);
                }
            }
        }
        else
        {
            if (currentEnemyStats != null) currentEnemyStats.GetComponent<EnemyAIAgent>().RequestDecision();
        }
        bool isDead = currentFighterStats.GetDead();

        yield return new WaitForSeconds(0.1f);

        UpdateHUDs();

        // yield return new WaitForSeconds(2f);
        yield return new WaitForSeconds(0.1f);

        if (isDead)
        {
            state = BattleState.LOST;
            // EndBattle();
            TrainingOnly();
            StartCoroutine(HeroTraining());
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
        DisableBattleTexts();

        bool isDead = currentFighterStats.GetDead();
        if (!isDead)
        {
            battleAffinityText.gameObject.SetActive(true);
            battleAffinityText.text = "Giliranmu!";
            DisableAllPanels();
            if (GameMode.instance.isUsingElement)
            {
                ActionMainElementalPanel.SetActive(true);
            }
            else
            {
                ActionMainNonElementalPanel.SetActive(true);
            }
        }
    }

    public IEnumerator HeroTraining()
    {
        DisableBattleTexts();
        DisableAllPanels();

        string actionType = SelectHeroAction();
        currentFighterStats?.GetComponent<FighterAction>().SelectAction(actionType);

        bool isDead = currentEnemyStats.GetDead();

        yield return new WaitForSeconds(0.1f);

        UpdateHUDs();

        // yield return new WaitForSeconds(2f);
        yield return new WaitForSeconds(0.1f);

        if (isDead)
        {
            state = BattleState.WON;
            // EndBattle();

            TrainingOnly();
            StartCoroutine(EnemyTurn());
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    string SelectHeroAction()
    {
        string[] actionsWithElement = { "melee", "guard", "fireball", "chainLightning", "waterSlash", "ramuanMujarab", "ramuanPemula" };
        string[] actionsWithoutElement = { "melee", "guard", "magicBurst" };

        string[] actions = GameMode.instance?.isUsingElement ?? true ? actionsWithElement : actionsWithoutElement;
        int intActionType = Random.Range(0, actions.Length);

        return actions[intActionType];
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
        if (battleEnded) return;
        battleEnded = true;
        
        DisableAllPanels();
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

    
    private void ClearElementData(FighterStats stats)
    {
        stats.elementWeakness = new string[] { };
        stats.elementResistance = new string[] { };
        stats.elementBlock = new string[] { };
    }

    private void DisableAllPanels()
    {
        ActionMainElementalPanel.SetActive(false);
        ActionMainNonElementalPanel.SetActive(false);
        ItemPanel.SetActive(false);
        SkillElementalPanel.SetActive(false);
        SkillNonElementalPanel.SetActive(false);
    }

    private void DisableBattleTexts()
    {
        battleText.gameObject.SetActive(false);
        battleAffinityText.gameObject.SetActive(false);
        battleEnemyText.gameObject.SetActive(false);
        battlePlayerText.gameObject.SetActive(false);
    }

    private void UpdateHUDs()
    {
        heroHUD.SetHP(currentFighterStats.health);
        heroHUD.SetMP(currentFighterStats.magic);
        enemyHUD.SetHP(currentEnemyStats.health);
        enemyHUD.SetMP(currentEnemyStats.magic);
    }


    public void AgentWin()
    {
        if (winLoseML) return;
        Debug.Log("Agent Win");
        enemy.GetComponent<EnemyAIAgent>().EvaluateReward(1.0f);
        enemy.GetComponent<EnemyAIAgent>().EndEpisode();
        winLoseML = true;
    }

    public void AgentLose()
    {
        if (winLoseML) return;
        Debug.Log("Agent Lose");
        enemy.GetComponent<EnemyAIAgent>().EvaluateReward(-1.0f);
        enemy.GetComponent<EnemyAIAgent>().EndEpisode();
        winLoseML = true;
    }

    private void TrainingOnly()
    {
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
    }
}
