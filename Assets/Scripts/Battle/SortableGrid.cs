using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

public class SortableGrid : MonoBehaviour
{
    [SerializeField]
    List<SortableUIObject> m_items;
    List<SortableUIObject> m_enable_items;

    // Start is called before the first frame update
    void Start()
    {
        m_enable_items = new List<SortableUIObject>();
        for (int i = 0; i < m_items.Count; i++)
        {
            if (m_items[i].enabled)
            {
                m_items[i].Order = m_enable_items.Count;
                m_items[i].PrevOrder = m_enable_items.Count;
                m_enable_items.Add(m_items[i]);
            }
        }
    }
    public void ItemsReset(int item_count)
    {
        m_enable_items.Clear();
        for (int i = 0; i < item_count; i++)
        {
            m_items[i].enabled = true;
            m_items[i].Order = m_enable_items.Count;
            m_items[i].SizeReset();
            m_enable_items.Add(m_items[i]);
        }
        for (int i = item_count;i < m_items.Count; i++)
        {
            m_items[i].enabled = false;
        }


    }
    public void Sort(IComparable[] data)
    {
        for (int i = 0; i < data.Length; i++)
        {
            m_enable_items[i].ComparableData = data[i];
        }
        var temp = m_enable_items.GetRange(0, data.Length);
        temp.Sort();
        for (int i = 0; i < temp.Count; i++)
        {
            temp[i].OrderChange(i);
        }
        for (int i = 0; i < temp.Count; i++)
        {
            m_enable_items[i].MoveByOrder();
        }
        m_enable_items = temp;
    }
    public void RemoveItem(int index)
    {
        m_enable_items[index].Remove();
        m_enable_items.RemoveAt(index);
    }

}

