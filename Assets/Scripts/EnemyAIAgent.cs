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

    void Start()
    {
        attackScript = GetComponent<AttackScript>();
    }

    public override void OnEpisodeBegin()
    {
        RequestDecision();
        // RequestAction();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Observe the attack element
        NUM_ELEMENT = (int)AttackScript.magicElement.LastElement;
        sensor.AddObservation(NUM_ELEMENT);
        
    }

    public void AgentAttack(ActionSegment<int> act)
    {
        FighterStats fighterStats = GetComponent<FighterStats>();
        FighterAction fighterAction = GetComponent<FighterAction>();
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;
        var physicalAttack = act[0];
        var fireAttack = act[1];
        // var iceAttack = act[2];
        // var lightningAttack = act[3];
        // var windAttack = act[4];
        // var heal = act[5];
        // var item = act[6];

        if(physicalAttack == 1)
        {
            attackScript = GameObject.Find("EMeleePrefab").GetComponent<AttackScript>();
            fighterAction.SelectAttack("melee");
            Debug.Log("Melee attack");
        } else if (fireAttack == 1)
        {
            attackScript = GameObject.Find("ERangePrefab").GetComponent<AttackScript>();
            fighterAction.SelectAttack("range");
            Debug.Log("Range attack");
        } 
        // else if (iceAttack == 1)
        // {
        //     attackScript.element = AttackScript.magicElement.Ice;
        // } else if (lightningAttack == 1)
        // {
        //     attackScript.element = AttackScript.magicElement.Thunder;
        // } else if (windAttack == 1)
        // {
        //     attackScript.element = AttackScript.magicElement.Wind;
        // }

        // If the agent is trying to attack

        if (physicalAttack == 1 || fireAttack == 1)
        {
            if (attackScript.IsBlockingAttack)
            {
                // reward = -attackPower * blockMultiplier;
                reward = -1f;
            } else if (attackScript.IsResistingAttack)
            {
                // reward = -attackPower * resistanceMultiplier;
                reward = -0.5f;
            } else if (attackScript.IsWeakToAttack)
            {
                // reward = attackPower * weaknessMultiplier;
                reward = 1f;

            } else
            {
                // reward = attackPower;
                reward = 0.5f;
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
        // Debug.Log("Action: " + action);

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

        // NextTurn();
        EndEpisode();
    }

    private float Attack()
    {
        // Observe the attack element
        AttackScript attackScript = GetComponent<AttackScript>();
        FighterStats fighterStats = GetComponent<FighterStats>();
        // attackScript.element = (AttackScript.magicElement) Mathf.FloorToInt(0);

        Debug.Log(attackScript.element);
        float reward = 0f;

        // Check if the attack hits the weakness
        if (attackScript.IsWeakToAttack)
        {
            reward = attackPower * weaknessMultiplier;
            // element.TakeDamage(attackPower);
        }
        else
        {
            reward = -attackPower;
        }

        return reward;
    }

    private float Block()
    {
        float reward = 0f;

        // Observe the attack element
        AttackScript attackScript = GetComponent<AttackScript>();
        FighterStats fighterStats = GetComponent<FighterStats>();
        // attackScript.element = (AttackScript.magicElement) Mathf.FloorToInt(0);

        Debug.Log(attackScript.element);

        // Check if the attack is being blocked
        if (attackScript.IsBlockingAttack)
        {
            reward = -attackPower * blockMultiplier;
        }
        else
        {
            reward = -attackPower;
            // element.TakeDamage(attackPower);
        }

        return reward;
    }

    private float Resist()
    {
        float reward = 0f;

        // Observe the attack element
        AttackScript attackScript = GetComponent<AttackScript>();
        FighterStats fighterStats = GetComponent<FighterStats>();
        // attackScript.element = (AttackScript.magicElement) Mathf.FloorToInt(0);

        Debug.Log(attackScript.element);

        // Check if the attack is being resisted
        if (attackScript.IsResistingAttack)
        {
            reward = -attackPower * resistanceMultiplier;
        }
        else
        {
            reward = -attackPower;
            // element.TakeDamage(attackPower);
        }

        return reward;
    }

    // private Element GetRandomElement()
    // {
    //     return elements[Random.Range(0, elements.Length)];
    // }
}

