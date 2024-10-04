using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Diagnostics;
using static UnityEditor.Progress;

/// <summary>
/// ĵ������ �����մϴ�. ShowPopupUI�� ShowSceneUI�� ���� ĵ������ �����ϰ� �����մϴ�.
/// 
/// </summary>
public class UIManager
{
    #region Public

    public UIManager()
    {
    }

    /// <summary>
    /// �˾� ĵ������ Instantiate�մϴ�. CloseUI�� ���������� ���� ĵ�������� �����ϴ�.
    /// </summary>
    /// <typeparam name="T">ĵ������ ������Ʈ�� �ִ� ��ũ��Ʈ�� �̸��Դϴ�. �̴� ĵ������ �̸��� ���ƾ��մϴ�.</typeparam>
    /// <param name="name">ĵ������ �̸��Դϴ�. Ȯ�ο��Դϴ�. ���� �ʾƵ� �˴ϴ�.</param>
    /// <returns></returns>
    public T ShowPopupUI<T>(string name = null) where T : UI_Base
    {
        return ShowUI<T>(name, true);
    }
    /// <summary>
    /// �⺻ ĵ������ Instantiate�մϴ�. CloseUI�� ������ ������ Clear�θ� ������ϴ�.
    /// </summary>
    /// <typeparam name="T">ĵ������ ������Ʈ�� �ִ� ��ũ��Ʈ�� �̸��Դϴ�. �̴� ĵ������ �̸��� ���ƾ��մϴ�.</typeparam>
    /// <param name="name">ĵ������ �̸��Դϴ�. Ȯ�ο��Դϴ�. ���� �ʾƵ� �˴ϴ�.</param>
    /// <returns></returns>
    public T ShowSceneUI<T>(string name = null) where T : UI_Base
    {
        return ShowUI<T>(name, false);
    }


    /// <summary>
    ///  ���� ������ �˾��� �����մϴ�.
    /// </summary>
    public void ClosePopupUI()
    {
        if (PopupUIStack.Count == 0) return;

        GameObject popup = PopupUIStack.Pop().gameObject;
        GameObject.Destroy(popup.gameObject);
        PopupOrder--;
    }
    /// <summary>
    /// ��� �˾��� �����մϴ�.
    /// </summary>
    public void CloseAllPopupUI()
    {
        while (PopupUIStack.Count > 0)
        {
            UI_Base popup = PopupUIStack.Pop();
            GameObject.Destroy(popup.gameObject);
        }
        PopupOrder = 10;
    }
    /// <summary>
    /// ��� �˾��� �⺻ UI�� �����մϴ�.
    /// </summary>
    public void ClearUIManger()
    {
        CloseAllPopupUI();
        CloseAllSceneUI();
    }
    #endregion Public

    #region Private


    private int PopupOrder = 10;
    private int SceneOrder = 0;
    private Stack<UI_Base> PopupUIStack = new Stack<UI_Base>();
    private List<UI_Base> SceneUIList = new List<UI_Base>();
    /// <summary>
    /// ĵ���� ������ �����մϴ�. sort�� false�Ͻ� �ʼ����� UI�� ���ֵǾ� ClosePopupUI�� ������ �ʽ��ϴ�.
    /// �ʼ� UI�� 10�������Դϴ�.
    /// </summary>
    /// <param name="go"> Canvas GameObject</param>
    /// <param name="sort"> �˾� ������. �⺻�� true.</param>
    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Utility.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;
        //�˾���
        if (sort)
        {
            canvas.sortingOrder = PopupOrder;
            PopupOrder++;
        }
        else
        {
            canvas.sortingOrder = SceneOrder;
            SceneOrder++;
        }

    }

    GameObject ui_Root;
    GameObject UI_Root
    {
        get
        {
            //����
            if (ui_Root == null)
            {
                ui_Root = GameObject.Find("$UI_Root");
                if (ui_Root == null)
                    ui_Root = new GameObject { name = "$UI_Root" };
            }

            return ui_Root;
        }
    }







    /// <summary>
    /// Ŭ������ �̸��� ���� ĵ���� �������� �����ͼ� Instantiate
    /// </summary>
    /// <typeparam name="T">UI_Base�� ��ӹ��� Ŭ����. �����հ� �̸��� ���ƾ��� </typeparam>
    /// <param name="name">�������� �̸�. �⺻�� null</param>
    /// <returns></returns>
    private T ShowUI<T>(string name = null, bool sort = true) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;
        GameObject prefab = Resources.Load<GameObject>($"Prefabs/UI/Canvas/{name}");
        GameObject go = GameObject.Instantiate(prefab);
        SetCanvas(go, sort);
        T scene = Utility.GetOrAddComponent<T>(go);
        if (sort)
        {
            PopupUIStack.Push(scene);
        }
        else
        {
            SceneUIList.Add(scene);
        }
        go.transform.SetParent(UI_Root.transform);
        return scene;
    }


    /// <summary>
    /// Clear�� �����Լ��Դϴ�.
    /// </summary>
    private void CloseAllSceneUI()
    {
        for (int i = 0; i < SceneUIList.Count; i++)
        {
            UI_Base scene = SceneUIList[i];
            GameObject.Destroy(scene.gameObject);
        }
        SceneUIList.Clear();
        SceneOrder = 0;
    }

    #endregion Private

}
