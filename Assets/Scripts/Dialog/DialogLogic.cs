using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DialogLogic : MonoBehaviour
{
    DialogDB dialogDB;
    DialogUI dialogUI;
    Dialog cur_dialog;
    bool is_first = true;
    int cur_index;
    int cur_context_index = 0;
    string cur_context;
    public UnityEvent<Dialog> OnDialogFinish = new UnityEvent<Dialog>();
    private void Awake()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        dialogDB = GetComponent<DialogDB>();
        dialogUI = GetComponent<DialogUI>();
        cur_context = dialogDB.DialogContextList[0].name;
        cur_index = 0;
        dialogUI.OnBranchSelected.AddListener(JumpDialog);
        dialogUI.OnNextRequested.AddListener(NextDialog);

    }
    private void Update()
    {
        if (is_first)
        {
            StartDialog(cur_context);
            is_first = false;
        }
    }

    public void StartDialog(string context)
    {
        StartDialog(context, 0);
    }
    public void StartDialog(string context, int index)
    {
        cur_context = context;
        cur_index = index;
        cur_dialog = dialogDB.DialogTable[cur_context][index];

        if (dialogDB.DialogTable[cur_context][cur_index].BranchCount == 0)
            dialogUI.SetDialog(cur_dialog);
        else
            dialogUI.SetBranchDialog(cur_dialog);
    }

    void NextDialog()
    {
        if (!string.IsNullOrEmpty(cur_dialog.EndOfDialog))
        {
            FinishDialog();
            return;
        }
        if (!string.IsNullOrEmpty(cur_dialog.ContextSwitch))
        {
            StartDialog(cur_dialog.ContextSwitch, cur_dialog.Links[0]);
            return;
        }
        if (cur_dialog.Links[0] == 0)
            StartDialog(cur_context, cur_index + 1);
        else
            StartDialog(cur_context, cur_dialog.Links[0]);

    }
    void JumpDialog(PointerEventData eventData)
    {
        var btnIndex = eventData.pointerClick.gameObject.transform.GetSiblingIndex();
        var link = cur_dialog.Links[btnIndex];
        StartDialog(cur_context, link);
    }
    void FinishDialog()
    {
        dialogUI.CloseDialog();
        cur_context_index++;
        if (dialogDB.DialogContextList.Count <= cur_context_index)
        {
            OnDialogFinish.Invoke(cur_dialog);
            return;
        }
        cur_context = dialogDB.DialogContextList[cur_context_index].name;
        cur_index = 0;
        OnDialogFinish.Invoke(cur_dialog);

    }
}
