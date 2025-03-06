using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_StageEndPopup : UI_Base
{
    enum Texts
    {
        ClearText,
    }
    enum Buttons
    {
        Confirm,
    }
    void Start()
    {
        Init();
    }
    protected override void Init()
    {
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        GetUI<Button>((int)Buttons.Confirm).gameObject.AddUIEvent((p) => SceneController.ChangeScene(SceneController.SceneType.Maintenance));
        GetUI<TextMeshProUGUI>((int)Texts.ClearText).text = GetText(BattleManager.Instance.IsClear); 
    }
    string GetText(bool is_clear)
    {
        return is_clear ? "Clear" : "Fail";
    }
}
