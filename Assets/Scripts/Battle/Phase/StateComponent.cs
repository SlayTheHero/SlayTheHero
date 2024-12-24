using fsm;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Events;

public class StateComponent : MonoBehaviour
{
    private Dictionary<int, State> m_states;
    private fsm.State m_curState;
    private bool m_is_stop;
    public IReadOnlyDictionary<int, State> States { get { return m_states; } }

    public UnityEvent<int, int> StateChanged;
    private void Awake()
    {
        StateChanged = new();
        m_states = new Dictionary<int, State>();
    }
    // Start is called before the first frame update
    void Start()
    {
        BattleManager.Instance.StateComponent = this;
    }

    // Update is called once per frame
    void Update()
    {
        int next;
        if (m_is_stop)
            return;
        if (m_curState == null)
            return;
        if (m_curState.CheckTransition(out next))
            ChangeState(next);
        else
            m_curState.OnStateUpdate();
    }
    public void AddState(State state)
    {
        m_states.Add(state.StateNum, state);
        state.OnStateInit();
    }
    public void ChangeState(int state)
    {
        if (m_is_stop)
            return;
        var prev = m_curState.StateNum;
        m_curState.OnStateExit();
        m_curState = m_states[state];
        m_curState.OnStateEnter();
        StateChanged.Invoke(prev, state);
    }
    public void FSMStart(int start_state)
    {
        m_is_stop = false;
        m_curState = m_states[start_state];
        m_curState.OnStateEnter();
    }
    public void FSMStop()
    {
        m_is_stop = true;
    }
}
