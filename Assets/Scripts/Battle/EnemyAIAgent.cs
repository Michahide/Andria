using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Sensors.Reflection;
using UnityEngine;

public class EnemyAIAgent : Agent
{
    // private int selectedElement;
    // private int NUM_ELEMENT;
    private float reward = 0f;

    public AttackScript attackScript;
    public FighterStats heroFighterStats;
    public FighterStats enemyFighterStats;
    public FighterAction fighterAction;
    public GameController gameController;
    public BehaviorParameters behaviorParameters;

    void Start()
    {
        behaviorParameters = GetComponent<BehaviorParameters>();
        heroFighterStats = GameObject.FindWithTag("Hero").GetComponent<FighterStats>();
        enemyFighterStats = GameObject.FindWithTag("Enemy").GetComponent<FighterStats>();
        fighterAction = GetComponent<FighterAction>();
        gameController = GameObject.Find("GameControllerObject").GetComponent<GameController>();
    }

    public void AgentAction(ActionSegment<int> act)
    {
        var physicalAttack = act[0];
        var guard = act[1];

        // Elemental Skill
        var iceAttack = act[2];
        var earthAttack = act[3];
        var windAttack = act[4];

        // Item
        var ramuanMujarab = act[5];
        var ramuanPemula = act[6];

        // Non elemental skill
        // var hempasanRatu = act[2];

        if (physicalAttack == 1)
        {
            attackScript = GameObject.Find("EMeleePrefab").GetComponent<AttackScript>();
            fighterAction.SelectAction("melee");
            Debug.Log("Agent Melee attack");
        }
        else if (iceAttack == 1)
        {
            attackScript = GameObject.Find("EIceStormPrefab").GetComponent<AttackScript>();
            fighterAction.SelectAction("iceStorm");
            Debug.Log("Agent Ice Storm attack");
        }
        else if (earthAttack == 1)
        {
            attackScript = GameObject.Find("EStompPrefab").GetComponent<AttackScript>();
            fighterAction.SelectAction("stomp");
            Debug.Log("Agent Stomp Attack");
        }
        else if (windAttack == 1)
        {
            attackScript = GameObject.Find("EWindSlashPrefab").GetComponent<AttackScript>();
            fighterAction.SelectAction("windSlash");
            Debug.Log("Agent Wind Slash Attack");
        }
        else if (guard == 1)
        {
            fighterAction.SelectAction("guard");
            Debug.Log("Agent Guard");
        }
        else if (ramuanMujarab == 1)
        {
            fighterAction.SelectAction("ramuanMujarab");
            Debug.Log("Agent use Ramuan Mujarab");
        }
        else if (ramuanPemula == 1)
        {
            fighterAction.SelectAction("ramuanPemula");
            Debug.Log("Agent use Ramuan Pemula");
        }

        // If the agent is trying to attack

        if (physicalAttack == 1 || iceAttack == 1 || earthAttack == 1 || windAttack == 1 || guard == 1)
        {
            if (attackScript.IsBlockingAttack)
            {
                reward = -1f;
            }
            else if (attackScript.IsResistingAttack)
            {
                reward = -0.5f;
            }
            else if (attackScript.IsWeakToAttack)
            {
                reward = 1f;

            }
            else
            {
                reward = 0.1f;
            }
        }
        else if (ramuanMujarab == 1)
        {
            if (enemyFighterStats.health > 75)
            {
                reward = -1f;
            }
            else if (enemyFighterStats.health > 50)
            {
                reward = -0.5f;
            }
            else
            {
                reward = 1f;
            }
        }
        else if (ramuanPemula == 1)
        {
            if (enemyFighterStats.health > 90)
            {
                reward = -1f;
            }
            else if (enemyFighterStats.health > 75)
            {
                reward = -0.5f;
            }
            else
            {
                reward = 1f;
            }
        }
        // else if (hempasanRatu == 1)
        // {
        //     reward = 1f;
        // }
    }


    public override void OnActionReceived(ActionBuffers actions)
    {
        if (gameController.state == GameController.BattleState.ENEMYTURN)
        {
            AgentAction(actions.DiscreteActions);

            // Apply the reward
            AddReward(reward);
            Debug.Log("Reward: " + reward);
        }

        gameController.state = GameController.BattleState.HEROTURN;
        EndEpisode();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Observe the target's health
        sensor.AddObservation(heroFighterStats.health / heroFighterStats.startHealth);

        // Observe owner (enemy)'s health
        sensor.AddObservation(enemyFighterStats.health / enemyFighterStats.startHealth);

        // Observe the current action type
        sensor.AddObservation(fighterAction.GetCurrentActionType / 10);

        // Observe the element used by Enemy
        // Debug.Log("Num Magic Element: " + attackScript.NUM_MAGIC_ELEMENT);

    }
}

