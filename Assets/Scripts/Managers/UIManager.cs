using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Diagnostics;
using static UnityEditor.Progress;

/// <summary>
/// 캔버스를 관리합니다. ShowPopupUI와 ShowSceneUI를 통해 캔버스를 생성하고 해제합니다.
/// 
/// </summary>
public class UIManager
{
    #region Public

    public UIManager()
    {
    }

    /// <summary>
    /// 팝업 캔버스를 Instantiate합니다. CloseUI로 마지막으로 열린 캔버스부터 닫힙니다.
    /// </summary>
    /// <typeparam name="T">캔버스의 컴포넌트로 있는 스크립트의 이름입니다. 이는 캔버스와 이름이 같아야합니다.</typeparam>
    /// <param name="name">캔버스의 이름입니다. 확인용입니다. 넣지 않아도 됩니다.</param>
    /// <returns></returns>
    public T ShowPopupUI<T>(string name = null) where T : UI_Base
    {
        return ShowUI<T>(name, true);
    }
    /// <summary>
    /// 기본 캔버스를 Instantiate합니다. CloseUI로 닫히지 않으며 Clear로만 사라집니다.
    /// </summary>
    /// <typeparam name="T">캔버스의 컴포넌트로 있는 스크립트의 이름입니다. 이는 캔버스와 이름이 같아야합니다.</typeparam>
    /// <param name="name">캔버스의 이름입니다. 확인용입니다. 넣지 않아도 됩니다.</param>
    /// <returns></returns>
    public T ShowSceneUI<T>(string name = null) where T : UI_Base
    {
        return ShowUI<T>(name, false);
    }


    /// <summary>
    ///  가장 상위의 팝업을 삭제합니다.
    /// </summary>
    public void ClosePopupUI()
    {
        if (PopupUIStack.Count == 0) return;

        GameObject popup = PopupUIStack.Pop().gameObject;
        GameObject.Destroy(popup.gameObject);
        PopupOrder--;
    }
    /// <summary>
    /// 모든 팝업을 삭제합니다.
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
    /// 모든 팝업과 기본 UI를 삭제합니다.
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
    /// 캔버스 설정을 조정합니다. sort가 false일시 필수적인 UI로 간주되어 ClosePopupUI로 닫히지 않습니다.
    /// 필수 UI는 10개까지입니다.
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
    /// 클래스나 이름을 통해 캔버스 프리팹을 가져와서 Instantiate
    /// </summary>
    /// <typeparam name="T">UI_Base를 상속받은 클래스. 프리팹과 이름이 같아야함 </typeparam>
    /// <param name="name">프리팹의 이름. 기본은 null</param>
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
    /// Clear용 내부함수입니다.
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
