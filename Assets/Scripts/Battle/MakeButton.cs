using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeButton : MonoBehaviour
{
    [SerializeField] GameObject ActionMainElementalPanel;
    [SerializeField] GameObject ActionMainNonElementalPanel;
    [SerializeField] GameObject ItemPanel;
    [SerializeField] GameObject SkillElementalPanel;
    [SerializeField] GameObject SkillNonElementalPanel;

    private GameObject hero;
    void Start()
    {
        string temp = gameObject.name;
        gameObject.GetComponent<Button>().onClick.AddListener(() => AttachCallback(temp));
        hero = GameObject.FindGameObjectWithTag("Hero");
    }

    private void AttachCallback(string btn)
    {
        if (btn.CompareTo("MeleeBtn") == 0)
        {
            hero.GetComponent<FighterAction>().SelectAction("melee");
        }
        else if (btn.CompareTo("SkillBtn") == 0 || btn.CompareTo("SkillExit") == 0)
        {
            if (SkillElementalPanel.activeInHierarchy)
            {
                SkillElementalPanel.SetActive(false);
                ActionMainElementalPanel.SetActive(true);
            }
            else if (!SkillElementalPanel.activeInHierarchy)
            {
                SkillElementalPanel.SetActive(true);
                ActionMainElementalPanel.SetActive(false);
            }
        }
        else if (btn.CompareTo("SkillNonElementalBtn") == 0 || btn.CompareTo("SkillNonElementalExit") == 0)
        {
            if (SkillNonElementalPanel.activeInHierarchy)
            {
                SkillNonElementalPanel.SetActive(false);
                ActionMainNonElementalPanel.SetActive(true);
            }
            else if (!SkillNonElementalPanel.activeInHierarchy)
            {
                SkillNonElementalPanel.SetActive(true);
                ActionMainNonElementalPanel.SetActive(false);
            }
        }
        else if (btn.CompareTo("ItemBtn") == 0 || btn.CompareTo("ItemExit") == 0)
        {
            if (ItemPanel.activeInHierarchy)
            {
                ItemPanel.SetActive(false);
                ActionMainElementalPanel.SetActive(true);
            }
            else if (!ItemPanel.activeInHierarchy)
            {
                ItemPanel.SetActive(true);
                ActionMainElementalPanel.SetActive(false);
            }
        }
        else if (btn.CompareTo("GuardBtn") == 0)
        {
            hero.GetComponent<FighterAction>().SelectAction("guard");
        }
        else if (btn.CompareTo("FireballBtn") == 0)
        {
            hero.GetComponent<FighterAction>().SelectAction("fireball");
        }
        else if (btn.CompareTo("WaterSlashBtn") == 0)
        {
            hero.GetComponent<FighterAction>().SelectAction("waterSlash");
        }
        else if (btn.CompareTo("ChainLightningBtn") == 0)
        {
            hero.GetComponent<FighterAction>().SelectAction("chainLightning");
        }
        else if (btn.CompareTo("RamuanMujarabBtn") == 0)
        {
            hero.GetComponent<FighterAction>().SelectAction("ramuanMujarab");
        }
        else if (btn.CompareTo("RamuanPemulaBtn") == 0)
        {
            hero.GetComponent<FighterAction>().SelectAction("ramuanPemula");
        }

        // Non Elemental Attack
        else if (btn.CompareTo("MagicBurstBtn") == 0)
        {
            hero.GetComponent<FighterAction>().SelectAction("magicBurst");
        }
    }
}
