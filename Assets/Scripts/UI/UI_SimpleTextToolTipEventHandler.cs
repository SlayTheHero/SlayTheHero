using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// �ؽ�Ʈ�� �ִ� �����Դϴ�.
/// �����ʹ� �ܺο��� �����ɴϴ�.(���� ���ٸ� �ϵ��ڵ��ص� ��� �����ϴ�.)
/// </summary>
public class UI_SimpleTextToolTipEventHandler : UI_BaseToolTipEventHandler
{

    void Awake()
    {
        InitWithName("UI_SimpleTextToolTip");

        // ũ������
        RectTransform ToolTipRect = ToolTipInstance.GetComponent<RectTransform>();
        TextMeshProUGUI textMesh = ToolTipInstance.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        float textWidth = textMesh.preferredWidth;
        float textHeight = textMesh.preferredHeight;
        ToolTipRect.sizeDelta = new Vector2(textWidth, textHeight);
    }

    /// <summary>
    /// �ӽ�. GameObject�� �����͸� �θ��� �����ͷ� �ٲߴϴ�.
    /// </summary>
    protected override void setData()
    {
        GameObject go = ToolTipInstance.transform.GetChild(0).gameObject;
        TextMeshProUGUI textMeshPro = go.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = this.name;
    }

}
