using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 텍스트만 있는 툴팁입니다.
/// 데이터는 외부에서 가져옵니다.(양이 적다면 하드코딩해도 상관 없습니다.)
/// </summary>
public class UI_SimpleTextToolTipEventHandler : UI_BaseToolTipEventHandler
{

    void Awake()
    {
        InitWithName("UI_SimpleTextToolTip");

        // 크기조정
        RectTransform ToolTipRect = ToolTipInstance.GetComponent<RectTransform>();
        TextMeshProUGUI textMesh = ToolTipInstance.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        float textWidth = textMesh.preferredWidth;
        float textHeight = textMesh.preferredHeight;
        ToolTipRect.sizeDelta = new Vector2(textWidth, textHeight);
    }

    /// <summary>
    /// 임시. GameObject의 데이터를 부모의 데이터로 바꿉니다.
    /// </summary>
    protected override void setData()
    {
        GameObject go = ToolTipInstance.transform.GetChild(0).gameObject;
        TextMeshProUGUI textMeshPro = go.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = this.name;
    }

}
