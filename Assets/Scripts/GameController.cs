﻿using System.Collections;
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
    private List<FighterStats> fighterStats;

    private GameObject battleMenu;

    public Text battleText;

    private void Awake()
    {
        battleMenu = GameObject.Find("ActionMenu");
    }
    void Start()
    {
        fighterStats = new List<FighterStats>();
        GameObject hero = GameObject.FindGameObjectWithTag("Hero");
        FighterStats currentFighterStats = hero.GetComponent<FighterStats>();
        currentFighterStats.CalculateNextTurn(0);
        fighterStats.Add(currentFighterStats);

        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
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
                battleMenu.SetActive(true);
                Debug.Log("Hero's turn");
            }
            else
            {
                battleMenu.SetActive(false);
                Debug.Log("Enemy's turn");

                // Scripted AI
                // string attackType = Random.Range(0, 2) == 1 ? "melee" : "range";
                // currentUnit.GetComponent<FighterAction>().SelectAttack(attackType);

                // ML-Agents AI
                // float[] actions = new float[2];
                // float[] actions = new float[] { 0, 1, 0, 0, 0, 0, 0 };
                // ActionBuffers actionBuffers = ActionBuffers.FromDiscreteActions(actions);
                // currentUnit.GetComponent<EnemyAIAgent>().OnActionReceived(actionBuffers);
                currentUnit.GetComponent<EnemyAIAgent>().RequestDecision();
            
            }
        }
        else
        {
            NextTurn();
        }
    }
}
