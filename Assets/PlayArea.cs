using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class PlayArea : MonoBehaviour
{
    public GameObject puck;
    public GameObject agentA;
    public GameObject agentB;

    PusherAgent m_AgentA;
    PusherAgent m_AgentB;

    Rigidbody m_PuckRb;
    PuckController puckController;
    // Start is called before the first frame update
    void Start()
    {
        m_PuckRb = puck.GetComponent<Rigidbody>();
        m_AgentA = agentA.GetComponent<PusherAgent>();
        m_AgentB = agentB.GetComponent<PusherAgent>();

        puckController = puck.GetComponent<PuckController>();
        puckController.m_area = this;

        MatchReset(true); // AgentA start first
    }

    public void MatchReset(bool agentA_start)
    {
        m_AgentA.EndEpisode();
        m_AgentB.EndEpisode();
        agentA.transform.position = new Vector3(0f, 0f, -3f);
        agentB.transform.position = new Vector3(0f, 0f, 3f);
        if (agentA_start)
        {
            puck.transform.position = new Vector3(0f, 0f, -1f);
        }
        else
        {
            puck.transform.position = new Vector3(0f, 0f, 1f);
        }        
        m_PuckRb.velocity = new Vector3(0f, 0f, 0f);
    }

    void AgentAWins()
    {
        m_AgentA.SetReward(1);
        m_AgentB.SetReward(-1);
        m_AgentA.score += 1;
        MatchReset(false);
    }

    void AgentBWins()
    {
        m_AgentA.SetReward(-1);
        m_AgentB.SetReward(1);
        m_AgentB.score += 1;
        MatchReset(true);
    }

    public void GoalTouched(PusherAgent.Team scoredTeam)
    {
        if (scoredTeam == PusherAgent.Team.Blue)
        {
            AgentAWins();
        }
        else // Purple 
        {
            AgentBWins();
        }
    }
}
