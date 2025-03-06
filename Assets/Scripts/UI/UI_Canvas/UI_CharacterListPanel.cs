using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_CharacterListPanel : UI_EventHandler
{
    List<GameObject> UnitList;
    GameManager manager;
    public GameObject CharacterGridPanel;
    public Scrollbar scrollbar;

    GridLayoutGroup CharacterGrid;
    List<UI_UnitButton> UnitButtons = new List<UI_UnitButton>();
    public List<UnitBase> unitList = new List<UnitBase>();

    private void LoadPlayerData()
    {
        int unitCount = manager.PlayerData.unitDeque.GetUnitCount();
        unitList = new List<UnitBase>();
        for (int i = 0; i < unitCount; i++)
        {
            unitList.Add(manager.PlayerData.unitDeque.GetUnit(i));
        }
    }
    private void DisplayUnitButtons()
    {
        int unitCount = unitList.Count;
        UnitList = new List<GameObject>();
        if (CharacterGridPanel.transform.childCount > 0)
        {
            foreach (Transform child in CharacterGridPanel.transform)
            {
                if (child.name == CharacterGridPanel.name) continue;
                Image nowIm = child.GetComponent<Image>();
                nowIm.sprite = null;
                GameObject.Destroy(child.gameObject);
            }
        }
        GameObject prefab = Resources.Load<GameObject>("Prefabs/UI/Composite/UI_UnitButton");
        UnitButtons = new List<UI_UnitButton>();
        for (int i = 0; i < unitCount; i++)
        {
            GameObject temp = GameObject.Instantiate(prefab, CharacterGridPanel.transform);
            temp.name = "UI_Character_" + i;
            UnitBase unit = unitList[i];

            UnitButtons.Add(temp.GetComponent<UI_UnitButton>());
            UnitButtons[i].SetSynergyData(((int)Synergy.FromUnitEnumToSynergy(unit.Race),
                                           (int)Synergy.FromUnitEnumToSynergy(unit.Job),
                                           (int)Synergy.FromUnitEnumToSynergy(unit.Feature)));
            UnitButtons[i].SetImage(ImageDB.GetImage(ImageDB.ImageType.Unit, unit.ID));
            UnitList.Add(temp);
        }
        if (unitCount <= 4)
        {
            canScroll = false;
            scrollbar.gameObject.SetActive(false);
        }
        else
        {
            canScroll = true;
            if (unitCount % 2 == 0)
            {
                maxPaddingTop = unitCount / 2 * 310 + (unitCount / 2 - 1) * 30 - 810;
            }
            else
            {
                maxPaddingTop = (unitCount / 2 + 1) * 310 + (unitCount / 2) * 30 - 810;
            }
        }
    }
    public void Initialize(Type type = null)
    {
        if (manager == null)
            manager = GameManager.getInstance();

        LoadPlayerData();
        SortUnitList(type);
        DisplayUnitButtons();
        ApplyUnitEvent();
    }

    private static readonly Dictionary<Type, object> Enum_Sort_Data = new Dictionary<Type, object>
    {
    // 마수, 몽마, 뱀파이어, 유령, 인간
    { typeof(Race), new Dictionary<Race, int> {
        { Race.DemonBeast, 0 }, { Race.NightMare, 1 }, { Race.Vampire, 2 }, { Race.Ghost, 3 }, { Race.Human, 4 } } },

    // 검사, 궁사, 마법사, 기사, 농부, 용병, 성기사, 성직자
    { typeof(Job), new Dictionary<Job, int> {
        { Job.SwordMan, 0 }, { Job.Archer, 1 }, { Job.Magician, 2 }, { Job.Knight, 3 }, { Job.Farmer, 4 }, { Job.Paladin, 5 }, { Job.Priest, 6 } } },

    // 나태, 신속, 의심암귀, 질투, 귀족, 꿈꾸지않는자, 불굴, 사냥꾼, 신자, 정의
    { typeof(Feature), new Dictionary<Feature, int> {
        { Feature.Sloth, 0 }, { Feature.Swiftness, 1 }, { Feature.SuspiciousGhost, 2 }, { Feature.Envy, 3 }, { Feature.Noble, 4 },
        { Feature.Dreamless, 5 }, { Feature.Fortify, 6 }, { Feature.Hunter, 7 }, { Feature.Faithful, 8 }, { Feature.Justice, 9 } } }
    };

    private void SortUnitsByEnum<T>(List<UnitBase> units, Type enumType, Func<UnitBase, T> getEnum) where T : Enum
    {
        if (!Enum_Sort_Data.TryGetValue(enumType, out object sortOrderObj) || !(sortOrderObj is Dictionary<T, int> sortOrder))
        {
            throw new ArgumentException("UndeFined Enum Value.");
        }

        unitList.Sort((a, b) =>
        {
            int indexA = sortOrder.TryGetValue(getEnum(a), out int aIndex) ? aIndex : int.MaxValue;
            int indexB = sortOrder.TryGetValue(getEnum(b), out int bIndex) ? bIndex : int.MaxValue;

            return indexA.CompareTo(indexB);
        });
    }


    private void SortUnitList(Type type)
    {
        if (type == null || unitList.Count == 0)
        {
            return;
        }
        if (type == typeof(Race))
        {
            SortUnitsByEnum(unitList, type, u => u.Race);
        }
        if (type == typeof(Feature))
        {
            SortUnitsByEnum(unitList, type, u => u.Feature);
        }
        if (type == typeof(Job))
        {
            SortUnitsByEnum(unitList, type, u => u.Job);
        }
        Debug.Log("sorted");
    }

    public void SetSynergy(bool isSynergyShow)
    {
        foreach (UI_UnitButton ui in UnitButtons)
        {
            ui.SetSynergyVisible(isSynergyShow);
        }
    }

    List<(Action<PointerEventData>, UI_EventHandler.UIEvent)> UnitEvents = new List<(Action<PointerEventData>, UI_EventHandler.UIEvent)>();
    public void SetUnitEvent(Action<PointerEventData> ev, UI_EventHandler.UIEvent type)
    {
        UnitEvents.Add((ev,type));
    }
    private void ApplyUnitEvent()
    {
        for (int i = 0; i < UnitEvents.Count; i++)
        {
            for (int j = 0; j < UnitList.Count; j++)
            {
                UnitList[j].AddUIEvent(UnitEvents[i].Item1, UnitEvents[i].Item2);
            }
        }
    }

    public void SetUnitSelected(int index, bool isGrey)
    {
        if (UnitList.Count <= index) return;
        UnitButtons[index].SetSelected(isGrey);
    }

    Coroutine PushScrollCoroutine;
    private float dragStartPosY;
    private float scrollbarStartValue;

    private void SetDragEvent()
    {
        CharacterGridPanel.GetComponent<UI_ScrollEventHandler>().OnDragBeginHandler -= OnBeginDrag;
        CharacterGridPanel.GetComponent<UI_ScrollEventHandler>().OnDragBeginHandler += OnBeginDrag;
        CharacterGridPanel.GetComponent<UI_ScrollEventHandler>().OnDragHandler -= OnDrag;
        CharacterGridPanel.GetComponent<UI_ScrollEventHandler>().OnDragHandler += OnDrag;
        CharacterGridPanel.GetComponent<UI_ScrollEventHandler>().OnDragEndHandler -= OnEndDrag;
        CharacterGridPanel.GetComponent<UI_ScrollEventHandler>().OnDragEndHandler += OnEndDrag;
        scrollbar.onValueChanged.AddListener(OnScroll);
    }

    public float inertiaDuration = 0.5f; // 관성 지속 시간
    public float inertiaDamping = 0.9f;  // 관성 감속률

    private float lastDragDeltaY;
    private float dragSpeed;
    private bool isDragging = false;
    private Coroutine inertiaCoroutine;


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!canScroll) return;
        dragStartPosY = eventData.position.y;
        scrollbarStartValue = scrollbar.value;
        lastDragDeltaY = 0f;
        isDragging = true;

        // 관성 처리 중이면 멈춤
        if (inertiaCoroutine != null)
        {
            StopCoroutine(inertiaCoroutine);
            inertiaCoroutine = null;
        }
    }

    // 드래그 중 호출
    public void OnDrag(PointerEventData eventData)
    {
        if (!canScroll) return;
        if (scrollbar == null)
            return;

        // 드래그한 거리 계산
        float dragDeltaY = eventData.position.y - dragStartPosY;
        float screenHeight = Screen.height;
        float scrollValueDelta = dragDeltaY / screenHeight;

        scrollbar.value = Mathf.Clamp(scrollbarStartValue + scrollValueDelta, 0f, 1f);

        // 드래그 속도 계산 
        dragSpeed = (dragDeltaY - lastDragDeltaY) / Time.deltaTime;
        lastDragDeltaY = dragDeltaY;
    }

    // 드래그 종료 시 호출
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!canScroll) return;
        isDragging = false;

        // 드래그가 끝남
        if (inertiaCoroutine == null)
        {
            inertiaCoroutine = StartCoroutine(HandleInertia());
        }
    }

    // 관성을 처리하는 코루틴
    private IEnumerator HandleInertia()
    {
        float inertiaTimer = inertiaDuration;

        while (inertiaTimer > 0 && Mathf.Abs(dragSpeed) > 0.01f)
        {
            inertiaTimer -= Time.deltaTime;

            // 관성 감속
            dragSpeed *= inertiaDamping;

            // 스크롤바 값 업데이트
            float scrollValueDelta = (dragSpeed / Screen.height) * Time.deltaTime;
            scrollbar.value = Mathf.Clamp(scrollbar.value + scrollValueDelta, 0f, 1f);

            yield return null;
        }

        //종료
        inertiaCoroutine = null;
    }

    public int ScrollAdjustValue = 100;

    public float maxPaddingTop; // 패딩의 최대 높이

    void OnScroll(float scrollPosition)
    {
        if (!canScroll) return;
        float newPaddingTop = Mathf.Lerp(-maxPaddingTop, 0, 1f - scrollPosition);

        CharacterGrid.padding.top = 50 + Mathf.RoundToInt(newPaddingTop);

        CharacterGrid.SetLayoutVertical();
    }

    bool canScroll = false;
    void SetScroll()
    {
        if (canScroll)
        {
            scrollbar.gameObject.SetActive(false);
        }
        else
        {
            scrollbar.gameObject.SetActive(true);
        }
    }


    private void SetSortEvent()
    {
        Utility.FindChild<Transform>(transform.gameObject, "UI_Tab_1", true).gameObject.AddUIEvent((p) => { Initialize(typeof(Race)); }, UIEvent.LClick);
        Utility.FindChild<Transform>(transform.gameObject, "UI_Tab_2", true).gameObject.AddUIEvent((p) => { Initialize(typeof(Feature)); }, UIEvent.LClick);
        Utility.FindChild<Transform>(transform.gameObject, "UI_Tab_3", true).gameObject.AddUIEvent((p) => { Initialize(typeof(Job)); }, UIEvent.LClick);
        Utility.FindChild<Transform>(transform.gameObject, "UI_Tab_4", true).gameObject.AddUIEvent((p) => { Initialize(); }, UIEvent.LClick);
    }
    void Start()
    {
        SetDragEvent();
        SetSortEvent();
        CharacterGrid = CharacterGridPanel.GetComponent<GridLayoutGroup>();
    }



}
