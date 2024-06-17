using System;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class EnemyAIAgent : Agent
{
    private float reward = 0f;

    public AttackScript attackScript;
    public FighterStats heroFighterStats;
    public FighterStats enemyFighterStats;
    public FighterAction fighterAction;
    public GameController gameController;
    public BehaviorParameters behaviorParameters;
    private GameMode gameMode;

    void Start()
    {
        if (behaviorParameters == null) behaviorParameters = GetComponent<BehaviorParameters>();
        if (heroFighterStats == null) heroFighterStats = GameObject.FindWithTag("Hero").GetComponent<FighterStats>();
        if (enemyFighterStats == null) enemyFighterStats = gameObject.GetComponent<FighterStats>();
        if (fighterAction == null) fighterAction = GetComponent<FighterAction>();
        if (gameController == null) gameController = GameObject.Find("GameControllerObject").GetComponent<GameController>();
        gameMode = GameObject.Find("GameModeManager") ? GameObject.Find("GameModeManager").GetComponent<GameMode>() : null;
    }

    private void HandleAction(ActionSegment<int> act)
    {
        var physicalAttack = act[0];
        var guard = act[1];

        if (gameMode != null)
        {
            if (gameMode.isUsingElement)
            {
                var iceAttack = act[2];
                var earthAttack = act[3];
                var windAttack = act[4];
                var ramuanMujarab = act[5];
                var ramuanPemula = act[6];

                if (physicalAttack == 1)
                {
                    ExecuteAction("melee", "EMeleePrefab");
                }
                else if (guard == 1)
                {
                    ExecuteAction("guard", null);
                }
                else if (iceAttack == 1)
                {
                    ExecuteAction("iceStorm", "EIceStormPrefab");
                }
                else if (earthAttack == 1)
                {
                    ExecuteAction("stomp", "EStompPrefab");
                }
                else if (windAttack == 1)
                {
                    ExecuteAction("windSlash", "EWindSlashPrefab");
                }
                else if (ramuanMujarab == 1)
                {
                    ExecuteAction("ramuanMujarab", null);
                }
                else if (ramuanPemula == 1)
                {
                    ExecuteAction("ramuanPemula", null);
                }
            }
            else
            {
                var hempasanRatu = act[2];
                if (physicalAttack == 1)
                {
                    ExecuteAction("melee", "EMeleePrefab");
                    reward = -0.1f;
                }
                else if (guard == 1)
                {
                    ExecuteAction("guard", null);
                    reward = 0.05f;
                }
                else if (hempasanRatu == 1)
                {
                    ExecuteAction("hempasanRatu", "EHempasanRatuPrefab");
                    reward = 0.2f;
                }
            }
        }
        else
        {
            var iceAttack = act[2];
            var earthAttack = act[3];
            var windAttack = act[4];
            var ramuanMujarab = act[5];
            var ramuanPemula = act[6];

            if (physicalAttack == 1)
            {
                ExecuteAction("melee", "EMeleePrefab");
            }
            else if (guard == 1)
            {
                ExecuteAction("guard", null);
            }
            else if (iceAttack == 1)
            {
                ExecuteAction("iceStorm", "EIceStormPrefab");
            }
            else if (earthAttack == 1)
            {
                ExecuteAction("stomp", "EStompPrefab");
            }
            else if (windAttack == 1)
            {
                ExecuteAction("windSlash", "EWindSlashPrefab");
            }
            else if (ramuanMujarab == 1)
            {
                ExecuteAction("ramuanMujarab", null);
            }
            else if (ramuanPemula == 1)
            {
                ExecuteAction("ramuanPemula", null);
            }

            // var hempasanRatu = act[2];
            // if (physicalAttack == 1)
            // {
            //     ExecuteAction("melee", "EMeleePrefab");
            //     reward = -0.1f;
            // }
            // else if (guard == 1)
            // {
            //     ExecuteAction("guard", null);
            //     reward = 0.05f;
            // }
            // else if (hempasanRatu == 1)
            // {
            //     ExecuteAction("hempasanRatu", "EHempasanRatuPrefab");
            //     reward = 0.2f;
            // }
        }
    }

    private void ExecuteAction(string actionType, string prefabName)
    {
        attackScript = prefabName != null ? GameObject.Find(prefabName).GetComponent<AttackScript>() : null;
        fighterAction.SelectAction(actionType);
        Debug.Log($"Agent {actionType} executed");
    }
    private void EvaluateImmediateReward()
    {
        if (attackScript != null)
        {
            if (attackScript.IsBlockingAttack)
            {
                reward = -0.1f;
            }
            else if (attackScript.IsResistingAttack)
            {
                reward = -0.05f;
            }
            else if (attackScript.IsWeakToAttack)
            {
                reward = 0.2f;
            }
            else
            {
                reward = 0.05f;
            }
        }
        else if (fighterAction.currentAction == "ramuanMujarab")
        {
            if (enemyFighterStats.health > 150)
            {
                reward = -0.1f;
            }
            else if (enemyFighterStats.health > 100)
            {
                reward = -0.05f;
            }
            else
            {
                reward = 0.2f;
            }
        }
        else if (fighterAction.currentAction == "ramuanPemula")
        {
            if (enemyFighterStats.health > 200)
            {
                reward = -0.1f;
            }
            else if (enemyFighterStats.health > 150)
            {
                reward = -0.05f;
            }
            else
            {
                reward = 0.2f;
            }
        }
        else
        {
            reward = 0.05f;
        }
    }

    public void EvaluateReward(float winlosereward)
    {
        Debug.Log("Winlose Reward: " + winlosereward);
        AddReward(winlosereward);
    }


    public override void OnActionReceived(ActionBuffers actions)
    {
        if (gameController.state == GameController.BattleState.ENEMYTURN)
        {
            HandleAction(actions.DiscreteActions);
            EvaluateImmediateReward();
            AddReward(reward);
            Debug.Log("Immediate Reward: " + reward);

            // // Only end episode if the battle is won or lost
            // if (gameController.state == GameController.BattleState.WON || gameController.state == GameController.BattleState.LOST)
            // {
            //     Debug.Log("End Episode");
            //     EndEpisode();
            // }
        }

        gameController.state = GameController.BattleState.HEROTURN;
    }


    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(heroFighterStats.health / heroFighterStats.startHealth);
        sensor.AddObservation(enemyFighterStats.health / enemyFighterStats.startHealth);
        sensor.AddObservation(fighterAction.GetCurrentActionType / 10);
    }
}
