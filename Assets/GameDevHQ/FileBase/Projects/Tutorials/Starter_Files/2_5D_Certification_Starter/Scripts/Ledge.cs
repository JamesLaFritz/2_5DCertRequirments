using UnityEngine;

public class Ledge : MonoBehaviour
{
   [SerializeField, Tag] private string m_grabberTag = "LedgeGrabber";
   [SerializeField] private BoolReference m_playerIsLedgeGrabbing;

   private void OnTriggerEnter(Collider other)
   {
      if (!other.CompareTag(m_grabberTag)) return;

      m_playerIsLedgeGrabbing.Value = true;
   }
}
