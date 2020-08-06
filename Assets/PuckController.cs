using UnityEngine;

public class PuckController : MonoBehaviour
{
    [HideInInspector]
    public PlayArea m_area;
    public string purpleGoalTag; //will be used to check if collided with purple goal
    public string blueGoalTag; //will be used to check if collided with blue goal

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag(purpleGoalTag)) //ball touched purple goal
        {
            m_area.GoalTouched(PusherAgent.Team.Blue);
        }
        if (col.gameObject.CompareTag(blueGoalTag)) //ball touched blue goal
        {
            m_area.GoalTouched(PusherAgent.Team.Purple);
        }
    }
}
