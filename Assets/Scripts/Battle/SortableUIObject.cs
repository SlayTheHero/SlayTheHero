using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortableUIObject : MonoBehaviour, IComparable<SortableUIObject>
{
    private RectTransform m_RectTransform;
    public int PrevOrder;
    public int Order;
    public IComparable ComparableData;
    // Start is called before the first frame update
    void Start()
    {
        m_RectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OrderChange(int order)
    {
        PrevOrder = Order;
        Order = order;
    }
    public void MoveByOrder()
    {
        if (Order == 0)
        {
            m_RectTransform.DOAnchorPosX(-190, 0.5f);
            m_RectTransform.DOSizeDelta(new Vector3(230f, 210f, 0f), 0.5f);
            return;
        }
        if (PrevOrder == 0)
            m_RectTransform.DOSizeDelta(new Vector3(140f, 140f, 0f), 0.5f);
        m_RectTransform.DOAnchorPosX(((Order - 1) * 150f) + 90f, 0.5f);
    }
    public void SizeReset()
    {
        if (Order != 0)
            m_RectTransform.sizeDelta = new Vector3(140f, 140f, 0f);
        else
            m_RectTransform.sizeDelta = new Vector3(230f, 210f, 0f);
        return;

    }
    public void Remove()
    {
        m_RectTransform.DOSizeDelta(Vector2.zero, 0.5f).OnComplete(() => gameObject.SetActive(false));
    }

    public int CompareTo(SortableUIObject other)
    {
        return ComparableData.CompareTo(other.ComparableData);
    }
}
