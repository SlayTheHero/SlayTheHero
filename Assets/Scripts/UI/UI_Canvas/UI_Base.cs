using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// UI 부모가 되는 클래스. 씬별로 상속받아 사용하시면 됩니다.
/// Hierarchy에 있는 캔버스에 넣습니다.
/// </summary>
public abstract class UI_Base : MonoBehaviour
{
    /// <summary>
    /// 저장용 Dictionary 직접 접근하지 않습니다.
    /// </summary>
    private Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();
    protected abstract void Init();

    /// <summary>
    /// Type에 Enum 값을 넣으면 Enum 목록을 string으로 변환하여 자식중에 오브젝트가 있다면 연결합니다.
    /// 이후에 GetUI 를통해 연결된 값을 Dictionary에서 받아옵니다.
    /// </summary>
    /// <typeparam name="T"> 원하는 타입(Image, Text, GameObject etc..) </typeparam>
    /// <param name="type"> typeof(Enum) 형태로 인자에 전달합니다. </param>
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
            {
                objects[i] = Utility.FindChild(gameObject, names[i], true);
            }
            else
            {
                objects[i] = Utility.FindChild<T>(gameObject, names[i], true);
            }
        }
        _objects.Add(typeof(T), objects);
    }
    /// <summary>
    /// Enum 값을 이용해 Enum 값에 해당하는 컴포넌트를 가져옵니다.
    /// </summary>
    /// <typeparam name="T"> 컴포넌트 종류 </typeparam>
    /// <param name="index">(int)enum 값을 인자에 넣습니다.</param>
    /// <returns></returns>
    protected T GetUI<T>(int index) where T : UnityEngine.Object
    {
        if (!_objects.ContainsKey(typeof(T)))
            return null;

        return _objects[typeof(T)][index] as T;
    }
    /// <summary>
    /// Enum 값을 이용해 Enum 값에 해당하는 텍스트를 가져옵니다.
    /// (Text)가 아닌 TextMeshProUGUI를 사용하므로 사용할 수 없습니다
    /// </summary>
    /// <param name="index">(int)enum 값을 인자에 넣습니다.</param>
    /// <returns></returns>
    public Text GetText(int index) { return GetUI<Text>(index); }
    /// <summary>
    /// Enum 값을 이용해 Enum 값에 해당하는 버튼을 가져옵니다.
    /// </summary>
    /// <param name="index">(int)enum 값을 인자에 넣습니다.</param>
    /// <returns></returns>
    public Button GetButton(int index) { return GetUI<Button>(index); }
    /// <summary>
    /// Enum 값을 이용해 Enum 값에 해당하는 이미지를 가져옵니다.
    /// </summary>
    /// <param name="index">(int)enum 값을 인자에 넣습니다.</param>
    /// <returns></returns>
    public Image GetImage(int index) { return GetUI<Image>(index); } 
    /// <summary>
    /// Enum 값을 이용해 Enum 값에 해당하는 GameObject를 가져옵니다.
    /// </summary>
    /// <param name="index">(int)enum 값을 인자에 넣습니다.</param>
    /// <returns></returns>
    public GameObject GetGameObject(int index) { return GetUI<GameObject>(index); }
    /// <summary>
    /// Enum 값을 이용해 Enum 값에 해당하는 TextMeshProUGUI를 가져옵니다.
    /// </summary>
    /// <param name="index">(int)enum 값을 인자에 넣습니다.</param>
    /// <returns></returns>
    public TextMeshProUGUI GetTextMeshPro(int index) { return GetUI<TextMeshProUGUI>(index); }

    /// <summary>
    /// Get 으로 얻은 GameObject에 UI Event를 연결합니다.
    /// </summary>
    /// <param name="go">자식 오브젝트</param>
    /// <param name="action">PointerEventData를 인자로 가지는 Action</param>
    /// <param name="type"> 이벤트 타입 </param>
    public static void BindUIEvent(GameObject go, Action<PointerEventData> action, UI_EventHandler.UIEvent type = UI_EventHandler.UIEvent.LClick)
    {

        UI_EventHandler handler = Utility.GetOrAddComponent<UI_EventHandler>(go);
        switch (type)
        {
            case UI_EventHandler.UIEvent.LClick:
                handler.OnClickHandler = action; 
                break;
            case UI_EventHandler.UIEvent.Enter:
                handler.OnPointerEnterHandler = action;
                break;
            case UI_EventHandler.UIEvent.Exit:
                handler.OnPointerExitHandler = action;
                break;
        }
    }
}
