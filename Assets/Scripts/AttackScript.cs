using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    public GameObject owner;

    [SerializeField] private string animationName;

    [SerializeField] private bool magicAttack;

    [SerializeField] private float magicCost;
    [SerializeField] private string magicElement;

    [SerializeField] private float minAttackMultiplier;

    [SerializeField] private float maxAttackMultiplier;

    [SerializeField] private float minDefenseMultiplier;

    [SerializeField] private float maxDefenseMultiplier;

    private FighterStats attackerStats;
    private FighterStats targetStats;
    private float damage = 0.0f;

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

            if(targetStats.elementBlock.Contains(magicElement))
            {
                targetStats.IsBlockingAttack = true;
            }
            else if(targetStats.elementResistance.Contains(magicElement))
            {
                targetStats.IsResistingAttack = true;
            }
            else if(targetStats.elementWeakness.Contains(magicElement))
            {
                targetStats.IsWeakToAttack = true;
            }

            if(targetStats.IsBlockingAttack)
            {
                damage = 0;
            }
            else if(targetStats.IsResistingAttack)
            {
                damage = Mathf.CeilToInt(damage / 2);
            }
            else if(targetStats.IsWeakToAttack)
            {
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
        GameObject.Find("GameControllerObject").GetComponent<GameController>().NextTurn();
    }
}
