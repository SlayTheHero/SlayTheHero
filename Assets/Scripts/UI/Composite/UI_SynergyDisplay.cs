using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_SynergyDisplay : MonoBehaviour
{
    public List<GameObject> elements = new List<GameObject>();
    public List<UI_SynergyToolTipEventHandler> handlers = new List<UI_SynergyToolTipEventHandler>();
    List<(int,bool)> nowSynergy = new List<(int, bool)>();
    // Using BattleManager's PlayerTeam, Apply to Its Display 
    // if input especial unitList, Apply that list's synergy
    public void InitializeDisplay(List<UnitBase> unitLists = null)
    {
        if(unitLists == null)
        {
            IEnumerable<UnitBase> units = BattleManager.Instance.PlayerTeam;
            nowSynergy = SynergyDB.getSynergyFromUnitList(units.ToList());
            SetDisplay();
        }
        else
        {
            nowSynergy = SynergyDB.getSynergyFromUnitList(unitLists.ToList());
            SetDisplay();
        }
    }

    private void SetDisplay()
    {
        for (int i = 0; i < elements.Count; i++)
        {
            GameObject nowElement = elements[i];
            nowElement.SetActive(true);
            nowElement.transform.GetChild(0).GetComponent<Image>().sprite = ImageDB.GetImage(ImageDB.ImageType.Default, 0);
            nowElement.transform.GetChild(1).GetComponent<Image>().sprite = ImageDB.GetImage(ImageDB.ImageType.Default, 0);
            handlers[i].setSynergyID(-1);
        }
        if (nowSynergy == null || nowSynergy.Count == 0)
        {
            for (int i = 0; i < elements.Count; i++)
            {
                elements[i].SetActive(false);
            }
            return;
        }
        int index = 0;

        for (int i = 0; i < nowSynergy.Count; i++)
        {
            index = i;
            GameObject nowElement = elements[i];
            nowElement.transform.GetChild(0).GetComponent<Image>().sprite = ImageDB.GetImage(ImageDB.ImageType.Synergy, nowSynergy[i].Item1);
            nowElement.transform.GetChild(1).GetComponent<Image>().sprite = nowSynergy[i].Item2 ? ImageDB.GetImage(ImageDB.ImageType.Synergy, 12) : ImageDB.GetImage(ImageDB.ImageType.Synergy, 11);
            handlers[i].setSynergyID(nowSynergy[i].Item1);
        }
        if (index < elements.Count - 1)
        {
            for (int i = index + 1; i < elements.Count; i++)
            {
                elements[i].SetActive(false);
            }
        }
    }
}
