using UnityEngine;

public class Ladder : MonoBehaviour
{
    [SerializeField, Tag] private string m_playerTag = "Player";
    [SerializeField] private BoolReference m_playerOnLadder;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(m_playerTag))
        {
            m_playerOnLadder.Value = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(m_playerTag))
        {
            m_playerOnLadder.Value = false;
        }
    }
}
