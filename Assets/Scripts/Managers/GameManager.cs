using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    UIManager ui = new UIManager();
    public  UIManager UI { get { return ui; } }

    PlayerData nowPlayerData;

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
            //����Ŵ��� �ʱ�ȭ
            instance.ui = new UIManager();
        }
    }
    /// <summary>
    /// GameManager�� instance�� ��ȯ�մϴ�.
    /// </summary>
    /// <returns></returns>
    public static GameManager getInstance()
    {
        Init();
        return instance;
    }

    /// <summary>
    /// ���� ���� @GameManager ������Ʈ ������ �����Դϴ�.
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
