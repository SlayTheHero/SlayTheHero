using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 툴팁를 위한 부모클래스입니다. 
/// Prefab 확인을 위해 InitWithName(string)을 호출해주어야 하며
/// setData() 함수를 오버라이딩 하여 데이터를 넣어주어야합니다.
/// ToolTipInstance를 접근할 수 있습니다.
/// </summary>
public class UI_BaseToolTipEventHandler : UI_EventHandler 
{ 
    protected static Dictionary<string,GameObject> ToolTipInstanceDict = new Dictionary<string,GameObject>();
    /// <summary>
    /// InitWithName 을 통해 이름 입력 후 사용합니다
    /// 해당 툴팁 종류에 맞는 GameObject입니다.
    /// </summary>
    protected GameObject ToolTipInstance;
    private RectTransform ToolTipRect;
    private string ToolTipName = "";

    /// <summary>
    /// hierarchy 정렬용 오브젝트입니다.
    /// </summary>
    private GameObject ToolTipGroupObject;
    private GameObject canvas;

    /// <summary>
    /// name에 맞는 Preafb을 Instance를 Instantiate 하여 저장한 후 Action 을 연결합니다.
    /// Prefab에 데이터를 넣은 뒤에 hierarchy를 수정합니다.
    /// </summary>
    private void Init()
    {
        canvas = transform.GetComponentInParent<Canvas>().gameObject;
        // 정렬용
        if (ToolTipGroupObject == null)
        {
            // 캔버스가 여러개일 수 있으므로 찾아서 들어갑니다.
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
        // 툴팁 프리팹
        if (ToolTipInstance == null && ToolTipName != "")
        {
            //Dict에서 찾기
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
    /// Resources/Prefabs/UI/ 안의 이름을 통해 Init 에서 Instantiate합니다.
    /// 하위 클래스의 Awake나 Start에서 항상 호출해주어야합니다.
    /// </summary>
    /// <param name="prefabName">Prefab 이름과 동일해야합니다.</param>
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
    /// 마우스 진입 이벤트입니다. 
    /// </summary>
    /// <param name="data"></param>
    private void setActiveToolTip(PointerEventData data)
    {
        if (ToolTipInstance != null)
        {
            ToolTipInstance.transform.SetParent(canvas.transform);
            ToolTipInstance.transform.position = adjustToolTipPosition(data);

            // virtual 함수
            setData();

            ToolTipInstance.SetActive(true);

        }
    }
    /// <summary>
    /// 마우스 이동 이벤트입니다. 
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
    /// 마우스 이탈 이벤트입니다. 툴팁을 비활성화 합니다.
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
    /// 툴팁이 화면 안 쪽에 위치하도록 합니다.
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