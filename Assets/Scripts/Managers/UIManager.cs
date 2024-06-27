using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Diagnostics;
using static UnityEditor.Progress;

/// <summary>
/// UI ����, ���� ���� - ���� �˾� ������
/// </summary>
public class UIManager
{
    int _order = 10;
    List<UI_Base> UIList = new List<UI_Base>();
    /// <summary>
    /// ���� ĵ���� ���� UI_Base ��ӹ��� Init���� ȣ��.
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
            canvas.sortingOrder = _order;
            _order++;
        }
        else
        {
            canvas.sortingOrder = 0;
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
    /// Ŭ������ �̸��� ���� �������� �����ͼ� Instantiate
    /// </summary>
    /// <typeparam name="T">UI_Base�� ��ӹ��� Ŭ����. �����հ� �̸��� ���ƾ��� </typeparam>
    /// <param name="name">�������� �̸�. �⺻�� null</param>
    /// <returns></returns>
    public T ShowUI<T>(string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;
        GameObject prefab = Resources.Load<GameObject>($"Prefabs/UI/{name}");
        GameObject go = GameObject.Instantiate(prefab);
        T scene = Utility.GetOrAddComponent<T>(go);
        UIList.Add(scene);

        go.transform.SetParent(UI_Root.transform);
        return scene;
    }
}
