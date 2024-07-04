using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class DialogUI : UI_Base
{
    const int MAX_BRANCH_COUNT = 4;
    public UnityEvent<PointerEventData> OnBranchSelected = new();
    public UnityEvent OnNextRequested = new();
    enum GameObjects
    {
        DialogUI, DialogPanel, DialogText, DialogBranchText, DialogBranchSelection, SpeakerName, SpeakerImage, Selection1, Selection2, Selection3, Selection4
    }
    enum Texts
    {
        DialogText, DialogBranchText, DialogBranchSelection, SpeakerName
    }
    enum Images
    {
        SpeakerImage
    }
    // Start is called before the first frame update
    void Start()
    {
        Bind<GameObject>(typeof(GameObjects));
        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));

        GetUI<GameObject>((int)GameObjects.DialogPanel).AddUIEvent((p) => { OnNextRequested.Invoke(); }, UI_EventHandler.UIEvent.LClick);
        BranchButtonAddUIEvent();
        GetUI<GameObject>((int)GameObjects.DialogBranchSelection).SetActive(false);
    }
    void BranchButtonAddUIEvent()
    {
        GetUI<GameObject>((int)GameObjects.Selection1).AddUIEvent((p) => { OnBranchSelected.Invoke(p); }, UI_EventHandler.UIEvent.LClick);
        GetUI<GameObject>((int)GameObjects.Selection2).AddUIEvent((p) => { OnBranchSelected.Invoke(p); }, UI_EventHandler.UIEvent.LClick);
        GetUI<GameObject>((int)GameObjects.Selection3).AddUIEvent((p) => { OnBranchSelected.Invoke(p); }, UI_EventHandler.UIEvent.LClick);
        GetUI<GameObject>((int)GameObjects.Selection4).AddUIEvent((p) => { OnBranchSelected.Invoke(p); }, UI_EventHandler.UIEvent.LClick);
    }
    public void SetDialog(Dialog data)
    {
        GetUI<GameObject>((int)GameObjects.DialogBranchSelection).SetActive(false);
        GetUI<GameObject>((int)GameObjects.DialogPanel).SetActive(true);
        GetUI<GameObject>((int)GameObjects.SpeakerName).GetComponent<TextMeshProUGUI>().text = data.SpeakerName;
        GetUI<GameObject>((int)GameObjects.DialogText).GetComponent<TextMeshProUGUI>().text = data.Content;
        GetImage((int)Images.SpeakerImage).sprite = Resources.Load<Sprite>(data.SpeakerName);
    }
    public void SetBranchDialog(Dialog data)
    {
        //SetDialog(data);
        GetUI<GameObject>((int)GameObjects.DialogBranchSelection).SetActive(true);
        for (int i = 0, j = (int)GameObjects.Selection1; i < MAX_BRANCH_COUNT; i++, j++)
        {
            var go = GetUI<GameObject>(j);
            if (i >= data.BranchCount && go.activeSelf) { go.SetActive(false); return; }
            go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = data.BranchContents[i];
            if (!go.activeSelf)
                go.SetActive(true);
        }
    }
    public void CloseDialog()
    {
        GetUI<GameObject>((int)GameObjects.DialogPanel).SetActive(false);
        GetUI<GameObject>((int)GameObjects.DialogBranchSelection).SetActive(false);
    }
    protected override void Init()
    {
        //throw new System.NotImplementedException();
    }
}
