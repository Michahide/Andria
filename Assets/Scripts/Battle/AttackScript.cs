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

    private string NotEnoughMagic = "Tidak cukup magic untuk menyerang!";
    private string WeakText = "Weak!";
    private string BlockText = "Block!";
    private string ResistText = "Resist!";
    private float GuardMultiplier;

    public void Awake()
    {
        gameMode = GameObject.Find("GameModeManager").GetComponent<GameMode>();
        GameControllerObj = GameObject.Find("GameControllerObject");

        if (!gameMode.isUsingElement)
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
                GameControllerObj.GetComponent<GameController>().battleAffinityText.gameObject.SetActive(true);
                GameControllerObj.GetComponent<GameController>().battleAffinityText.text = BlockText;
                damage = 0;
            }
            else if (targetStats.elementResistance.ToList().Contains(element.ToString()))
            {
                IsResistingAttack = true;
                GameControllerObj.GetComponent<GameController>().battleAffinityText.gameObject.SetActive(true);
                GameControllerObj.GetComponent<GameController>().battleAffinityText.text = ResistText;
                damage = Mathf.CeilToInt(damage / 2);
            }
            else if (targetStats.elementWeakness.ToList().Contains(element.ToString()))
            {
                IsWeakToAttack = true;
                GameControllerObj.GetComponent<GameController>().battleAffinityText.gameObject.SetActive(true);
                GameControllerObj.GetComponent<GameController>().battleAffinityText.text = WeakText;
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
                Debug.Log("Hero's attack!");
                GameControllerObj.GetComponent<GameController>().StartCoroutine(GameControllerObj.GetComponent<GameController>().HeroAttack());
            }
            else
            {
                Debug.Log("Enemy's attack!");
            }
        }
        else
        {
            GameControllerObj.GetComponent<GameController>().battleText.gameObject.SetActive(true);
            GameControllerObj.GetComponent<GameController>().battleText.text = NotEnoughMagic;
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
