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
    public enum magicElement { Physical, Fire, Ice, Wind, Thunder, LastElement};
    public magicElement element;
    [SerializeField] private float minAttackMultiplier;

    [SerializeField] private float maxAttackMultiplier;

    [SerializeField] private float minDefenseMultiplier;

    [SerializeField] private float maxDefenseMultiplier;

    private FighterStats attackerStats;
    [HideInInspector] public FighterStats targetStats;

    [HideInInspector] public bool IsBlockingAttack;
    [HideInInspector] public bool IsResistingAttack;
    [HideInInspector] public bool IsWeakToAttack;

    public void Attack(GameObject victim)
    {
        attackerStats = owner.GetComponent<FighterStats>();
        targetStats = victim.GetComponent<FighterStats>();
        if (attackerStats.magic >= magicCost)
        {
            float multiplier = Random.Range(minAttackMultiplier, maxAttackMultiplier);

            damage = multiplier * attackerStats.melee;
            if (magicAttack)
            {
                damage = multiplier * attackerStats.magicRange;
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
        }
        else
        {
            Invoke("SkipTurnContinueGame", 2);
        }
    }

    void SkipTurnContinueGame()
    {
        GameObject.Find("GameControllerObject").GetComponent<GameController>().battleText.gameObject.SetActive(true);
        GameObject.Find("GameControllerObject").GetComponent<GameController>().battleText.text = "Not enough magic to perform this action";
        GameObject.Find("GameControllerObject").GetComponent<GameController>().NextTurn();
    }
}
