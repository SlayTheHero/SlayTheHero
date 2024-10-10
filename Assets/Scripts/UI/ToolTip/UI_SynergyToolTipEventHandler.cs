using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UI_SynergyToolTipEventHandler : UI_BaseToolTipEventHandler
{
    TextMeshProUGUI UI_SynergyName;
    TextMeshProUGUI UI_SynergyTwoExplanation;
    TextMeshProUGUI UI_SynergyThreeExplanation;
    Image UI_SynergyImage;
    Button UI_CloseButton;

    int nowSynergyId;
    
    public void setSynergyID(int id)
    {
        nowSynergyId = id;
        setData();
    }

    void Awake()
    {
        InitWithName("UI_SynergyToolTip");
        UI_SynergyName = ToolTipInstance.transform.Find("UI_SynergyName").GetComponent<TextMeshProUGUI>();
        UI_SynergyTwoExplanation = ToolTipInstance.transform.Find("UI_SynergyTwoExplanation").GetComponent<TextMeshProUGUI>();
        UI_SynergyThreeExplanation = ToolTipInstance.transform.Find("UI_SynergyThreeExplanation").GetComponent<TextMeshProUGUI>();
        UI_SynergyImage = ToolTipInstance.transform.Find("UI_SynergyImage").GetComponent<Image>();
        UI_CloseButton = ToolTipInstance.transform.Find("UI_CloseButton").GetComponent<Button>();
        UI_CloseButton.gameObject.AddUIEvent(setInActiveToolTip, UIEvent.LClick);
    }

    protected override void setData()
    {
        if (nowSynergyId == -1)
        {
            UI_SynergyImage.sprite = null;
            UI_SynergyName.text = "없음";
            UI_SynergyTwoExplanation.text = "없음";
            UI_SynergyThreeExplanation.text = "없음";
            return;
        }
        Synergy sy = SynergyDB.GetSynergy(nowSynergyId);
        UI_SynergyImage.sprite = null;
        UI_SynergyName.text = sy.name;
        UI_SynergyTwoExplanation.text = $"{sy.sK_Attribute.ToString()} : {sy.twoImpact}";
        UI_SynergyThreeExplanation.text = $"{sy.sK_Attribute.ToString()} : {sy.threeImpact}";
        UI_SynergyImage.sprite = ImageDB.GetImage(ImageDB.ImageType.Synergy, nowSynergyId);
        // 크기조정
        RectTransform ToolTipRect = ToolTipInstance.GetComponent<RectTransform>();
        float textWidth = UI_SynergyImage.preferredWidth + UI_SynergyTwoExplanation.preferredWidth;
        float textHeight = UI_SynergyImage.preferredHeight + UI_SynergyTwoExplanation.preferredHeight + UI_SynergyThreeExplanation.preferredHeight;
        ToolTipRect.sizeDelta = new Vector2(textWidth, textHeight);
    }
}
