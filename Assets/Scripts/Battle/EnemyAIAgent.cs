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
        // attackScript = GetComponent<AttackScript>();
        behaviorParameters = GetComponent<BehaviorParameters>();
        // fighterStats = GameObject.Find("WizardHero").GetComponent<FighterStats>();
        heroFighterStats = GameObject.FindWithTag("Hero").GetComponent<FighterStats>();
        enemyFighterStats = GameObject.FindWithTag("Hero").GetComponent<FighterStats>();
        fighterAction = GetComponent<FighterAction>();
        gameController = GameObject.Find("GameControllerObject").GetComponent<GameController>();

        // if (attackScript == null)
        // {
        //     behaviorParameters.BrainParameters.VectorObservationSize = 2;
        //     behaviorParameters.BrainParameters.NumStackedVectorObservations = 3;
        // }
    }

    // public void CollectObservations(VectorSensor sensor)
    // {
    //     FighterStats fighterStats = GetComponent<FighterStats>();

    //     // Add the physicalAttack and fireAttack values to the sensor
    //     sensor.AddObservation(fighterStats.PhysicalAttack);
    //     sensor.AddObservation(fighterStats.FireAttack);
    // }
    public void AgentAttack(ActionSegment<int> act)
    {
        var physicalAttack = act[0];
        var iceAttack = act[1];
        var earthAttack = act[2];
        var windAttack = act[3];
        var guard = act[4];

        if (physicalAttack == 1)
        {
            attackScript = GameObject.Find("EMeleePrefab").GetComponent<AttackScript>();
            fighterAction.SelectAttack("melee");
            Debug.Log("Agent Melee attack");
        }
        else if (iceAttack == 1)
        {
            attackScript = GameObject.Find("EIceStormPrefab").GetComponent<AttackScript>();
            fighterAction.SelectAttack("iceStorm");
            Debug.Log("Agent Ice Storm attack");
        }
        else if (earthAttack == 1)
        {
            attackScript = GameObject.Find("EStompPrefab").GetComponent<AttackScript>();
            fighterAction.SelectAttack("stomp");
            Debug.Log("Agent Stomp Attack");
        }
        else if (windAttack == 1)
        {
            attackScript = GameObject.Find("EWindSlashPrefab").GetComponent<AttackScript>();
            fighterAction.SelectAttack("windSlash");
            Debug.Log("Agent Wind Slash Attack");
        }
        else if (guard == 1)
        {
            fighterAction.SelectAttack("guard");
            Debug.Log("Agent Guard");
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
    }


    public override void OnActionReceived(ActionBuffers actions)
    {
        if (gameController.state == GameController.BattleState.ENEMYTURN)
        {
            AgentAttack(actions.DiscreteActions);

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
        sensor.AddObservation(heroFighterStats.health/heroFighterStats.startHealth);
        // if (attackScript != null)
        // {
        //     sensor.AddOneHotObservation((int)attackScript.element, attackScript.NUM_MAGIC_ELEMENT);
        // }
        // Observe the current attack type
        sensor.AddObservation(fighterAction.GetCurrentAttackType/7);

        // Observe the element used by Enemy
        // Debug.Log("Num Magic Element: " + attackScript.NUM_MAGIC_ELEMENT);

    }
}

