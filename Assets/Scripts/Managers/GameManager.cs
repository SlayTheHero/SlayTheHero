using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    UIManager ui = new UIManager();
    public  UIManager UI { get { return ui; } }

    PlayerData nowPlayerData = new PlayerData();

    public PlayerData PlayerData { get {  return nowPlayerData; } }


    private static void Init()
    {
        if (instance == null)
        {
            GameObject mg = GameObject.Find("@GameManager");
            if (mg == null)
            {
                mg = new GameObject { name = "@GameManager" };
                mg.AddComponent<GameManager>();
            }
            DontDestroyOnLoad(mg);
            instance = mg.GetComponent<GameManager>();
            //서브매니저 초기화
            instance.ui = new UIManager();
        }
    }
    /// <summary>
    /// GameManager의 instance를 반환합니다.
    /// </summary>
    /// <returns></returns>
    public static GameManager getInstance()
    {
        Init();
        return instance;
    }

    /// <summary>
    /// 기존 씬에 @GameManager 오브젝트 있을시 로직입니다.
    /// </summary>
    private void Awake()
    {
        Init();
        if(instance.gameObject != gameObject)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    { 
    }
}
