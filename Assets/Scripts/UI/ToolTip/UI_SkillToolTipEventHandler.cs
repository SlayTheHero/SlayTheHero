using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillToolTipEventHandler : UI_BaseToolTipEventHandler
{
    TextMeshProUGUI SkillName;
    TextMeshProUGUI SkillExplanation;
    TextMeshProUGUI SkillType;
    Image SkillImage;
    Button CloseButton;
    int nowSkillID;

    public void setSkillID(int id)
    {
        nowSkillID = id;
    }

    void Awake()
    {
        InitWithName("UI_SkillToolTip");
        SkillName = ToolTipInstance.transform.Find("UI_SkillName").GetComponent<TextMeshProUGUI>();
        SkillExplanation = ToolTipInstance.transform.Find("UI_SkillExplanation").GetComponent<TextMeshProUGUI>();
        SkillType = ToolTipInstance.transform.Find("UI_SkillType").GetComponent<TextMeshProUGUI>();
        SkillImage = ToolTipInstance.transform.Find("UI_SkillImage").GetComponent<Image>();
        CloseButton = ToolTipInstance.transform.Find("UI_CloseButton").GetComponent<Button>();
        CloseButton.gameObject.AddUIEvent(setInActiveToolTip, UIEvent.LClick);
    }

    protected override void setData()
    {
        if (nowSkillID == -1)
        {
            SkillImage.sprite = null;
            SkillName.text = "스킬없음";
            SkillExplanation.text = "스킬없음"; 
            return;
        }
        Skill tempSkill = SkillDB.GetSkill(nowSkillID);
        SkillName.text = tempSkill.name;
        SkillExplanation.text = tempSkill.description;
        SkillImage.sprite = ImageDB.GetImage(ImageDB.ImageType.Skill, nowSkillID);

        // 크기조정
        RectTransform ToolTipRect = ToolTipInstance.GetComponent<RectTransform>();
        float textWidth = SkillExplanation.preferredWidth;
        float textHeight = SkillExplanation.preferredHeight + SkillType.preferredHeight + SkillName.preferredHeight;
        ToolTipRect.sizeDelta = new Vector2(textWidth, textHeight);
    }

}
