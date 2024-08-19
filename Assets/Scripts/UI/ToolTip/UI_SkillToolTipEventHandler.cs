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
    }

    protected override void setData()
    {
        Skill tempSkill = SkillDB.GetSkill(nowSkillID);
        SkillName.text = tempSkill.name;
        SkillExplanation.text = tempSkill.description;
        // 크기조정
        RectTransform ToolTipRect = ToolTipInstance.GetComponent<RectTransform>();
        float textWidth = SkillExplanation.preferredWidth;
        float textHeight = SkillExplanation.preferredHeight + SkillType.preferredHeight;
        ToolTipRect.sizeDelta = new Vector2(textWidth, textHeight);
    }
}
