using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitButton : MonoBehaviour
{
    public List<GameObject> Synergys = new List<GameObject>();
    void Start()
    { 
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
}
