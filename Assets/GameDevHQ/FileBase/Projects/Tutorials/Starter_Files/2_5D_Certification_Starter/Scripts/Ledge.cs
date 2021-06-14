using System.Runtime.InteropServices;
using UnityEngine;

public class Ledge : MonoBehaviour
{
   [SerializeField, Tag] private string m_grabberTag = "LedgeGrabber";

   private void OnTriggerEnter(Collider other)
   {
      //Debug.Log($"{other.name} with tag {other.tag} as collided with {name}");

      if (!other.CompareTag(m_grabberTag)) return;

      Debug.Log("Collided with Grab Checker");
      // When Colliding with the player edge grab checker
      // Trigger the players Edge Hang Animation.
      // Freeze the players gravity
   }
}
