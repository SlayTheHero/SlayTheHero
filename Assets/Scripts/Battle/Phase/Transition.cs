using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace fsm
{
    public class Transition
    {
        public int from;
        public int to;
        public delegate bool Condition();
        public Condition CheckCondition;
        public Transition(int from,int to,Condition condition)
        {
            this.to = to;
            this.from = from;
            CheckCondition = condition;
        }
        public void SetTransitionCondition(Condition condition)
        {
            CheckCondition = condition;
        }
    }
}
