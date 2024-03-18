using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterAction : MonoBehaviour
{
    private GameObject hero;
    private GameObject enemy;

    [SerializeField] private GameObject meleePrefab;

    [SerializeField] private GameObject rangePrefab;

    [SerializeField] private Sprite faceIcon;
    private GameObject gameAudioManager;

    private GameObject currentAttack;

    // public int GetCurrentAttackType()
    // {
    //     if (currentAttack == meleePrefab)
    //     {
    //         return 0;
    //     }
    //     else if (currentAttack == rangePrefab)
    //     {
    //         return 1;
    //     }
    //     return -1;
    // }

    public int GetCurrentAttackType;

    void Awake()
    {
        hero = GameObject.FindGameObjectWithTag("Hero");
        enemy = GameObject.FindGameObjectWithTag("Enemy");
    }
    public void SelectAttack(string btn)
    {
        GameObject victim = hero;
        if (tag == "Hero")
        {
            victim = enemy;
        }
        if (btn.CompareTo("melee") == 0)
        {
            meleePrefab.GetComponent<AttackScript>().Attack(victim);
            AudioManager.Instance.Play("NormalAttack");
            GetCurrentAttackType = 1;

        }
        else if (btn.CompareTo("range") == 0)
        {
            rangePrefab.GetComponent<AttackScript>().Attack(victim);
            AudioManager.Instance.Play("MagicAttack");
            GetCurrentAttackType = 2;
        }
        else if (btn.CompareTo("block") == 0)
        {
            Debug.Log("Block");
            GetCurrentAttackType = 3;
        }
        else if (btn.CompareTo("resist") == 0)
        {
            Debug.Log("Resist");
            GetCurrentAttackType = 4;
        }
        else if (btn.CompareTo("run") == 0)
        {
            Debug.Log("Run");
            GetCurrentAttackType = 5;
        }
    }
}
