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
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private GameObject waterSlashPrefab;
    [SerializeField] private GameObject chainLightningPrefab;
    [SerializeField] private GameObject stompPrefab;
    [SerializeField] private GameObject iceStormPrefab;
    [SerializeField] private GameObject windSlashPrefab;


    [SerializeField] private Sprite faceIcon;
    private GameObject gameAudioManager;

    private GameObject currentAttack;
    private GameObject GameControllerObj;
    private string AttackText = "Serangan Normal!";
    private string FireballText = "Bola Api!";
    private string WaterSlashText = "Tebasan Air!";
    private string ChainLightningText = "Rambatan Petir!";
    private string StompText = "Hentakan!";
    private string WindSlashText = "Tebasan Angin!";
    private string IceStormText = "Badai Es!";
    private string GuardText = "Guard!";
    private GameObject victim;
    private GameObject currentTurn;

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

    void Start()
    {
        hero = GameObject.FindGameObjectWithTag("Hero");
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        // hero = GameControllerObj.GetComponent<GameController>().hero;
        // enemy = GameControllerObj.GetComponent<GameController>().enemy;
        GameControllerObj = GameObject.Find("GameControllerObject");
    }
    public void SelectAttack(string btn)
    {
        Debug.Log("FighterAction Hero: " + hero);
        Debug.Log("FighterAction Enemy: " + enemy);
        Debug.Log("btn: " + btn);
        currentTurn = enemy;
        victim = hero;

        if (tag == "Hero")
        {
            currentTurn = hero;
            victim = enemy;
        }
        Debug.Log("Victim: " + victim);
        if (btn.CompareTo("melee") == 0)
        {
            meleePrefab.GetComponent<AttackScript>().Attack(victim);
            AudioManager.Instance.Play("NormalAttack");
            GetCurrentAttackType = 1;
            SkillText(AttackText);

        }
        else if (btn.CompareTo("range") == 0)
        {
            rangePrefab.GetComponent<AttackScript>().Attack(victim);
            AudioManager.Instance.Play("MagicAttack");
            SkillText(AttackText);
        }
        else if (btn.CompareTo("fireball") == 0)
        {
            fireballPrefab.GetComponent<AttackScript>().Attack(victim);
            AudioManager.Instance.Play("MagicAttack");
            GetCurrentAttackType = 2;
            SkillText(FireballText);
        }
        else if (btn.CompareTo("waterSlash") == 0)
        {
            waterSlashPrefab.GetComponent<AttackScript>().Attack(victim);
            AudioManager.Instance.Play("MagicAttack");
            GetCurrentAttackType = 3;
            SkillText(WaterSlashText);
        }
        else if (btn.CompareTo("chainLightning") == 0)
        {
            chainLightningPrefab.GetComponent<AttackScript>().Attack(victim);
            AudioManager.Instance.Play("MagicAttack");
            GetCurrentAttackType = 4;
            SkillText(ChainLightningText);
        }
        else if (btn.CompareTo("stomp") == 0)
        {
            stompPrefab.GetComponent<AttackScript>().Attack(victim);
            AudioManager.Instance.Play("MagicAttack");
            GetCurrentAttackType = 5;
            SkillText(StompText);
        }
        else if (btn.CompareTo("iceStorm") == 0)
        {
            iceStormPrefab.GetComponent<AttackScript>().Attack(victim);
            AudioManager.Instance.Play("MagicAttack");
            GetCurrentAttackType = 6;
            SkillText(IceStormText);
        }
        else if (btn.CompareTo("windSlash") == 0)
        {
            windSlashPrefab.GetComponent<AttackScript>().Attack(victim);
            AudioManager.Instance.Play("MagicAttack");
            GetCurrentAttackType = 7;
            SkillText(WindSlashText);
        }
        else if (btn.CompareTo("guard") == 0)
        {
            SkillText(GuardText);
            if (tag == "Hero")
            {
                hero.GetComponent<FighterStats>().guard = true;
                GameControllerObj.GetComponent<GameController>().state = GameController.BattleState.ENEMYTURN;
                GameControllerObj.GetComponent<GameController>().StartCoroutine(GameControllerObj.GetComponent<GameController>().EnemyTurn());
            }
            else
            {
                enemy.GetComponent<FighterStats>().guard = true;
                GameControllerObj.GetComponent<GameController>().state = GameController.BattleState.HEROTURN;
                GameControllerObj.GetComponent<GameController>().HeroTurn();
            }
            // Invoke("ContinueGame", 2);
        }
    }

    public void SkillText(string skillNameText)
    {
        if (victim == enemy)
        {
            GameControllerObj.GetComponent<GameController>().battlePlayerText.gameObject.SetActive(true);
            GameControllerObj.GetComponent<GameController>().battlePlayerText.text = skillNameText;
        }
        else
        {
            GameControllerObj.GetComponent<GameController>().battleEnemyText.gameObject.SetActive(true);
            GameControllerObj.GetComponent<GameController>().battleEnemyText.text = skillNameText;
        }
    }

    // void ContinueGame()
    // {
    //     GameObject.Find("GameControllerObject").GetComponent<GameController>().NextTurn();
    // }
}
