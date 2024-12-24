using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.Image;

public class BattleUI : MonoBehaviour
{
    [SerializeField]
    GameObject m_Cursors;
    [SerializeField]
    GameObject m_TargetCursors;
    [SerializeField]
    GameObject[] m_SkillBtns;
    [SerializeField]
    GameObject[] m_WaitingUnitInfoUIs;
    [SerializeField]
    SortableGrid m_SortableGrid;
    [SerializeField]
    Image m_CurUnitImage;

    UnitObject m_SelectedUnit;

    int? m_SelectedSkillNum = null;

    public UnityEvent<int, int> SkillTargetSelected;

    private void Awake()
    {
        SkillTargetSelected = new();
    }
    private void Start()
    {
        BattleManager.Instance.BattleUI = this;
        m_SkillBtns[0].GetComponent<Button>().onClick.AddListener(() => { SkillTargetingOn(0); });
        m_SkillBtns[1].GetComponent<Button>().onClick.AddListener(() => { SkillTargetingOn(1); });
        m_SkillBtns[2].GetComponent<Button>().onClick.AddListener(() => { SkillTargetingOn(2); });
        m_SkillBtns[3].GetComponent<Button>().onClick.AddListener(() => { SkillTargetingOn(3); });
    }
    private void Update()
    {
        if (m_SelectedSkillNum != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Unit")
                {
                    if (!m_SelectedUnit.IsDestroyed())
                        m_SelectedUnit?.Scale(2.5f, 2.5f, 0.5f);
                    m_SelectedUnit = hit.transform.GetComponent<UnitObject>();
                    m_SelectedUnit.Scale(3f, 3f, 0.5f);
                    SetTargetCursorEnable(true, m_SelectedUnit.Unit.Position);
                }
                if (Input.GetMouseButtonDown(0))
                {
                    if (hit.transform.tag == "Unit")
                    {
                        Debug.Log("HIT");
                        BattleManager.Instance.SkillUsed.Invoke(m_SelectedSkillNum.Value,
                            m_SelectedUnit.Unit);
                        m_SelectedUnit?.Scale(2.5f, 2.5f, 0.5f);
                        SkillTargetingOff();
                        SkillBtnOn(BattleManager.Instance.CurUnit.SkillList.Count, false);
                    }
                    else
                    {
                        SkillTargetingOff();
                    }
                }

            }
            else
            {
                if (m_SelectedUnit != null)
                {
                    m_SelectedUnit.Scale(2.5f, 2.5f, 0.5f);
                    SetTargetCursorEnable(false, 0);
                }
            }
        }
    }
    public void SetCursorEnable(bool value, int pos)
    {
        m_Cursors.SetActive(value);
        var dist = pos < 5 ? ((pos - 1f) * -2f) - 1.5f : (pos - 5f) * 2f + 1.5f;
        m_Cursors.transform.position = new Vector3(dist, m_Cursors.transform.position.y, 0);
    }
    public void SetTargetCursorEnable(bool value, int pos)
    {
        m_TargetCursors.SetActive(value);
        var dist = pos < 5 ? ((pos - 1f) * -2f) - 1.5f : (pos - 5f) * 2f + 1.5f;
        m_TargetCursors.transform.position = new Vector3(dist, m_TargetCursors.transform.position.y, 0);
    }
    public void SkillBtnOn(int count, bool is_interatable)
    {
        var bm = BattleManager.Instance;
        for (int i = 0; i < count; i++)
        {

            m_SkillBtns[i].GetComponent<Button>().enabled = is_interatable;
            m_SkillBtns[i].SetActive(true);
        }
    }
    public void SkillBtnOff()
    {
        for (int i = 0; i < 4; i++)
        {
            m_SkillBtns[i].SetActive(false);
        }
    }
    public void SkillTargetingOff()
    {
        AlphaReset();
        SetUntargetableAll();
        m_SelectedSkillNum = null;
    }
    void SkillTargetingOn(int idx)
    {
        if (m_SelectedSkillNum == null)
        {
            m_SelectedSkillNum = idx;
            var unit = BattleManager.Instance.CurUnit;
            var range = unit.SkillList[idx].range;
            ShowTargetableUnit(range, unit.Position);
            SetTargetingEnable(range, unit.Position, true);
        }
    }
    void ShowTargetableUnit(int range, int origin)
    {
        origin = origin < 5 ? 5 - origin : origin;
        int start = origin - range > 1 ? origin - range : 1;
        int end = origin + range > 8 ? 8 : origin + range;
        foreach (var item in BattleManager.Instance.Units)
        {
            bool is_active = false;
            var pos = item.Key;
            pos = pos < 5 ? 5 - pos : pos;
            if (pos >= start && pos <= end)
                is_active = true;
            ChageAlpha(item.Value, is_active);
        }
    }
    void ChageAlpha(GameObject target, bool can_select)
    {
        var sr = target.GetComponentsInChildren<SpriteRenderer>();
        foreach (var c in sr)
        {
            c.color = new Color(c.color.r, c.color.g, c.color.b, can_select ? 1f : 0.5f);
        }
    }
    void AlphaReset()
    {
        foreach (var unit in BattleManager.Instance.Units)
        {
            ChageAlpha(unit.Value, true);
        }
    }
    void SetTargetingEnable(int range, int origin, bool value)
    {
        origin = origin < 5 ? 5 - origin : origin;
        int start = origin - range > 1 ? origin - range : 1;
        int end = origin + range > 8 ? 8 : origin + range;
        foreach (var item in BattleManager.Instance.Units)
        {
            var pos = item.Key;
            pos = pos < 5 ? 5 - pos : pos;
            bool is_active = !value;
            BoxCollider boxCollider2D = item.Value.GetComponent<BoxCollider>();
            if (pos >= start && pos <= end)
                is_active = value;
            boxCollider2D.enabled = is_active;
        }
        // 4 3 2 1 5 6 7 8
    }
    void SetUntargetableAll()
    {
        SetTargetingEnable(8, 0, false);
    }

    public void InitWaitingUnitInfo(List<UnitBase> waiting_list)
    {
        m_SortableGrid.ItemsReset(waiting_list.Count);
        for (int i = 0; i < waiting_list.Count; i++)
        {
            SetWaitingUnitInfo(i, waiting_list[i], true);

        }
    }

    public void SetWaitingUnitInfo(int pos, UnitBase data, bool is_enabled)
    {
        m_WaitingUnitInfoUIs[pos].SetActive(is_enabled);
        m_WaitingUnitInfoUIs[pos].GetComponent<Image>().sprite = ImageDB.GetImage(ImageDB.ImageType.Unit, data.ID);
        m_WaitingUnitInfoUIs[pos].GetComponentInChildren<TextMeshProUGUI>().text = data.Position.ToString();
    }

    public void SortWaitingUI(List<UnitBase> waiting_list)
    {
        IComparable[] list = new IComparable[waiting_list.Count];
        for (int i = 0; i < list.Length; i++)
        {
            list[i] = (waiting_list[i].Status.Waiting);
        }
        m_SortableGrid.Sort(list);
    }
    public void OnUnitDead(UnitBase unit)
    {
        m_SortableGrid.RemoveItem(unit.Order);
    }

    public void SetCurUnitInfo(UnitBase cur_unit)
    {
        m_CurUnitImage.sprite = ImageDB.GetImage(ImageDB.ImageType.Unit, cur_unit.ID);
        for (int i = 0; i < cur_unit.SkillList.Count; i++)
        {
            m_SkillBtns[i].GetComponent<Image>().sprite = ImageDB.GetImage(ImageDB.ImageType.Skill, cur_unit.SkillList[i].id);
        }
    }
}
