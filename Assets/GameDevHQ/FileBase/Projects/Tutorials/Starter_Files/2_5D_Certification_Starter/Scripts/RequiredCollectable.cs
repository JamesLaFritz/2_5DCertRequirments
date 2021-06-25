using UnityEngine;
using UnityEngine.Events;

public class RequiredCollectable : MonoBehaviour
{
    [SerializeField] private IntReference m_collectableCount;
    [SerializeField] private int m_amountNeeded = 10;
    [SerializeField] private UnityEvent onRequirementMeet;
    private bool m_requirementMeet;

    private void Update()
    {
        if (m_requirementMeet) return;

        if (m_amountNeeded > m_collectableCount.Value) return;

        m_requirementMeet = true;
        onRequirementMeet?.Invoke();
    }
}
