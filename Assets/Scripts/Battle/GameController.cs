using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Transactions;
using UnityEngine.SocialPlatforms;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class GameController : MonoBehaviour
{
    public enum BattleState { START, HEROTURN, ENEMYTURN, WON, LOST };
    public BattleState state;
    private List<FighterStats> fighterStats;

    private GameObject battleMenu;

    public Text battleText;

    public ChangeScene changeScene;
    public GameObject hero;
    public GameObject enemy;

    public GameMode gameMode;

    private void Awake()
    {
        battleMenu = GameObject.Find("ActionMenu");
    }
    void Start()
    {
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
        FighterStats currentFighterStats = fighterStats[0];
        fighterStats.Remove(currentFighterStats);
        if (!currentFighterStats.GetDead())
        {
            GameObject currentUnit = currentFighterStats.gameObject;
            currentFighterStats.CalculateNextTurn(currentFighterStats.nextActTurn);
            fighterStats.Add(currentFighterStats);
            fighterStats.Sort();
            if (currentUnit.CompareTag("Hero"))
            {
                state = BattleState.HEROTURN;
                battleMenu.SetActive(true);
                Debug.Log("Hero's turn");
            }
            else
            {
                state = BattleState.ENEMYTURN;
                battleMenu.SetActive(false);
                Debug.Log("Enemy's turn");

                if(gameMode.isUsingMLAgent)
                {
                    Debug.Log("Using ML Agents");
                    currentUnit.GetComponent<EnemyAIAgent>().RequestDecision();
                }
                else
                {
                    Debug.Log("Using Simple AI");
                    string attackType = Random.Range(0, 2) == 1 ? "melee" : "range";
                    currentUnit.GetComponent<FighterAction>().SelectAttack(attackType);
                }

                if(gameMode.isUsingElement)
                {
                    Debug.Log("Using Element");
                    // currentUnit.GetComponent<FighterAction>().SelectElement();
                }
                else
                {
                    Debug.Log("Not Using Element");
                }
            }
        }
        else
        {
            Academy.Instance.AutomaticSteppingEnabled = false;
            Debug.Log("Unit is dead");
            GameObject currentUnit = currentFighterStats.gameObject;
            if (currentUnit.CompareTag("Hero"))
            {
                Debug.Log("Menang");
                state = BattleState.WON;
                EndBattle();
            }
            else
            {
                Debug.Log("Kalah");
                state = BattleState.LOST;
                EndBattle();
            }
        }
    }

    void MainMenu()
    {
        changeScene.ChangeToScene("MainMenu");
    }

    void Credit()
    {
        changeScene.ChangeToScene("Credit");
    }

    void EndBattle()
    {

        if (state == BattleState.WON)
        {
            battleText.gameObject.SetActive(true);
            battleText.text = "You won the battle!";
            Invoke("Credit", 10);
        }
        else if (state == BattleState.LOST)
        {
            battleText.gameObject.SetActive(true);
            battleText.text = "You were defeated.";
            Invoke("MainMenu", 10);
        }
    }
}
