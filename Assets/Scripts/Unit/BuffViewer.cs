using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BuffViewer : MonoBehaviour
{
    GameObject buffPrefab;
    private void Awake()
    {
        buffPrefab = Resources.Load<GameObject>("Prefabs/UI/Element/UI_Buff");
    }
    public void SetBuff(UnitBase unit)
    {
        List<Skill> list = unit.BuffController.GetBuffQueue().ToList();
        for (int i = 0; i < list.Count; i++)
        {
            GameObject temp = GameObject.Instantiate(buffPrefab);
            temp.transform.parent = this.gameObject.transform;
            temp.transform.localScale = new Vector3(1, 1, 1);
            temp.GetComponent<UI_BuffToolTipEventHandler>().setBuffID(list[i].id);
            temp.GetComponent<Image>().sprite = ImageDB.GetImage(ImageDB.ImageType.Synergy, list[i].id);
        }
    }
}
