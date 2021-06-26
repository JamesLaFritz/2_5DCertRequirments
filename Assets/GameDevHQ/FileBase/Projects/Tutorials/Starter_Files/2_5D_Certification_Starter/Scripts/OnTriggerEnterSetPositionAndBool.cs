using UnityEngine;

public class OnTriggerEnterSetPositionAndBool : MonoBehaviour
{
    [SerializeField, Tag] private string m_tag = "Player";
    [SerializeField] private Vector3 m_offset;
    [SerializeField] private BoolReference m_boolReference;
    [SerializeField] private Vector3Reference m_vector3Reference;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(m_tag)) return;

        m_vector3Reference.Value = transform.position + m_offset;
        m_boolReference.Value = true;
    }
}
