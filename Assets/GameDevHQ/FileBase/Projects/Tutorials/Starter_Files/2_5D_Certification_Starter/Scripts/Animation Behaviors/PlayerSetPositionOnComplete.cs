using UnityEngine;

public class PlayerSetPositionOnComplete : StateMachineBehaviour
{
    [SerializeField] private Vector3Reference m_position;
    private Vector3 m_animationPosition = Vector3.zero;
    [SerializeField] private BoolReference m_animationComplete;
    [SerializeField] private bool m_useYPosition = true;

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_animationComplete.Value = true;
    }

    // OnStateIK is called right after Animator.OnAnimatorIK()
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (m_animationComplete.Value) return;

        if (m_useYPosition)
            m_animationPosition.y = animator.bodyPosition.y;
        else
        {
            m_animationPosition.y = animator.transform.parent.position.y;
        }

        m_animationPosition.z = animator.bodyPosition.z;
        m_position.Value = m_animationPosition;
    }
}
