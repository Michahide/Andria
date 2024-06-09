using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FighterStats : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject GameControllerObj;

    [Header("Stats")]
    public float health;
    public float magic;
    public float melee;
    public float magicRange;
    public float defense;
    public float speed;
    public float experience;

    [Header("Element")]
    // Fire, Ice, Poison, etc.
    public string[] elementResistance;
    public string[] elementWeakness;
    public string[] elementBlock;
    [HideInInspector] public float startHealth;
    [HideInInspector] public float startMagic;

    // Turn Order
    [HideInInspector] public int nextActTurn;

    private bool dead = false;
    public bool guard = false;


    void Awake()
    {
        startHealth = health;
        startMagic = magic;

        if (GameControllerObj == null) GameControllerObj = GameObject.Find("GameControllerObject");
    }

    public bool ReceiveDamage(float damage)
    {
        health = health - damage;
        animator.Play("Damage");

        // Set damage text

        if (health <= 0)
        {
            dead = true;
            if (gameObject.CompareTag("Hero"))
            {
                GameControllerObj.GetComponent<GameController>().state = GameController.BattleState.LOST;

                if (GameMode.instance != null)
                {
                    if (GameMode.instance.isUsingMLAgent)
                    {
                        if (GameControllerObj.GetComponent<GameController>().enemy != null)
                        {
                            GameControllerObj.GetComponent<GameController>().AgentWin();
                        }
                    }
                }
                GameControllerObj.GetComponent<GameController>().AgentWin();

                // GameControllerObj.GetComponent<GameController>().EndBattle();
            }
            else
            {
                GameControllerObj.GetComponent<GameController>().state = GameController.BattleState.WON;

                if (GameMode.instance != null)
                {
                    if (GameMode.instance.isUsingMLAgent)
                    {
                        if (GameControllerObj.GetComponent<GameController>().enemy != null)
                        {
                            GameControllerObj.GetComponent<GameController>().AgentLose();
                        }
                    }
                }

                GameControllerObj.GetComponent<GameController>().AgentLose();
            }
            // Destroy(gameObject);
            return true;
        }
        if (health > 0)
        {
            if (gameObject.CompareTag("Hero"))
            {
                GameControllerObj.GetComponent<GameController>().battlePlayerText.gameObject.SetActive(true);
                GameControllerObj.GetComponent<GameController>().battlePlayerText.text = "-" + damage.ToString();
            }
            else
            {
                GameControllerObj.GetComponent<GameController>().battleEnemyText.gameObject.SetActive(true);
                GameControllerObj.GetComponent<GameController>().battleEnemyText.text = "-" + damage.ToString();
            }
        }
        return false;
    }

    public void Heal(float heal)
    {
        health += heal;
        if (health > startHealth)
        {
            health = startHealth;
        }
    }

    public void updateMagicFill(float cost)
    {
        if (cost > 0)
        {
            magic = magic - cost;
        }
    }

    public bool GetDead()
    {
        return dead;
    }

    public void removeDeadStatus()
    {
        dead = false;
    }

}


