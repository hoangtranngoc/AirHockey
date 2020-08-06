using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class PusherAgent : Agent
{
    public GameObject puck;
    public bool invertX;
    public int score;
    public GameObject myArea;
    public float maxSpeed;

    Rigidbody m_AgentRb;
    Rigidbody m_PuckRb;

    // Looks for the scoreboard based on the name of the gameObjects.
    // Do not modify the names of the Score GameObjects
    const string k_CanvasName = "Canvas";
    const string k_ScoreBoardAName = "ScoreBlue";
    const string k_ScoreBoardBName = "ScorePurple";

    Text m_TextComponent;

    public enum Team
    {
        Blue = 0,
        Purple = 1
    }

    // Start is called before the first frame update
    public override void Initialize()
    {
        m_AgentRb = GetComponent<Rigidbody>();
        m_PuckRb = puck.GetComponent<Rigidbody>();
        var canvas = GameObject.Find(k_CanvasName);
        GameObject scoreBoard;
        if (invertX)
        {
            scoreBoard = canvas.transform.Find(k_ScoreBoardBName).gameObject;
        }
        else
        {
            scoreBoard = canvas.transform.Find(k_ScoreBoardAName).gameObject;
        }
        m_TextComponent = scoreBoard.GetComponent<Text>();
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        Vector3 controlSignal = Vector3.zero;
        if (m_AgentRb.velocity.x > maxSpeed)
        {
            controlSignal.x = 0;
        }
        else
        {
            controlSignal.x = vectorAction[0] * maxSpeed;
        }

        if (m_AgentRb.velocity.z > maxSpeed)
        {
            controlSignal.z = 0;
        }
        else
        {
            controlSignal.z = vectorAction[1] * maxSpeed;
        }
        
        m_AgentRb.AddForce(controlSignal, ForceMode.Impulse);
        m_TextComponent.text = score.ToString();
    }

    public override void OnEpisodeBegin()
    {
        m_AgentRb.velocity = new Vector3(0f, 0f, 0f);

    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Horizontal");
        actionsOut[1] = Input.GetAxis("Vertical");
    }

    void OnCollisionEnter(Collision c)
    {
        var force = 10f;
        if (c.gameObject.CompareTag("puck"))
        {
            Debug.Log("collide puck");
            var dir = c.contacts[0].point - transform.position;
            dir = dir.normalized;
            c.gameObject.GetComponent<Rigidbody>().AddForce(dir * force);
        }
    }


}
