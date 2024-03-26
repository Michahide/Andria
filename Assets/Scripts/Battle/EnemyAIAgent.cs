using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Sensors.Reflection;
using UnityEngine;

public class EnemyAIAgent : Agent
{
    public float attackPower = 10f;
    public float weaknessMultiplier = 2f;
    public float resistanceMultiplier = 0.5f;
    public float blockMultiplier = 0.2f;

    private int NUM_ELEMENT;
    private float reward = 0f;

    public AttackScript attackScript;
    public FighterStats fighterStats;
    public FighterAction fighterAction;
    public GameController gameController;

    void Start()
    {
        attackScript = GetComponent<AttackScript>();
        fighterStats = GameObject.Find("WizardHero").GetComponent<FighterStats>();
        fighterAction = GetComponent<FighterAction>();
        gameController = GameObject.Find("GameControllerObject").GetComponent<GameController>();
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

        Debug.Log("Physical Attack: " + physicalAttack);

        // var iceAttack = act[2];
        // var lightningAttack = act[3];
        // var windAttack = act[4];
        // var heal = act[5];
        // var item = act[6];

        if (physicalAttack == 1)
        {
            attackScript = GameObject.Find("EMeleePrefab").GetComponent<AttackScript>();
            fighterAction.SelectAttack("melee");
            Debug.Log("Melee attack");
        }
        else if (iceAttack == 1)
        {
            attackScript = GameObject.Find("EIceStormPrefab").GetComponent<AttackScript>();
            fighterAction.SelectAttack("iceStorm");
            Debug.Log("Ice Storm attack");
        }
        else if (earthAttack == 1)
        {
            attackScript = GameObject.Find("EStompPrefab").GetComponent<AttackScript>();
            fighterAction.SelectAttack("stomp");
            Debug.Log("Stomp Attack");
        }
        else if (windAttack == 1)
        {
            attackScript = GameObject.Find("EWindSlashPrefab").GetComponent<AttackScript>();
            fighterAction.SelectAttack("windSlash");
            Debug.Log("Wind Slash Attack");
        }

        // If the agent is trying to attack

        if (physicalAttack == 1 || iceAttack == 1 || earthAttack == 1 || windAttack == 1)
        {
            if (attackScript.IsBlockingAttack)
            {
                // reward = -attackPower * blockMultiplier;
                reward = -1f;
            }
            else if (attackScript.IsResistingAttack)
            {
                // reward = -attackPower * resistanceMultiplier;
                reward = -0.5f;
            }
            else if (attackScript.IsWeakToAttack)
            {
                // reward = attackPower * weaknessMultiplier;
                reward = 1f;

            }
            else
            {
                // reward = attackPower;
                reward = 0.1f;
            }
        }

        // if(heal == 1)
        // {

        // }
    }


    public override void OnActionReceived(ActionBuffers actions)
    {
        // Get the selected action
        // Declare and assign a value to the vectorAction variable
        // float vectorAction = actions.ContinuousActions[0];
        // int action = Mathf.FloorToInt(vectorAction);

        // Calculate the reward based on the action and target element
        // float reward = 0f;
        // switch (action)
        // {
        //     case 0: // Attack
        //         reward = Attack();
        //         break;
        //     case 1: // Block
        //         reward = Block();
        //         break;
        //     case 2: // Resist
        //         reward = Resist();
        //         break;
        // }
        AgentAttack(actions.DiscreteActions);

        // Apply the reward
        AddReward(reward);
        Debug.Log("Reward: " + reward);

        gameController.NextTurn();
        EndEpisode();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Observe the fighter's health
        sensor.AddObservation(fighterStats.health);
        Debug.Log("Health: " + fighterStats.health);

        // Observe the current attack type
        sensor.AddObservation(fighterAction.GetCurrentAttackType);
        Debug.Log("Attack Type: " + fighterAction.GetCurrentAttackType);
    }
}

