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
    private List<FighterStats> fighterStats;

    public TMP_Text battleText;
    public TMP_Text battleAffinityText;
    public TMP_Text battleEnemyText;
    public TMP_Text battlePlayerText;

    public ChangeScene changeScene;
    public GameObject hero;
    public GameObject enemy;

    public GameMode gameMode;

    [SerializeField] GameObject ActionMainPanel;
    [SerializeField] GameObject ItemPanel;
    [SerializeField] GameObject SkillPanel;
    void Start()
    {
        ActionMainPanel.SetActive(true);
        ItemPanel.SetActive(false);
        SkillPanel.SetActive(false);
        changeScene = GetComponent<ChangeScene>();

        gameMode = GameObject.Find("GameModeManager").GetComponent<GameMode>();

        fighterStats = new List<FighterStats>();
        hero = GameObject.FindGameObjectWithTag("Hero");
        FighterStats currentFighterStats = hero.GetComponent<FighterStats>();
        currentFighterStats.CalculateNextTurn(0);
        fighterStats.Add(currentFighterStats);

        enemy = GameObject.FindGameObjectWithTag("Enemy");
        FighterStats currentEnemyStats = enemy.GetComponent<FighterStats>();
        currentEnemyStats.CalculateNextTurn(0);
        fighterStats.Add(currentEnemyStats);

        fighterStats.Sort();

        NextTurn();
    }

    public void NextTurn()
    {
        battleText.gameObject.SetActive(false);
        battleAffinityText.gameObject.SetActive(false);
        battleEnemyText.gameObject.SetActive(false);
        battlePlayerText.gameObject.SetActive(false);
        FighterStats currentFighterStats = fighterStats[0];
        fighterStats.Remove(currentFighterStats);
        if (enemy && hero) // If there is an enemy and a player
        {
            GameObject currentUnit = currentFighterStats.gameObject;
            currentFighterStats.CalculateNextTurn(currentFighterStats.nextActTurn);
            fighterStats.Add(currentFighterStats);
            fighterStats.Sort();
            if (currentUnit.CompareTag("Hero"))
            {
                state = BattleState.HEROTURN;
                ActionMainPanel.SetActive(true);
                ItemPanel.SetActive(false);
                SkillPanel.SetActive(false);
                // Debug.Log("Hero's turn");
            }
            else
            {
                state = BattleState.ENEMYTURN;
                ActionMainPanel.SetActive(false);
                ItemPanel.SetActive(false);
                SkillPanel.SetActive(false);
                // Debug.Log("Enemy's turn");

                if (gameMode.isUsingMLAgent)
                {
                    // Debug.Log("Using ML Agents");
                    currentUnit.GetComponent<EnemyAIAgent>().RequestDecision();
                }
                else
                {
                    // Debug.Log("Using Simple AI");
                    // string attackType = Random.Range(0, 2) == 1 ? "melee" : "range";
                    int intAttackType = Random.Range(0, 4);
                    Debug.Log("intAttackType: " + intAttackType);
                    string attackType;
                    if (intAttackType == 1)
                    {
                        attackType = "stomp";
                    }
                    else if (intAttackType == 2)
                    {
                        attackType = "iceStorm";
                    }
                    else if (intAttackType == 3)
                    {
                        attackType = "windSlash";
                    }
                    else if (intAttackType == 4)
                    {
                        attackType = "guard";
                    }
                    else
                    {
                        attackType = "melee";
                    }
                    currentUnit.GetComponent<FighterAction>().SelectAttack(attackType);
                }

                if (gameMode.isUsingElement)
                {
                    // Debug.Log("Using Element");
                    // currentUnit.GetComponent<FighterAction>().SelectElement();
                }
                else
                {
                    // Debug.Log("Not Using Element");
                }
            }
        }
        else
        {
            NextTurn();
        }
    }

    void MainMenu()
    {
        changeScene.ChangeToScene("MainMenu");
    }

    public void Credit()
    {
        changeScene.ChangeToScene("Credit");
    }

    public void EndBattle()
    {

        if (state == BattleState.WON)
        {
            battleText.gameObject.SetActive(true);
            battleText.text = "You won the battle!";
            Invoke("Credit", 5);
        }
        else if (state == BattleState.LOST)
        {
            battleText.gameObject.SetActive(true);
            battleText.text = "You were defeated.";
            Invoke("MainMenu", 5);
        }
    }
}
