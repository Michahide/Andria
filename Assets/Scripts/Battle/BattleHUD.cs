using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHUD : MonoBehaviour
{
    public float health;
    public float magic;
    public float startHealth;
    public float startMagic;
    // Show health bar
    [SerializeField] private GameObject healthFill;

    // Show magic bar
    [SerializeField] private GameObject magicFill;

    // Resize health and magic bar
    private Transform healthTransform;
    private Transform magicTransform;

    private Vector2 healthScale;
    private Vector2 magicScale;
    private float xNewHealthScale;
    private float xNewMagicScale;

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

        healthTransform = healthFill.GetComponent<RectTransform>();
        healthScale = healthFill.transform.localScale;

        magicTransform = magicFill.GetComponent<RectTransform>();
        magicScale = magicFill.transform.localScale;
    }

    public void SetHUD(FighterStats fighterStats)
    {
        startHealth = fighterStats.startHealth;
        startMagic = fighterStats.startMagic;
        xNewHealthScale = healthScale.x;
        xNewMagicScale = magicScale.x;
    }

    public void SetHP(float hp)
    {
        xNewHealthScale = hp > 0 ? healthScale.x * (hp / startHealth) : 0;
        // Debug.Log("SetHP: " + xNewHealthScale);
        healthFill.transform.localScale = new Vector2(xNewHealthScale, healthScale.y);
    }

    public void SetMP(float mp)
    {

        xNewMagicScale = magicScale.x * (mp / startMagic);
        // Debug.Log("SetMP: " + xNewMagicScale);
        magicFill.transform.localScale = new Vector2(xNewMagicScale, magicScale.y);
    }
}
