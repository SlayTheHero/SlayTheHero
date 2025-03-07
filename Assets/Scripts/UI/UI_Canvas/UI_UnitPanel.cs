using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class UI_UnitPanel : UI_Base
{
    [SerializeField]
    Color _attaker_cursor_color;
    [SerializeField]
    Color _target_cursor_color;
    BuffViewer buff;
    int _pos;
    UnitBase unit;
    enum Sliders
    {
        HPBar
    }
    enum Images
    {
        Cursor
    }
    enum GameObjects
    {
        BuffViewer
    }
    private void Start()
    {
        Init();
    }
    protected override void Init()
    {
        Bind<Slider>(typeof(Sliders));
        Bind<Image>(typeof(Images));
        Bind<GameObject>(typeof(GameObjects));
        GetUI<Image>((int)Images.Cursor).gameObject.SetActive(false);

        transform.parent.GetComponent<UnitController>().UnitStateChange.AddListener(OnUnitStateChanged);
        buff = GetUI<GameObject>((int)GameObjects.BuffViewer).GetComponent<BuffViewer>();

        unit = transform.parent.GetComponent<UnitController>().Unit;
        buff.SetBuff(unit);
        GetUI<Slider>((int)Sliders.HPBar).maxValue = unit.Status.MaxHP;
    }
    void HPChanged()
    { 
        GetUI<Slider>((int)Sliders.HPBar).DOValue(unit.Status.HP, 1f);
    }
    void OnUnitStateChanged(UnitController.UnitState state)
    {
        switch (state)
        {
            case UnitController.UnitState.Hit:
                HPChanged();
                buff.SetBuff(unit); 
                break;
            case UnitController.UnitState.Attacker:
                GetImage((int)Images.Cursor).gameObject.SetActive(true);
                GetImage((int)Images.Cursor).color = _attaker_cursor_color;
                break;
            case UnitController.UnitState.Default:
                GetImage((int)Images.Cursor).gameObject.SetActive(false);
                break;
            case UnitController.UnitState.Target:
                GetImage((int)Images.Cursor).gameObject.SetActive(true);
                GetImage((int)Images.Cursor).color = _target_cursor_color;
                break;
            case UnitController.UnitState.Targetable:
                break;
            default:
                GetImage((int)Images.Cursor).gameObject.SetActive(false);
                break;

        }
    }
    private void OnDestroy()
    {
        transform.parent.GetComponent<UnitController>().UnitStateChange.RemoveListener(OnUnitStateChanged);
    }

}
