using System.Collections;
using Cinemachine;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private float m_speed = 0.25f;
    [SerializeField] private float m_waitTime = 5.0f;
    [SerializeField] private CinemachineDollyCart m_platformDollyCart;

    private float m_nextPos;
    private int m_currentDirection = 1;
    private bool m_isWaiting;

    private void Start()
    {
        if (m_platformDollyCart == null)
        {
            Debug.LogException(new System.NullReferenceException(
                                   "A Platform With a Dolly Cart set up is Required, Please Assign one in the inspector"),
                               this);
        }

        m_platformDollyCart.m_Speed = 0;
        m_platformDollyCart.m_PositionUnits = CinemachinePathBase.PositionUnits.PathUnits;
        m_platformDollyCart.m_UpdateMethod = CinemachineDollyCart.UpdateMethod.FixedUpdate;
    }

    private void Update()
    {
        // If the elevator is at a floor waiting Exit the Method
        if (m_isWaiting) return;

        // If the elevator is going up
        if (m_currentDirection == 1)
        {
            // If the platform has reached the next floor
            if (m_platformDollyCart.m_Position >= m_nextPos)
            {
                StartCoroutine(MoveToNextPosition());
            }
        }
        // else if we ar moving down
        else if (m_currentDirection == -1)
        {
            // If the platform has reached the next floor
            if (m_platformDollyCart.m_Position <= m_nextPos)
            {
                StartCoroutine(MoveToNextPosition());
            }
        }
    }

    private void GetNextPosition()
    {
        // Increase the next position
        m_nextPos = m_nextPos + m_currentDirection;

        // If the next position is outside the bounds
        if (m_nextPos < m_platformDollyCart.m_Path.MinPos ||
            m_nextPos > m_platformDollyCart.m_Path.MaxPos)
        {
            // change the direction
            m_currentDirection *= -1;
            // set the next position to the curret position
            m_nextPos = m_nextPos + m_currentDirection * 2;
        }
    }

    // Coroutine to delay at the current floor
    IEnumerator MoveToNextPosition()
    {
        // Stop the platform from moving
        m_platformDollyCart.m_Speed = 0;
        // we are waiting
        m_isWaiting = true;
        // set the next position
        GetNextPosition();

        // wait for the desired amount of time
        yield return new WaitForSeconds(m_waitTime);

        // Set the Speed to the to the speed multiplied by the direction
        m_platformDollyCart.m_Speed = m_speed * m_currentDirection;
        // The Elevator is No Longer waiting
        m_isWaiting = false;
    }
}
