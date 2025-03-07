using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuffViewer : MonoBehaviour
{
    GameObject gridPanel;
    GameObject buffPrefab;
    private void OnEnable()
    {
        buffPrefab = Resources.Load<GameObject>("Prefabs/UI/Element/UI_Buff");
        gridPanel = Utility.FindChild(this.gameObject, "BuffGridPanel", true);
    }
    public void SetBuff(UnitBase unit)
    {
        if(gridPanel.transform.childCount > 0)
        {
            int count = gridPanel.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                GameObject.Destroy(gridPanel.transform.GetChild(i).gameObject);
            }
        }
        List<Skill> list = unit.BuffController.GetBuffQueue().ToList();
        for (int i = 0; i < list.Count; i++)
        {
            GameObject temp = GameObject.Instantiate(buffPrefab);
            temp.transform.SetParent(gridPanel.transform);
            temp.transform.localScale = new Vector3(1, 1, 1);
            temp.GetComponent<UI_BuffToolTipEventHandler>().setBuffID(list[i].id);
            temp.GetComponent<Image>().sprite = ImageDB.GetImage(ImageDB.ImageType.Synergy, list[i].id);
        }
    }
}
