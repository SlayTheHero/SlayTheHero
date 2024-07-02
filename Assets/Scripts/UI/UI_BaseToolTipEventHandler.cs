using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// ������ ���� �θ�Ŭ�����Դϴ�. 
/// Prefab Ȯ���� ���� InitWithName(string)�� ȣ�����־�� �ϸ�
/// setData() �Լ��� �������̵� �Ͽ� �����͸� �־��־���մϴ�.
/// ToolTipInstance�� ������ �� �ֽ��ϴ�.
/// </summary>
public class UI_BaseToolTipEventHandler : UI_EventHandler 
{ 
    protected static Dictionary<string,GameObject> ToolTipInstanceDict = new Dictionary<string,GameObject>();
    /// <summary>
    /// InitWithName �� ���� �̸� �Է� �� ����մϴ�
    /// �ش� ���� ������ �´� GameObject�Դϴ�.
    /// </summary>
    protected GameObject ToolTipInstance;
    private RectTransform ToolTipRect;
    private string ToolTipName = "";

    /// <summary>
    /// hierarchy ���Ŀ� ������Ʈ�Դϴ�.
    /// </summary>
    private GameObject ToolTipGroupObject;
    private GameObject canvas;

    /// <summary>
    /// name�� �´� Preafb�� Instance�� Instantiate �Ͽ� ������ �� Action �� �����մϴ�.
    /// Prefab�� �����͸� ���� �ڿ� hierarchy�� �����մϴ�.
    /// </summary>
    private void Init()
    {
        canvas = transform.GetComponentInParent<Canvas>().gameObject;
        // ���Ŀ�
        if (ToolTipGroupObject == null)
        {
            // ĵ������ �������� �� �����Ƿ� ã�Ƽ� ���ϴ�.
            GameObject group = canvas.transform.GetChild(canvas.transform.childCount-1).gameObject;
            if (group.name.Equals("@ToolTipGroupObject"))
            {
                ToolTipGroupObject = group;
            }else
            {
                ToolTipGroupObject = new GameObject("@ToolTipGroupObject").AddComponent<RectTransform>().gameObject;
                ToolTipGroupObject.transform.SetParent(canvas.transform);
                ToolTipGroupObject.transform.SetAsLastSibling();
            }
        }
        // ���� ������
        if (ToolTipInstance == null && ToolTipName != "")
        {
            //Dict���� ã��
            if(ToolTipInstanceDict.ContainsKey(ToolTipName))
            {
                ToolTipInstance = ToolTipInstanceDict[ToolTipName];
            }
            else
            {
                ToolTipInstance = GameObject.Instantiate(Resources.Load<GameObject>($"Prefabs/UI/{ToolTipName}"));
                ToolTipInstanceDict.Add(ToolTipName, ToolTipInstance);
            }
            if (ToolTipInstance == null)
            {
                Debug.Log($"ToolTipInstance Instnatiate Failed Resources/Prefabs/UI/{ToolTipName}");
            }
            ToolTipInstance.transform.SetParent(ToolTipGroupObject.transform);
            ToolTipRect = ToolTipInstance.GetComponent<RectTransform>();
            OnPointerEnterHandler += setActiveToolTip;
            OnPointerMoveHandler += moveToolTip;
            OnPointerExitHandler += setInActiveToolTip;

             
            ToolTipInstance.SetActive(false);
        }
    }

    /// <summary>
    /// Resources/Prefabs/UI/ ���� �̸��� ���� Init ���� Instantiate�մϴ�.
    /// ���� Ŭ������ Awake�� Start���� �׻� ȣ�����־���մϴ�.
    /// </summary>
    /// <param name="prefabName">Prefab �̸��� �����ؾ��մϴ�.</param>
    protected void InitWithName(string prefabName)
    {
        ToolTipName = prefabName;
        Init();
    }

    protected virtual void setData()
    {
        Debug.Log("base setData");
    }



    Vector2 offSet = new Vector2(5, 5);
    /// <summary>
    /// ���콺 ���� �̺�Ʈ�Դϴ�. 
    /// </summary>
    /// <param name="data"></param>
    private void setActiveToolTip(PointerEventData data)
    {
        if (ToolTipInstance != null)
        {
            ToolTipInstance.transform.SetParent(canvas.transform);
            ToolTipInstance.transform.position = adjustToolTipPosition(data);

            // virtual �Լ�
            setData();

            ToolTipInstance.SetActive(true);

        }
    }
    /// <summary>
    /// ���콺 �̵� �̺�Ʈ�Դϴ�. 
    /// </summary>
    /// <param name="data"></param>
    private void moveToolTip(PointerEventData data)
    {
        if (ToolTipInstance != null)
        {
            ToolTipInstance.transform.position = adjustToolTipPosition(data);
        }
    }
    /// <summary>
    /// ���콺 ��Ż �̺�Ʈ�Դϴ�. ������ ��Ȱ��ȭ �մϴ�.
    /// </summary>
    /// <param name="data"></param>
    private void setInActiveToolTip(PointerEventData data)
    {
        if (ToolTipInstance != null)
        {
            ToolTipInstance.SetActive(false);
            ToolTipInstance.transform.SetParent(ToolTipGroupObject.transform);
        }
    }

    /// <summary>
    /// ������ ȭ�� �� �ʿ� ��ġ�ϵ��� �մϴ�.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private Vector2 adjustToolTipPosition(PointerEventData data)
    {
        Vector2 newPosition = data.position + offSet + new Vector2(ToolTipRect.rect.width / 2, ToolTipRect.rect.height); 
        if (newPosition.x + ToolTipRect.rect.width / 2 > Screen.width)
        {
            newPosition.x = Screen.width - ToolTipRect.rect.width/2;
        }
        if(newPosition.y + ToolTipRect.rect.height / 2 > Screen.height)
        {
            newPosition.y = data.position.y - offSet.y - ToolTipRect.rect.height;
        }
        return newPosition;
    }

    private void OnDestroy()
    {
        if(ToolTipInstanceDict.ContainsKey(ToolTipName))
        {
            ToolTipInstanceDict.Remove(ToolTipName);
        }
    }
}