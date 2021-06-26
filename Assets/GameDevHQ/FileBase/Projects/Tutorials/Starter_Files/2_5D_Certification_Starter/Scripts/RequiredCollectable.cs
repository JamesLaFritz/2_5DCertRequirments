using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class RequiredCollectable : MonoBehaviour
{
    [SerializeField] private IntReference m_collectableCount;
    [SerializeField] private int m_amountNeeded = 10;
    [SerializeField] private UnityEvent onRequirementMeet;
    private bool m_requirementMeet;

    [SerializeField] private TextMeshPro m_textMesh;
    [SerializeField] private string collectableType = "Coins";
    private bool m_hasText;
    [SerializeField, Tag] private string m_playerTag = "Player";

    private void Start()
    {
        m_hasText = m_textMesh != null;
    }

    private void Update()
    {
        if (m_requirementMeet) return;

        if (m_amountNeeded > m_collectableCount.Value) return;

        m_requirementMeet = true;
        onRequirementMeet?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!m_hasText || m_requirementMeet) return;
        
        if (!other.CompareTag(m_playerTag)) return;

        m_textMesh.text = $"Requires {m_amountNeeded - m_collectableCount.Value} more {collectableType} in  order to Activate";
        m_textMesh.transform.gameObject.SetActive(true);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!m_hasText || m_requirementMeet) return;

        if (!other.CompareTag(m_playerTag)) return;

        m_textMesh.text =
            $"Requires {m_amountNeeded - m_collectableCount.Value} more {collectableType} in  order to Activate";
    }

    private void OnTriggerExit(Collider other)
    {
        if (!m_hasText || m_requirementMeet) return;

        if (!other.CompareTag(m_playerTag)) return;
        
        m_textMesh.transform.gameObject.SetActive(false);
    }
}
