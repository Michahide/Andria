using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    public GameObject owner;
    [SerializeField] private string magicName;
    [HideInInspector] public float damage = 0.0f;

    [SerializeField] private string animationName;

    [SerializeField] private bool magicAttack;

    [SerializeField] private float magicCost;
    public enum magicElement { None, Physical, Fire, Ice, Water, Wind, Thunder, Earth, LastElement };
    public magicElement element;
    [SerializeField] private float minAttackMultiplier;

    [SerializeField] private float maxAttackMultiplier;

    [SerializeField] private float minDefenseMultiplier;

    [SerializeField] private float maxDefenseMultiplier;
    private GameMode gameMode;
    private GameObject GameControllerObj;

    private FighterStats attackerStats;
    [HideInInspector] public FighterStats targetStats;

    [HideInInspector] public bool IsBlockingAttack;
    [HideInInspector] public bool IsResistingAttack;
    [HideInInspector] public bool IsWeakToAttack;
    private float GuardMultiplier;

    public void Awake()
    {
        gameMode = GameObject.Find("GameModeManager").GetComponent<GameMode>();
        GameControllerObj = GameObject.Find("GameControllerObject");

        if (!GameMode.instance.isUsingElement)
        {
            element = magicElement.None;
        }
    }

    public void Attack(GameObject victim)
    {
        attackerStats = owner.GetComponent<FighterStats>();
        targetStats = victim.GetComponent<FighterStats>();
        if (attackerStats.magic >= magicCost)
        {
            float multiplier = Random.Range(minAttackMultiplier, maxAttackMultiplier);
            if (targetStats.guard)
            {
                GuardMultiplier = 0.75f;
                targetStats.guard = false;
                Debug.Log("Target is guarding");
            }
            else
            {
                GuardMultiplier = 1.0f;
                Debug.Log("Target is not guarding");
            }

            damage = multiplier * attackerStats.melee * GuardMultiplier;
            if (magicAttack)
            {
                damage = multiplier * attackerStats.magicRange * GuardMultiplier;
            }

            if (targetStats.elementBlock.ToList().Contains(element.ToString()))
            {
                IsBlockingAttack = true;
                damage = 0;
            }
            else if (targetStats.elementResistance.ToList().Contains(element.ToString()))
            {
                IsResistingAttack = true;
                damage = Mathf.CeilToInt(damage / 2);
            }
            else if (targetStats.elementWeakness.ToList().Contains(element.ToString()))
            {
                IsWeakToAttack = true;
                damage = Mathf.CeilToInt(damage * 2);
            }

            float defenseMultiplier = Random.Range(minDefenseMultiplier, maxDefenseMultiplier);
            damage = Mathf.Max(0, damage - (defenseMultiplier * targetStats.defense));
            Debug.Log(damage);
            owner.GetComponent<Animator>().Play(animationName);
            targetStats.ReceiveDamage(Mathf.CeilToInt(damage));
            attackerStats.updateMagicFill(magicCost);
            if (owner.tag == "Hero")
            {
                GameControllerObj.GetComponent<GameController>().battleEnemyText.gameObject.SetActive(true);
                // Debug.Log("Hero's attack!");
                if (IsBlockingAttack)
                {
                    GameControllerObj.GetComponent<GameController>().battleEnemyText.text = "Blok Serangan!";
                }
                else if (IsWeakToAttack)
                {
                    GameControllerObj.GetComponent<GameController>().battleEnemyText.text = "Musuh Lemah!\n" + "-" + Mathf.CeilToInt(damage).ToString();
                }
                else if (IsResistingAttack)
                {
                    GameControllerObj.GetComponent<GameController>().battleEnemyText.text = "Menahan Sebagian Serangan!\n" + "-" + Mathf.CeilToInt(damage).ToString();
                }
                else
                {
                    GameControllerObj.GetComponent<GameController>().battleEnemyText.text = "-" + Mathf.CeilToInt(damage).ToString();
                }
                GameControllerObj.GetComponent<GameController>().StartCoroutine(GameControllerObj.GetComponent<GameController>().HeroAttack());
            }
            else
            {
                GameControllerObj.GetComponent<GameController>().battlePlayerText.gameObject.SetActive(true);
                // Debug.Log("Enemy's attack!");
                if (IsBlockingAttack)
                {
                    GameControllerObj.GetComponent<GameController>().battlePlayerText.text = "Blok Serangan!";
                }
                else if (IsWeakToAttack)
                {
                    GameControllerObj.GetComponent<GameController>().battlePlayerText.text = "Pemain Lemah!\n" + "-" + Mathf.CeilToInt(damage).ToString();
                }
                else if (IsResistingAttack)
                {
                    GameControllerObj.GetComponent<GameController>().battlePlayerText.text = "Menahan Sebagian Serangan!\n" + "-" + Mathf.CeilToInt(damage).ToString();
                }
                else
                {
                    GameControllerObj.GetComponent<GameController>().battlePlayerText.text = "-" + Mathf.CeilToInt(damage).ToString();
                }
            }
        }
        else
        {
            GameControllerObj.GetComponent<GameController>().battleText.gameObject.SetActive(true);
            GameControllerObj.GetComponent<GameController>().battleText.text = "Tidak cukup magic untuk menyerang!";
            // Invoke("SkipTurnContinueGame", 4);
            if (owner.tag == "Hero")
            {
                GameControllerObj.GetComponent<GameController>().StartCoroutine(GameControllerObj.GetComponent<GameController>().HeroAttack());
            }
            else
            {
                GameControllerObj.GetComponent<GameController>().state = GameController.BattleState.HEROTURN;
            }
        }
    }

    // void SkipTurnContinueGame()
    // {
    //     GameObject.Find("GameControllerObject").GetComponent<GameController>().NextTurn();
    // }
}
