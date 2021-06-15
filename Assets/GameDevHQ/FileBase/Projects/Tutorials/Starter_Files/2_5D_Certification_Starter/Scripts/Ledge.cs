using UnityEngine;

public class Ledge : MonoBehaviour
{
   [SerializeField, Tag] private string m_grabberTag = "LedgeGrabber";
   [SerializeField] private Vector3 m_playersOffsetPosition;
   [SerializeField] private BoolReference m_playerIsLedgeGrabbing;
   [SerializeField] private Vector3Reference m_playerLedgePosition;

   private void OnTriggerEnter(Collider other)
   {
      if (!other.CompareTag(m_grabberTag)) return;

      m_playerLedgePosition.Value = transform.position - m_playersOffsetPosition;
      m_playerIsLedgeGrabbing.Value = true;
   }
}
