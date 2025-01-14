using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CharacterDialoguePanel : MonoBehaviour
{
    public List<TextMeshProUGUI> UI_SynergyTexts;
    public List<Image> UI_SynergyImages;
    public List<Image> UI_SkillImages;
    public Image UI_CharacterImage;
    public TextMeshProUGUI UI_CharacterName;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCharacterData(UnitBase unit)
    {
        if(unit == null)
        { 
            UI_CharacterName.text = "Name";
            UI_SynergyTexts[0].text = "Race";
            UI_SynergyTexts[1].text = "Class";
            UI_SynergyTexts[2].text = "Feature";

            UI_SynergyImages[0].sprite = ImageDB.GetImage(ImageDB.ImageType.Default, 0);
            UI_SynergyImages[1].sprite = ImageDB.GetImage(ImageDB.ImageType.Default, 0);
            UI_SynergyImages[2].sprite = ImageDB.GetImage(ImageDB.ImageType.Default, 0);


            UI_SynergyImages[0].gameObject.GetComponent<UI_SynergyToolTipEventHandler>().setSynergyID(-1);
            UI_SynergyImages[1].gameObject.GetComponent<UI_SynergyToolTipEventHandler>().setSynergyID(-1);
            UI_SynergyImages[2].gameObject.GetComponent<UI_SynergyToolTipEventHandler>().setSynergyID(-1);


            for (int i = 0; i < 4; i++)
            { 
                UI_SkillImages[i].gameObject.GetComponent<UI_SkillToolTipEventHandler>().setSkillID(-1);
                UI_SkillImages[i].sprite = ImageDB.GetImage(ImageDB.ImageType.Default, 0);
            }
            return;
        }
        UI_CharacterName.text = unit.Name;
        UI_SynergyTexts[0].text = unit.Race.ToString();
        UI_SynergyTexts[1].text = unit.Job.ToString();
        UI_SynergyTexts[2].text = unit.Feature.ToString();

        UI_SynergyImages[0].sprite = ImageDB.GetImage(ImageDB.ImageType.Synergy, (int)Synergy.FromUnitEnumToSynergy(unit.Race));
        UI_SynergyImages[1].sprite = ImageDB.GetImage(ImageDB.ImageType.Synergy, (int)Synergy.FromUnitEnumToSynergy(unit.Job));
        UI_SynergyImages[2].sprite = ImageDB.GetImage(ImageDB.ImageType.Synergy, (int)Synergy.FromUnitEnumToSynergy(unit.Feature));

        UI_SynergyImages[0].gameObject.GetComponent<UI_SynergyToolTipEventHandler>().setSynergyID((int)Synergy.FromUnitEnumToSynergy(unit.Race));
        UI_SynergyImages[1].gameObject.GetComponent<UI_SynergyToolTipEventHandler>().setSynergyID((int)Synergy.FromUnitEnumToSynergy(unit.Job));
        UI_SynergyImages[2].gameObject.GetComponent<UI_SynergyToolTipEventHandler>().setSynergyID((int)Synergy.FromUnitEnumToSynergy(unit.Feature));

        for (int i = 0; i < 4; i++)
        {
            if (i < unit.SkillList.Count)
            {
                UI_SkillImages[i].gameObject.GetComponent<UI_SkillToolTipEventHandler>().setSkillID(unit.SkillList[i].id);
                UI_SkillImages[i].sprite = ImageDB.GetImage(ImageDB.ImageType.Skill, unit.SkillList[i].id);
            }
            else
            {
                UI_SkillImages[i].gameObject.GetComponent<UI_SkillToolTipEventHandler>().setSkillID(-1);
                UI_SkillImages[i].GetComponent<Image>().sprite = null;
            }
        }

    }
}
