using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Skill : UI_Base
{
    enum Images
    {
        SkillImage
    }
    [SerializeField]
    int m_skill_idx;
    private void Start()
    {
        Init();
    }
    protected override void Init()
    {
        Bind<Image>(typeof(Images));
    }
    public UI_Skill SetInfo(int skill_idx)
    {
        m_skill_idx = skill_idx;
        return this;
    }
    public void RefreshUI()
    {
        if (BattleManager.Instance.CurUnit.SkillList.Count <= m_skill_idx)
        {
            gameObject.SetActive(false);
            return;
        }
        GetImage((int)Images.SkillImage).sprite = ImageDB.GetImage(ImageDB.ImageType.Skill, BattleManager.Instance.CurUnit.SkillList[m_skill_idx].id);
        GetComponent<UI_EventHandler>().enabled = BattleManager.Instance.CurUnit.IsPlayerUnit;
    }

}
