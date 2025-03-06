using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitObject : MonoBehaviour
{
    public Slider HPBar;
    public BuffViewer BuffViewer;
    public UnitBase Unit;
    // Start is called before the first frame update
    void Start()
    {
        BuffViewerRefresh();

        if (Unit.IsPlayerUnit)
        {
            HPBar.GetComponent<RectTransform>().Rotate(new Vector3(0, 180f, 0));
            transform.Rotate(new Vector3(0, 180f, 0));
            transform.Translate(new Vector3(-3.18f, 0, 0));
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MoveFront()
    {
        if (Unit.IsPlayerUnit)
        {
            transform.DOLocalMoveX(transform.localPosition.x + 2f, 0.5f);

        }
        else
        {
            transform.DOLocalMoveX(transform.localPosition.x - 2f, 0.5f);
        }
    }
    public void HpBarRefresh()
    {
        HPBar.value = ((float)Unit.Status.HP / Unit.Status.MaxHP) * 100f;

    }
    public void Scale(float x, float y,float duration)
    {
        transform.DOScale(new Vector2(x, y),duration);
    }

    public void BuffViewerRefresh()
    {
        BuffViewer.SetBuff(Unit);
    }
}
