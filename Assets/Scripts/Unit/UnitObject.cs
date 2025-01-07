using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitObject : MonoBehaviour
{
    public Slider HPBar;
    public UnitBase Unit;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MoveFront()
    {
        if (Unit.IsPlayerUnit)
        {
            transform.DOLocalMoveX(transform.localPosition.x + 1.5f, 0.5f);

        }
        else
        {
            transform.DOLocalMoveX(transform.localPosition.x - 1.5f, 0.5f);
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
}
