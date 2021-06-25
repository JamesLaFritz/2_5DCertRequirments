using UnityEngine;
using UnityEngine.Events;

public class UnityEventTrigger : MonoBehaviour
{
    [SerializeField, Tag] private string m_triggerTag = "Player";

    [SerializeField] private UnityEvent m_triggerEnterEvent;
    private bool m_hasTriggerEnterEvent;

    [SerializeField] private UnityEvent m_triggerExitEvent;
    private bool m_hasTriggerExitEvent;

    [SerializeField] private UnityEvent m_triggerStayEvent;
    private bool m_hasTriggerStayEvent;

    private void Start()
    {
        m_hasTriggerEnterEvent = m_triggerEnterEvent?.GetPersistentEventCount() > 0;
        m_hasTriggerExitEvent = m_triggerExitEvent?.GetPersistentEventCount() > 0;
        m_hasTriggerStayEvent = m_triggerStayEvent?.GetPersistentEventCount() > 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!m_hasTriggerEnterEvent) return;
        // Debug.Log($"{other.name} with tag {other.tag} has entered trigger");
        if (other.CompareTag(m_triggerTag))
        {
            m_triggerEnterEvent.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!m_hasTriggerExitEvent) return;
        // Debug.Log($"{other.name} with tag {other.tag} has exited trigger");
        if (other.CompareTag(m_triggerTag))
        {
            m_triggerExitEvent.Invoke();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!m_hasTriggerStayEvent) return;
        // Debug.Log($"{other.name} with tag {other.tag} is staying in trigger");
        if (other.CompareTag(m_triggerTag))
        {
            m_triggerStayEvent.Invoke();
        }
    }
}
