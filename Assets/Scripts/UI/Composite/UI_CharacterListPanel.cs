using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; 

public class UI_CharacterListPanel : UI_EventHandler
{
    List<GameObject> UnitList;
    GameManager manager;
    GameObject GridPanel;
    public void LoadPlayerData()
    {
        if (manager == null)
            manager = GameManager.getInstance();
        UnitList = new List<GameObject>();
        int unitCount = manager.PlayerData.unitDeque.GetUnitCount();
        GridPanel = transform.Find("UI_CharacterListGridPanel").gameObject;
        if(GridPanel.transform.childCount > 0 )
        {
            foreach (Transform child in GridPanel.transform)
            {
                if (child.name == GridPanel.name) continue;
                Image nowIm = child.GetComponent<Image>();
                nowIm.sprite = null;
                GameObject.Destroy(child.gameObject);
            }
        }
        GameObject prefab = Resources.Load<GameObject>("Prefabs/UI/Element/UI_Button");
        for (int i = 0; i < unitCount; i++)
        {
            GameObject temp = GameObject.Instantiate(prefab, GridPanel.transform);
            temp.name = "UI_Character_" + i;
            temp.GetComponent<Image>().sprite = ImageDB.GetImage(ImageDB.ImageType.Unit, manager.PlayerData.unitDeque.GetUnit(i).ID);
            UnitList.Add(temp);
        }
    }
    public void SetUnitEvent(Action<PointerEventData> ev, UI_EventHandler.UIEvent type)
    {
        for (int i = 0; i < UnitList.Count; i++)
        {
            UnitList[i].AddUIEvent(ev, type);
        }
    }

    public void SetUnitSelected(int index, bool isGrey)
    {
        if (UnitList.Count <= index) return;
        if(isGrey)
        { 
            UnitList[index].GetComponent<Image>().color = new Color32(255, 255, 255, 100);
        }
        else
        { 
            UnitList[index].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }


}
