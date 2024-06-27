using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Diagnostics;
using static UnityEditor.Progress;

/// <summary>
/// UI 생성, 제거 전담 - 차후 팝업 관리용
/// </summary>
public class UIManager
{
    int _order = 10;
    List<UI_Base> UIList = new List<UI_Base>();
    /// <summary>
    /// 씬별 캔버스 안의 UI_Base 상속받은 Init에서 호출.
    /// </summary>
    /// <param name="go"> Canvas GameObject</param>
    /// <param name="sort"> 팝업 관리용. 기본은 true.</param>
    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Utility.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;
        //팝업용
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
            //정리
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
    /// 클래스나 이름을 통해 프리팹을 가져와서 Instantiate
    /// </summary>
    /// <typeparam name="T">UI_Base를 상속받은 클래스. 프리팹과 이름이 같아야함 </typeparam>
    /// <param name="name">프리팹의 이름. 기본은 null</param>
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
