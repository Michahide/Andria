using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeButton : MonoBehaviour
{
    [SerializeField]
    private bool physical;
    [SerializeField] GameObject ActionMainPanel;
    [SerializeField] GameObject ItemPanel;
    [SerializeField] GameObject SkillPanel;

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
            hero.GetComponent<FighterAction>().SelectAttack("melee");
        }
        else if (btn.CompareTo("SkillBtn") == 0 || btn.CompareTo("SkillExit") == 0)
        {
            if (SkillPanel.activeInHierarchy)
            {
                SkillPanel.SetActive(false);
                ActionMainPanel.SetActive(true);
            }
            else if (!SkillPanel.activeInHierarchy)
            {
                SkillPanel.SetActive(true);
                ActionMainPanel.SetActive(false);
            }
        }
        else if (btn.CompareTo("ItemBtn") == 0)
        {
            if (ItemPanel.activeInHierarchy)
            {
                ItemPanel.SetActive(false);
                ActionMainPanel.SetActive(true);
            }
            else if (!ItemPanel.activeInHierarchy)
            {
                ItemPanel.SetActive(true);
                ActionMainPanel.SetActive(false);
            }
        } 
        else if (btn.CompareTo("FireballBtn") == 0)
        {
            hero.GetComponent<FighterAction>().SelectAttack("range");
        }
        else if (btn.CompareTo("WaterSlashBtn") == 0)
        {
            hero.GetComponent<FighterAction>().SelectAttack("range");
        }
        else if (btn.CompareTo("ChainLightningBtn") == 0)
        {
            hero.GetComponent<FighterAction>().SelectAttack("range");
        }
    }
}
