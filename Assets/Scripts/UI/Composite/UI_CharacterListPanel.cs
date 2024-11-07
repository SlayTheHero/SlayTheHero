using System;
using System.Collections;
using System.Collections.Generic;
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
    List<UI_UnitButton> UnitButtons = new List<UI_UnitButton> ();
    public void LoadPlayerData()
    {
        if (manager == null)
            manager = GameManager.getInstance();
        UnitList = new List<GameObject>();
        int unitCount = manager.PlayerData.unitDeque.GetUnitCount(); 

        if(CharacterGridPanel.transform.childCount > 0 )
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
            UnitBase unit = manager.PlayerData.unitDeque.GetUnit(i);
            temp.GetComponent<Image>().sprite = ImageDB.GetImage(ImageDB.ImageType.Unit, unit.ID);
            UnitButtons.Add(temp.GetComponent<UI_UnitButton>());
            UnitButtons[i].SetSynergyData(((int)unit.Race, (int)unit.Job + 4, (int)unit.Feature + 7));
            UnitList.Add(temp);
        }
        if(unitCount <= 4)
        {
            canScroll = false;
            scrollbar.gameObject.SetActive(false);
        }
        else
        {
            canScroll = true;
            if(unitCount %2 == 0)
            {
                maxPaddingTop = unitCount / 2 * 310 + (unitCount / 2 - 1) * 30 - 810;
            }
            else
            {  
                maxPaddingTop = (unitCount / 2 + 1) * 310 + (unitCount / 2) * 30 - 810;
            }
        }
    }

    public void SetSynergy(bool isSynergyShow)
    {
        foreach(UI_UnitButton ui in UnitButtons)
        {
            ui.SetSynergyVisible(isSynergyShow);
        }
    }
    public void SetUnitEvent(Action<PointerEventData> ev, UI_EventHandler.UIEvent type)
    {
        for (int i = 0; i < UnitList.Count; i++)
        {
            UnitList[i].AddUIEvent(ev, type);
        }
    }

    public void SetUnitSelected(int index, bool isGrey)
    {
        if (UnitList.Count <= index) return;
        if(isGrey)
        { 
            UnitList[index].GetComponent<Image>().color = new Color32(255, 255, 255, 100);
        }
        else
        { 
            UnitList[index].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
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
        if(canScroll)
        {
            scrollbar.gameObject.SetActive(false);
        }
        else
        {
            scrollbar.gameObject.SetActive(true);
        }
    }

    void Start()
    {
        SetDragEvent();
        CharacterGrid = CharacterGridPanel.GetComponent<GridLayoutGroup>();
    }
     


}
