using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField, Tag] private string m_PlayerTage = "Player";
    [SerializeField] private IntReference m_collectableCount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(m_PlayerTage))
        {
            m_collectableCount.Value++;

            Destroy(gameObject);
        }
    }
}
