using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class UI_Turn : UI_Base
{
    enum Texts
    {
        PositionText
    }
    enum Images
    {
        UnitImage
    }
    public int Position;
    int m_PrevOrder;
    public int Order;

    private RectTransform m_RectTransform;

    protected override void Init()
    {
        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));

        m_RectTransform = GetComponent<RectTransform>();

    }
    private void Start()
    {
        Init();
    }
    public UI_Turn SetInfo(int position)
    {
        GetImage((int)Images.UnitImage).sprite = ImageDB.GetImage(ImageDB.ImageType.Unit, BattleManager.Instance.Units[position].GetComponent<UnitController>().Unit.ID);
        GetUI<TextMeshProUGUI>((int)Texts.PositionText).text = position.ToString();
        Position = position;
        return this;
    }
    public void OrderChange(int order)
    {
        m_PrevOrder = Order;
        Order = order;
        MoveByOrder();
    }
    void MoveByOrder()
    {
        if (Order == 0)
        {
            m_RectTransform.DOAnchorPosX(-190, 0.5f);
            m_RectTransform.DOSizeDelta(new Vector3(230f, 210f, 0f), 0.5f);
            return;
        }
        if (m_PrevOrder == 0)
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
    public void DisapearAndDisable()
    {
        m_RectTransform.DOSizeDelta(Vector2.zero, 0.5f).OnComplete(() => gameObject.SetActive(false));
    }
    public void RefreshUI()
    {

    }

}
