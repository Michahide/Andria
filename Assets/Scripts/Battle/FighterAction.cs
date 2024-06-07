using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterAction : MonoBehaviour
{
    [SerializeField] private GameObject hero;
    [SerializeField] private GameObject enemy;

    [SerializeField] private GameObject meleePrefab;

    [SerializeField] private GameObject rangePrefab;
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private GameObject waterSlashPrefab;
    [SerializeField] private GameObject chainLightningPrefab;
    [SerializeField] private GameObject stompPrefab;
    [SerializeField] private GameObject iceStormPrefab;
    [SerializeField] private GameObject windSlashPrefab;
    [SerializeField] private GameObject ramuanMujarabPrefab;
    [SerializeField] private GameObject ramuanPemulaPrefab;
    [SerializeField] private GameObject magicBurstPrefab;
    [SerializeField] private GameObject hempasanRatuPrefab;
    [HideInInspector] public string currentAction;


    [SerializeField] private Sprite faceIcon;
    private GameObject gameAudioManager;

    private GameObject currentAttack;
    [SerializeField] private GameObject GameControllerObj;
    private string AttackText = "Serangan Normal!";
    private string FireballText = "Bola Api!";
    private string WaterSlashText = "Tebasan Air!";
    private string ChainLightningText = "Rambatan Petir!";
    private string StompText = "Hentakan!";
    private string WindSlashText = "Tebasan Angin!";
    private string IceStormText = "Badai Es!";
    private string GuardText = "Guard!";
    private GameObject victim;

    public int GetCurrentActionType;

    void Start()
    {
        if (hero == null) hero = GameObject.FindGameObjectWithTag("Hero");
        if (enemy == null) enemy = GameObject.FindGameObjectWithTag("Enemy");

        if (GameControllerObj == null) GameControllerObj = GameObject.Find("GameControllerObject");
    }
    public void SelectAction(string btn)
    {
        victim = hero;
        currentAction = "";

        if (tag == "Hero")
        {
            victim = enemy;
        }
        if (victim != null)
        {
            if (btn.CompareTo("melee") == 0)
            {
                meleePrefab.GetComponent<AttackScript>().Attack(victim);
                if (AudioManager.Instance != null) AudioManager.Instance.Play("NormalAttack");
                GetCurrentActionType = 1;
                SkillText(AttackText);

            }
            else if (btn.CompareTo("range") == 0)
            {
                rangePrefab.GetComponent<AttackScript>().Attack(victim);
                if (AudioManager.Instance != null) AudioManager.Instance.Play("MagicAttack");
                SkillText(AttackText);
            }
            else if (btn.CompareTo("guard") == 0)
            {
                if (tag == "Hero")
                {
                    hero.GetComponent<FighterStats>().guard = true;
                    GameControllerObj.GetComponent<GameController>().battlePlayerText.gameObject.SetActive(true);
                    GameControllerObj.GetComponent<GameController>().battlePlayerText.text = GuardText;
                    GameControllerObj.GetComponent<GameController>().state = GameController.BattleState.ENEMYTURN;
                    GameControllerObj.GetComponent<GameController>().StartCoroutine(GameControllerObj.GetComponent<GameController>().EnemyTurn());
                }
                else
                {
                    enemy.GetComponent<FighterStats>().guard = true;
                    GetCurrentActionType = 2;
                    currentAction = "guard";
                    GameControllerObj.GetComponent<GameController>().battleEnemyText.gameObject.SetActive(true);
                    GameControllerObj.GetComponent<GameController>().battleEnemyText.text = GuardText;
                    GameControllerObj.GetComponent<GameController>().state = GameController.BattleState.HEROTURN;
                    // GameControllerObj.GetComponent<GameController>().HeroTurn();
                    GameControllerObj.GetComponent<GameController>().StartCoroutine(GameControllerObj.GetComponent<GameController>().HeroTraining());
                }
                // Invoke("ContinueGame", 2);
            }
            else if (btn.CompareTo("fireball") == 0)
            {
                fireballPrefab.GetComponent<AttackScript>().Attack(victim);
                if (AudioManager.Instance != null) AudioManager.Instance.Play("MagicAttack");
                GetCurrentActionType = 3;
                SkillText(FireballText);
            }
            else if (btn.CompareTo("waterSlash") == 0)
            {
                waterSlashPrefab.GetComponent<AttackScript>().Attack(victim);
                if (AudioManager.Instance != null) AudioManager.Instance.Play("MagicAttack");
                GetCurrentActionType = 3;
                SkillText(WaterSlashText);
            }
            else if (btn.CompareTo("chainLightning") == 0)
            {
                chainLightningPrefab.GetComponent<AttackScript>().Attack(victim);
                if (AudioManager.Instance != null) AudioManager.Instance.Play("MagicAttack");
                GetCurrentActionType = 4;
                SkillText(ChainLightningText);
            }
            else if (btn.CompareTo("stomp") == 0)
            {
                stompPrefab.GetComponent<AttackScript>().Attack(victim);
                if (AudioManager.Instance != null) AudioManager.Instance.Play("MagicAttack");
                GetCurrentActionType = 5;
                SkillText(StompText);
            }
            else if (btn.CompareTo("iceStorm") == 0)
            {
                iceStormPrefab.GetComponent<AttackScript>().Attack(victim);
                if (AudioManager.Instance != null) AudioManager.Instance.Play("MagicAttack");
                GetCurrentActionType = 6;
                SkillText(IceStormText);
            }
            else if (btn.CompareTo("windSlash") == 0)
            {
                windSlashPrefab.GetComponent<AttackScript>().Attack(victim);
                if (AudioManager.Instance != null) AudioManager.Instance.Play("MagicAttack");
                GetCurrentActionType = 7;
                SkillText(WindSlashText);
            }
            else if (btn.CompareTo("ramuanMujarab") == 0)
            {
                ramuanMujarabPrefab.GetComponent<ItemScript>().Item();
                if (AudioManager.Instance != null) AudioManager.Instance.Play("MagicAttack");
                GetCurrentActionType = 9;
                currentAction = "ramuanMujarab";
            }
            else if (btn.CompareTo("ramuanPemula") == 0)
            {
                ramuanPemulaPrefab.GetComponent<ItemScript>().Item();
                if (AudioManager.Instance != null) AudioManager.Instance.Play("MagicAttack");
                GetCurrentActionType = 10;
                currentAction = "ramuanPemula";
            }

            // Non elemental attack
            else if (btn.CompareTo("hempasanRatu") == 0)
            {
                hempasanRatuPrefab.GetComponent<AttackScript>().Attack(victim);
                if (AudioManager.Instance != null) AudioManager.Instance.Play("MagicAttack");
                GetCurrentActionType = 3;
                SkillText("Hempasan Ratu!");
            }
            else if (btn.CompareTo("magicBurst") == 0)
            {
                magicBurstPrefab.GetComponent<AttackScript>().Attack(victim);
                if (AudioManager.Instance != null) AudioManager.Instance.Play("MagicAttack");
                SkillText("Magic Burst!");
            }
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
}
