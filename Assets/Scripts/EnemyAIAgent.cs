using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class EnemyAIAgent : Agent
{
    public float attackPower = 10f;
    public float weaknessMultiplier = 2f;
    public float resistanceMultiplier = 0.5f;
    public float blockMultiplier = 0.2f;

    private Element targetElement;
    private Element[] elements;

    private void Start()
    {
        elements = FindObjectsOfType<Element>();
    }

    public override void OnEpisodeBegin()
    {
        // Reset the agent's position and target element
        transform.position = Vector3.zero;
        targetElement = GetRandomElement();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Observe the agent's position and target element
        sensor.AddObservation(transform.position);
        sensor.AddObservation(targetElement.transform.position);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        base.OnActionReceived(actions);
        // Get the selected action
        // Declare and assign a value to the vectorAction variable
        var vectorAction = actions.ContinuousActions;
        int action = Mathf.FloorToInt(vectorAction[0]);

        // Calculate the reward based on the action and target element
        float reward = 0f;
        switch (action)
        {
            case 0: // Attack
                reward = Attack(targetElement);
                break;
            case 1: // Block
                reward = Block(targetElement);
                break;
            case 2: // Resist
                reward = Resist(targetElement);
                break;
        }

        // Apply the reward
        AddReward(reward);
    }

    private float Attack(Element element)
    {
        float reward = 0f;

        // Check if the attack hits the weakness
        if (element.IsWeakToAttack)
        {
            reward = attackPower * weaknessMultiplier;
            element.TakeDamage(attackPower);
        }
        else
        {
            reward = -attackPower;
        }

        return reward;
    }

    private float Block(Element element)
    {
        float reward = 0f;

        // Check if the attack is being blocked
        if (element.IsBlockingAttack)
        {
            reward = -attackPower * blockMultiplier;
        }
        else
        {
            reward = -attackPower;
            element.TakeDamage(attackPower);
        }

        return reward;
    }

    private float Resist(Element element)
    {
        float reward = 0f;

        // Check if the attack is being resisted
        if (element.IsResistingAttack)
        {
            reward = -attackPower * resistanceMultiplier;
        }
        else
        {
            reward = -attackPower;
            element.TakeDamage(attackPower);
        }

        return reward;
    }

    private Element GetRandomElement()
    {
        return elements[Random.Range(0, elements.Length)];
    }
}

