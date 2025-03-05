using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class UI_UnitPanel : UI_Base
{
    [SerializeField]
    Color _attaker_cursor_color;
    [SerializeField]
    Color _target_cursor_color;
    int _pos;
    enum Sliders
    {
        HPBar
    }
    enum Images
    {
        Cursor
    }
    private void Start()
    {
        Init();
    }
    protected override void Init()
    {
        Bind<Slider>(typeof(Sliders));
        Bind<Image>(typeof(Images));
        GetUI<Image>((int)Images.Cursor).gameObject.SetActive(false);

        transform.parent.GetComponent<UnitController>().UnitStateChange.AddListener(OnUnitStateChanged);

        GetUI<Slider>((int)Sliders.HPBar).maxValue = transform.parent.GetComponent<UnitController>().Unit.Status.MaxHP;
    }
    void HPChanged()
    {
        var unit = BattleManager.Instance.Units[_pos].GetComponent<UnitController>().Unit;
        GetUI<Slider>((int)Sliders.HPBar).DOValue(unit.Status.HP, 1f);
    }
    void OnUnitStateChanged(UnitController.UnitState state)
    {
        switch (state)
        {
            case UnitController.UnitState.Hit:
                HPChanged();
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
