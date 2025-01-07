using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SynergyOutputPanel : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 1f; // 회전 속도
    [SerializeField] float radius = 1f; // 회전 반경
    [SerializeField] float cameraLen = 1f; // 카메라 거리 조절 값
    [SerializeField] float theta = 0f; // 초기 각도
    [SerializeField] Vector2 baseLen = new Vector2(0.15f, 0.4f); // 기본 크기 
    [SerializeField] float distanceAmplitude = 0.2f; // 거리 진폭
    [SerializeField] float leftRightAmplitude = 0.3f; // 왼쪽-오른쪽 움직임 진폭

    
    List<GameObject> cars = new List<GameObject>();
    List<RectTransform> rects = new List<RectTransform>();

    public int nowSelect = 0; 
    List<int> diffs = new List<int>() { 0, 0, 0 };
    List<int> nowThetas = new List<int>() { 0, 90, 270 };
    
    public void Clear()
    {
        nowThetas = new List<int>() { 0, 90, 270 }; 
        diffs = new List<int>() { 0, 0, 0 };
        isMoving = false;
        nowSelect = 0; count = 0;
        Rotation(0);
        Rotation(1);
        Rotation(2);
    }
    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            cars.Add(transform.GetChild(i).gameObject);
            rects.Add(cars[i].GetComponent<RectTransform>());
            cars[i].AddUIEvent(OnCardClicked, UI_EventHandler.UIEvent.LClick);
        }
        Clear();
    }
    bool isMoving = false;
    int count = 0;
    int speed = 3;
    void Update()
    {
        if(isMoving)
        {
            if(count < 90)
            {
                for (int i = 0; i < 3; i++)
                {
                    nowThetas[i] = (nowThetas[i] + diffs[i] * speed + 360) % 360;
                }
                for (int i = 0; i < 3; i++)
                {
                    Rotation(i);
                }
                count += speed;
            }
            else
            {
                isMoving = false;
            }
        }
    }

    void OnCardClicked(PointerEventData data)
    {
        if (isMoving) return;

        GameObject go = data.selectedObject;
        int index = int.Parse(go.name.Split("_")[2]) - 1;
        Debug.Log(index + " clkicked");
        int Diff = index - nowSelect;
        if(index == nowSelect)
        {
            // 확정
            return;
        }

        count = 0;
        if (Mathf.Abs(Diff) == 2)
        {
            Diff = Diff == 2 ? -1 : 1;
        }

        // 더해야함. 시계방향 회전
        if (Diff < 0)
        {
            for (int i = 0; i < 3; i++)
            {
                // 멀리 이동하는 카드
                if (i == 3 - index - nowSelect)
                {
                    diffs[i] = 2;
                    
                }
                // 정면에서 왼쪽으로
                else if (i == nowSelect)
                {
                    diffs[i] = 1;
                }
                // 오른쪽에서 정면으로. ** 360 넘어감.
                else
                {
                    diffs[i] = 1;
                }
            }
        }
        // 뻬야함. 시계반대방향 회전
        else
        {
            for (int i = 0; i < 3; i++)
            {
                // 멀리 이동하는 카드
                if (i == 3 - index - nowSelect)
                {
                    diffs[i] = -2;
                }
                // 정면에서 오른쪽으로 ** 360 넘어감
                else if (i == nowSelect)
                {
                    diffs[i] = -1;
                }
                // 왼쪽에서 정면으로.  
                else
                {
                    diffs[i] = -1;
                }
            }
        }
        // 순서 재배열 
        cars[3 - index - nowSelect].transform.SetParent(null);
        cars[nowSelect].transform.SetParent(null);
        cars[index].transform.SetParent(null);
        cars[3 - index - nowSelect].transform.SetParent(transform);
        cars[nowSelect].transform.SetParent(transform);
        cars[index].transform.SetParent(transform);

        nowSelect = index;
        isMoving = true;
    }

    void Rotation(int index)
    {
        float frontDistance = cameraLen + radius * distanceAmplitude - distanceAmplitude * Mathf.Cos(nowThetas[index] * Mathf.Deg2Rad);
        float leftCenter = 0.5f - leftRightAmplitude * Mathf.Sin(nowThetas[index] * Mathf.Deg2Rad);

        Vector2 anchorMax = new Vector2();
        Vector2 anchorMin = new Vector2();

        anchorMin.x = leftCenter - (baseLen.x / frontDistance);
        anchorMax.x = leftCenter + (baseLen.x / frontDistance);
        anchorMin.y = 0.5f - (baseLen.y / frontDistance);
        anchorMax.y = 0.5f + (baseLen.y / frontDistance);

        applyToRect(index,anchorMin, anchorMax);
    }

    void applyToRect(int index, Vector2 anchorMin, Vector2 anchorMax)
    {
        rects[index].anchorMin = anchorMin;
        rects[index].anchorMax = anchorMax;
    }
}
