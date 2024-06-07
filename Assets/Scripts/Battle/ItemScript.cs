using UnityEngine;
using TMPro;

public class ItemScript : MonoBehaviour
{
    public GameObject owner;
    [SerializeField] private GameObject GameControllerObj;
    [SerializeField] private TMP_Text RamuanMujarabValue;
    [SerializeField] private TMP_Text RamuanPemulaValue;
    private FighterStats ownerStats;
    [SerializeField] private string animationName;

    [SerializeField] private int healAmount;
    [SerializeField] private int itemAmount;
    public void Awake()
    {
        if(GameControllerObj == null) GameControllerObj = GameObject.Find("GameControllerObject");
        if(RamuanMujarabValue == null) RamuanMujarabValue = GameObject.Find("RamuanMujarabValue").GetComponent<TMP_Text>();
        if(RamuanPemulaValue == null) RamuanPemulaValue = GameObject.Find("RamuanPemulaValue").GetComponent<TMP_Text>();
    }

    public void Item()
    {
        ownerStats = owner.GetComponent<FighterStats>();
        if (itemAmount > 0)
        {
            ownerStats.Heal(healAmount);
            itemAmount--;
            if (owner.tag == "Hero")
            {
                if (gameObject.name == "WRamuanMujarabPrefab")
                {
                    RamuanMujarabValue.text = itemAmount.ToString();
                    GameControllerObj.GetComponent<GameController>().battlePlayerText.gameObject.SetActive(true);
                    GameControllerObj.GetComponent<GameController>().battlePlayerText.text = "<color=#07693a>Ramuan Mujarab! \n" + healAmount.ToString() + "</color>";
                }
                else if (gameObject.name == "WRamuanPemulaPrefab")
                {
                    RamuanPemulaValue.text = itemAmount.ToString();
                    GameControllerObj.GetComponent<GameController>().battlePlayerText.gameObject.SetActive(true);
                    GameControllerObj.GetComponent<GameController>().battlePlayerText.text = "<color=#07693a>Ramuan Pemula! \n" + healAmount.ToString() + "</color>";
                }
                GameControllerObj.GetComponent<GameController>().StartCoroutine(GameControllerObj.GetComponent<GameController>().HeroUseItem());
            } else
            {
                if (gameObject.name == "ERamuanMujarabPrefab")
                {
                    GameControllerObj.GetComponent<GameController>().battleEnemyText.gameObject.SetActive(true);
                    GameControllerObj.GetComponent<GameController>().battleEnemyText.text = "<color=#07693a>Ramuan Mujarab! \n" + healAmount.ToString() + "</color>";
                }
                else if (gameObject.name == "ERamuanPemulaPrefab")
                {
                    GameControllerObj.GetComponent<GameController>().battleEnemyText.gameObject.SetActive(true);
                    GameControllerObj.GetComponent<GameController>().battleEnemyText.text = "<color=#07693a>Ramuan Pemula! \n" + healAmount.ToString() + "</color>";
                }
            
            }
        }
        else
        {
            GameControllerObj.GetComponent<GameController>().battleText.gameObject.SetActive(true);
            GameControllerObj.GetComponent<GameController>().battleText.text = "Tidak ada item untuk dipakai!";
            if(owner.tag == "hero")
            {
                GameControllerObj.GetComponent<GameController>().StartCoroutine(GameControllerObj.GetComponent<GameController>().HeroUseItem());
            }
            else
            {
                GameControllerObj.GetComponent<GameController>().state = GameController.BattleState.HEROTURN;
            }
        }
    }
}