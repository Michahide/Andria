using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FighterStats : MonoBehaviour, IComparable
{
    [SerializeField] private Animator animator;

    // Show health bar
    [SerializeField] private GameObject healthFill;

    // Show magic bar
    [SerializeField] private GameObject magicFill;

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

    // Resize health and magic bar
    private Transform healthTransform;
    private Transform magicTransform;

    private Vector2 healthScale;
    private Vector2 magicScale;

    private float xNewHealthScale;
    private float xNewMagicScale;

    private GameObject GameControllerObj;

    void Awake()
    {
        // if (tag == "hero")
        // {
        //     healthFill = GameObject.Find("HeroHealthFill");
        //     magicFill = GameObject.Find("HeroMagicFill");
        // }
        // else
        // {
        //     healthFill = GameObject.Find("EnemyHealthFill");
        //     magicFill = GameObject.Find("EnemyMagicFill");
        // }

        // healthTransform = healthFill.GetComponent<RectTransform>();
        // healthScale = healthFill.transform.localScale;

        // magicTransform = magicFill.GetComponent<RectTransform>();
        // magicScale = magicFill.transform.localScale;

        startHealth = health;
        startMagic = magic;

        GameControllerObj = GameObject.Find("GameControllerObject");
    }

    public bool ReceiveDamage(float damage)
    {
        health = health - damage;
        animator.Play("Damage");

        // Set damage text

        if (health <= 0)
        {
            dead = true;
            Destroy(healthFill);
            if (gameObject.CompareTag("Hero"))
            {
                GameControllerObj.GetComponent<GameController>().state = GameController.BattleState.LOST;
                GameControllerObj.GetComponent<GameController>().EndBattle();
            }
            else
            {
                GameControllerObj.GetComponent<GameController>().state = GameController.BattleState.WON;
                GameControllerObj.GetComponent<GameController>().EndBattle();
            }
            Destroy(gameObject);
            return true;
        }
        // else if (health > 0 && damage > 0)
        // {
        //     xNewHealthScale = healthScale.x * (health / startHealth);
        //     healthFill.transform.localScale = new Vector2(xNewHealthScale, healthScale.y);
        // }
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
        // Invoke("ContinueGame", 2);
    }

    public void Heal(float heal)
    {
        health += heal;
        if (health > startHealth)
        {
            health = startHealth;
        }
        xNewHealthScale = healthScale.x * (health / startHealth);
        healthFill.transform.localScale = new Vector2(xNewHealthScale, healthScale.y);
    }

    public void updateMagicFill(float cost)
    {
        if (cost > 0)
        {
            magic = magic - cost;
            // xNewMagicScale = magicScale.x * (magic / startMagic);
            // magicFill.transform.localScale = new Vector2(xNewMagicScale, magicScale.y);
        }
    }

    public bool GetDead()
    {
        return dead;
    }

    // void ContinueGame()
    // {
    //     GameObject.Find("GameControllerObject").GetComponent<GameController>().NextTurn();
    // }
    public void CalculateNextTurn(int currentTurn)
    {
        nextActTurn = currentTurn + Mathf.CeilToInt(100f / speed);
    }

    public int CompareTo(object otherStats)
    {
        int nex = nextActTurn.CompareTo(((FighterStats)otherStats).nextActTurn);
        return nex;
    }

}


