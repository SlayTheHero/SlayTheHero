using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Unity.VisualScripting;

public readonly struct Dialog
{
    public readonly int Index;
    public readonly string SpeakerName;
    public readonly string Content;
    public readonly int BranchCount;
    public readonly string[] BranchContents;
    public readonly int[] Links;
    public readonly string ContextSwitch;
    public readonly string EndOfDialog;
    public Dialog(int index, string speakerName, string content, int branchCount, string[] branchContent, int[] link, string contextSwitch, string endOfDialog)
    {
        this.Index = index;
        this.SpeakerName = speakerName;
        this.Content = content;
        this.BranchCount = branchCount;
        this.BranchContents = branchContent;
        this.Links = link;
        this.ContextSwitch = contextSwitch;
        this.EndOfDialog = endOfDialog;
    }
}
public class DialogDB : MonoBehaviour
{
    const string TEST_PATH = "DialogTest";
    private Dictionary<string, List<Dialog>> dialogTable;
    [SerializeField]
    public List<TextAsset> DialogContextList;
    public IReadOnlyDictionary<string, List<Dialog>> DialogTable
    {
        get { return dialogTable; }
    }

    private void Awake()
    {
        dialogTable = new Dictionary<string, List<Dialog>>();
        LoadAll();
    }
    public void LoadAll()
    {
        foreach (var item in DialogContextList)
        {
            dialogTable[item.name] = CSVReader.ReadDialog(item);
        }

    }
    /// <summary>
    /// �����̸��� ���ڷ� ��ȭ�� �ε�.
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns>�̹� �ε�� �����̶�� false ��ȯ.</returns>
    public bool Load(string fileName)
    {
        if (dialogTable.ContainsKey(fileName))
            return false;
        dialogTable[fileName] = CSVReader.ReadDialog(Resources.Load<TextAsset>(fileName));
        return true;

    }
    /// <summary>
    /// DialogTable�� ������ Ŭ����
    /// </summary>
    /// <returns></returns>
    public void Clear()
    {
        dialogTable.Clear();

    }


}