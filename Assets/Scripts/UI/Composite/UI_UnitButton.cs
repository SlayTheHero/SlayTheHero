using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitButton : MonoBehaviour
{
    public List<GameObject> Synergys = new List<GameObject>();
    private Image UI_UnitImage;
    void Awake()
    {
        UI_UnitImage = transform.GetChild(0).GetComponent<Image>();
    }
    public void SetSynergyData((int,int,int) SynergyCode)
    {
        Synergys[0].GetComponent<Image>().sprite = ImageDB.GetImage(ImageDB.ImageType.Synergy, SynergyCode.Item1);
        Synergys[1].GetComponent<Image>().sprite = ImageDB.GetImage(ImageDB.ImageType.Synergy, SynergyCode.Item2);
        Synergys[2].GetComponent<Image>().sprite = ImageDB.GetImage(ImageDB.ImageType.Synergy, SynergyCode.Item3);
    }
    public void SetSynergyVisible(bool isShow)
    {
        Debug.Log(isShow);
        foreach(GameObject go in Synergys) 
        {
            go.SetActive(isShow);
        }

    }

    public void SetImage(Sprite sp)
    {
        UI_UnitImage.sprite = sp;
    }
    public Image GetImage()
    {
        return UI_UnitImage;
    }

    public void SetSelected(bool isSelected) 
    {
        if(isSelected)
        {
            UI_UnitImage.color = new Color32(255, 255, 255, 100);
        }
        else
        {
            UI_UnitImage.color = new Color32(255, 255, 255, 255);
        }
    }
}
