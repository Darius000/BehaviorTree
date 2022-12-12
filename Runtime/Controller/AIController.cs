using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIBehaviorTree;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIController : MonoBehaviour
{

    public BehaviorTree m_Tree;

    private NavMeshAgent m_Agent;

    bool m_IsRunning = false;

    bool m_IsPaused = false;

    [SerializeField]
    protected bool ShowDebugInfo = true;

    public BehaviorTree Tree { get { return m_Tree; } }

    public void RunBehaviourTree()
    {
        m_Agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        if (m_Agent)
        {
            m_Agent.updateUpAxis = false;
            m_Agent.updateRotation = false;
        }

        if (m_Tree)
        {
            m_Tree = m_Tree.Clone();
            m_Tree.Bind();

        }

        m_IsRunning = true;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        RunBehaviourTree();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (m_Tree && m_IsRunning && !m_IsPaused)
            m_Tree.Run(m_Agent, this);
    }

    public bool IsRunning { get { return m_IsRunning; } }

    public bool IsPaused { get { return m_IsPaused; } }

    public void Pause(bool pause)
    {
        m_IsPaused = pause;
    }

    public void Stop()
    {
        m_IsRunning = false;
    }

    public NavMeshAgent GetAgent()
    {
        return m_Agent;
    }

    public BlackBoard GetBlackBoard()
    {
        return m_Tree?.m_BlackBoard;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if(ShowDebugInfo && m_Tree)
        {
            m_Tree?.DrawDebugInfo(this);
        }
    }
#endif
}