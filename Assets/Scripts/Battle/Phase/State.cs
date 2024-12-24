using System;
using System.Collections.Generic;
namespace fsm
{
    [Serializable]
    public abstract class State
    {
        private int m_stateNum;
        private List<Transition> m_transitions;
        public IReadOnlyList<Transition> Transitions { get { return m_transitions; } }
        public int StateNum { get { return m_stateNum; } }

        public State(int stateNum)
        {
            m_stateNum = stateNum;
            m_transitions = new List<Transition>();
        }
        public virtual void OnStateInit() { }
        public virtual void OnStateEnter() { }
        public virtual void OnStateUpdate() { }
        public virtual void OnStateExit() { }
        public virtual bool CheckTransition(out int to)
        {
            to = m_stateNum;
            if(m_transitions.Count == 0) return false;
            foreach(var transition in m_transitions)
            {
                if (transition.CheckCondition())
                {
                    to = transition.to;
                    return true;
                }
            }
            return false;
        }
        protected void AddTransition(int to,Transition.Condition condition)
        {
            m_transitions.Add(new Transition(m_stateNum,to,condition));
        }
    }
}