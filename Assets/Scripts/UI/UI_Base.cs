using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// UI �θ� �Ǵ� Ŭ����. ������ ��ӹ޾� ����Ͻø� �˴ϴ�.
/// Hierarchy�� �ִ� ĵ������ �ֽ��ϴ�.
/// </summary>
public abstract class UI_Base : MonoBehaviour
{
    /// <summary>
    /// ����� Dictionary ���� �������� �ʽ��ϴ�.
    /// </summary>
    private Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();
    protected abstract void Init();

    /// <summary>
    /// Type�� Enum ���� ������ Enum ����� string���� ��ȯ�Ͽ� �ڽ��߿� ������Ʈ�� �ִٸ� �����մϴ�.
    /// ���Ŀ� GetUI ������ ����� ���� Dictionary���� �޾ƿɴϴ�.
    /// </summary>
    /// <typeparam name="T"> ���ϴ� Ÿ��(Image, Text, GameObject etc..) </typeparam>
    /// <param name="type"> typeof(Enum) ���·� ���ڿ� �����մϴ�. </param>
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
    /// Enum ���� �̿��� Enum ���� �ش��ϴ� ������Ʈ�� �����ɴϴ�.
    /// </summary>
    /// <typeparam name="T"> ������Ʈ ���� </typeparam>
    /// <param name="index">(int)enum ���� ���ڿ� �ֽ��ϴ�.</param>
    /// <returns></returns>
    protected T GetUI<T>(int index) where T : UnityEngine.Object
    {
        if (_objects[typeof(T)] == null)
            return null;

        return _objects[typeof(T)][index] as T;
    }
    /// <summary>
    /// Enum ���� �̿��� Enum ���� �ش��ϴ� �ؽ�Ʈ�� �����ɴϴ�.
    /// </summary>
    /// <param name="index">(int)enum ���� ���ڿ� �ֽ��ϴ�.</param>
    /// <returns></returns>
    public Text GetText(int index) { return GetUI<Text>(index); }
    /// <summary>
    /// Enum ���� �̿��� Enum ���� �ش��ϴ� ��ư�� �����ɴϴ�.
    /// </summary>
    /// <param name="index">(int)enum ���� ���ڿ� �ֽ��ϴ�.</param>
    /// <returns></returns>
    public Button GetButton(int index) { return GetUI<Button>(index); }
    /// <summary>
    /// Enum ���� �̿��� Enum ���� �ش��ϴ� �̹����� �����ɴϴ�.
    /// </summary>
    /// <param name="index">(int)enum ���� ���ڿ� �ֽ��ϴ�.</param>
    /// <returns></returns>
    public Image GetImage(int index) { return GetUI<Image>(index); } 
    /// <summary>
    /// Enum ���� �̿��� Enum ���� �ش��ϴ� GameObject�� �����ɴϴ�.
    /// </summary>
    /// <param name="index">(int)enum ���� ���ڿ� �ֽ��ϴ�.</param>
    /// <returns></returns>
    public GameObject GetGameObject(int index) { return GetUI<GameObject>(index); }

    /// <summary>
    /// Get ���� ���� GameObject�� UI Event�� �����մϴ�.
    /// </summary>
    /// <param name="go">�ڽ� ������Ʈ</param>
    /// <param name="action">PointerEventData�� ���ڷ� ������ Action</param>
    /// <param name="type"> �̺�Ʈ Ÿ�� </param>
    public static void BindUIEvent(GameObject go, Action<PointerEventData> action, UI_EventHandler.UIEvent type = UI_EventHandler.UIEvent.LClick)
    {

        UI_EventHandler handler = Utility.GetOrAddComponent<UI_EventHandler>(go);
        switch (type)
        {
            case UI_EventHandler.UIEvent.LClick:
                handler.OnClickHandler = action;
                break;
            case UI_EventHandler.UIEvent.LDrag:
                handler.OnDragHandler = action;
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
