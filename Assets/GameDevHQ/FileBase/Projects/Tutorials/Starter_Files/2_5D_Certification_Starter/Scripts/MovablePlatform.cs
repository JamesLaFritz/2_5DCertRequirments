using System.Collections;
using Cinemachine;
using UnityEngine;
// ReSharper disable PossibleNullReferenceException

public class MovablePlatform : MonoBehaviour
{
    [SerializeField] private Transform m_platform;
    [SerializeField] private CinemachinePathBase m_wayPoints;
    [SerializeField] private float m_speed = 5f;
    private float m_currentSpeed = 0;
    [SerializeField] private float m_waitTime = 0f;

    [SerializeField, Tag] private string m_playerTag = "Player";
    [SerializeField] private BoolReference m_isPlayerLedgeGrabbing;

    private int m_currentWayPoint;
    private int m_currentDirection = 1;
    private Vector3 m_currentPosition = Vector3.zero;
    private Vector3 m_nextPosition = Vector3.zero;

    private void Start()
    {
        if (m_platform == null)
        {
            Debug.LogException(
                new System.NullReferenceException("Platform Transform required, Please add one in the inspector."),
                this);
        }

        if (m_wayPoints == null)
        {
            Debug.LogException(
                new System.NullReferenceException("Way Points required, Please add one in the inspector."),
                this);
        }

        m_currentPosition = m_nextPosition = transform.position;
        
        
    }

    private void FixedUpdate()
    {
        // set the current position by using move towards
        m_currentPosition = Vector3.MoveTowards(m_currentPosition, m_nextPosition, m_currentSpeed * Time.deltaTime);
        // set the transform position to the current position
        m_platform.transform.position = m_currentPosition;
        // If the current position has not reached the next position return
        if (m_currentPosition != m_nextPosition) return;

        StartCoroutine(NextPosition());
    }

    private IEnumerator NextPosition()
    {
        m_currentSpeed = 0;

        // set the next position the the position at the current way point
        m_nextPosition =
            m_wayPoints.EvaluatePositionAtUnit(m_currentWayPoint, CinemachinePathBase.PositionUnits.PathUnits);

        // increase the current way point
        m_currentWayPoint += m_currentDirection;

        // If the next position is outside the bounds
        if (m_currentWayPoint < m_wayPoints.MinPos ||
            m_currentWayPoint > m_wayPoints.MaxPos)
        {
            // change the direction
            m_currentDirection *= -1;
            // set the next position to the current position
            m_currentWayPoint = m_currentWayPoint + m_currentDirection * 2;
        }

        if (m_waitTime > 0)
            yield return new WaitForSeconds(m_waitTime);

        m_currentSpeed = m_speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        // if the other is tagged with the player tag
        if (other.CompareTag(m_playerTag))
        {
            // parent the player to this platform
            other.transform.SetParent(m_platform.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // if the other is tagged with the player tag
        if (other.CompareTag(m_playerTag) && m_isPlayerLedgeGrabbing.Value == false)
        {
            // set the players parent to null.
            other.transform.parent = null;
        }
    }
}
