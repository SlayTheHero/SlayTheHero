using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_BuffToolTipEventHandler : UI_BaseToolTipEventHandler
{
    TextMeshProUGUI BuffName;
    TextMeshProUGUI BuffExplanation;
    TextMeshProUGUI BuffType;
    Image BuffImage;
    Button CloseButton;
    int nowBuffID;

    public void setBuffID(int id)
    {
        nowBuffID = id; setData();
    }

    void Awake()
    {
        InitWithName("UI_BuffToolTip");
        BuffName = ToolTipInstance.transform.Find("UI_BuffName").GetComponent<TextMeshProUGUI>();
        BuffExplanation = ToolTipInstance.transform.Find("UI_BuffExplanation").GetComponent<TextMeshProUGUI>();
        BuffType = ToolTipInstance.transform.Find("UI_BuffType").GetComponent<TextMeshProUGUI>();
        BuffImage = ToolTipInstance.transform.Find("UI_BuffImage").GetComponent<Image>();
        CloseButton = ToolTipInstance.transform.Find("UI_CloseButton").GetComponent<Button>();
        CloseButton.gameObject.AddUIEvent(setInActiveToolTip, UIEvent.LClick);
    }

    protected override void setData()
    {
        if (nowBuffID == -1)
        {
            BuffImage.sprite = null;
            BuffName.text = "없음";
            BuffExplanation.text = "없음"; 
            return;
        }
        Synergy tempBuff = SynergyDB.GetSynergy(nowBuffID);
        BuffName.text = tempBuff.name;
        BuffExplanation.text = tempBuff.description;
        BuffImage.sprite = ImageDB.GetImage(ImageDB.ImageType.Synergy, nowBuffID);

        // 크기조정
        RectTransform ToolTipRect = ToolTipInstance.GetComponent<RectTransform>(); 
        if(SceneManager.GetActiveScene().name.Contains("Battle"))
        {
            ToolTipRect.sizeDelta = new Vector2(6, 3);
        }
        else
        {
            ToolTipRect.sizeDelta = new Vector2(600, 300);
        }
    }

}
