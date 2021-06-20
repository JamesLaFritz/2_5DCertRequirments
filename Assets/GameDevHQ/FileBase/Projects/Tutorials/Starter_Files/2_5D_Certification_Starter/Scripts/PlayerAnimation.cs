using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    private Animator m_animator;

    [SerializeField] private FloatReference m_speed;
    private static readonly int Speed = Animator.StringToHash("Speed");

    [SerializeField] private BoolReference m_isJumping;
    private static readonly int Jump = Animator.StringToHash("IsJumping");
    private bool m_jumping;

    [SerializeField] private BoolReference m_isLedgeGrabbing;
    private static readonly int LedgeGrab = Animator.StringToHash("FreeHang");
    private bool m_ledgeGrabbing;

    [SerializeField] private BoolReference m_roll;
    private static readonly int Roll = Animator.StringToHash("Roll");

    [SerializeField] private bool m_useIK = true;
    private bool m_setIkPosition;
    [SerializeField] private Transform m_leftHandPosition;
    [SerializeField] private Transform m_rightHandPosition;

    private void Start()
    {
        m_animator = GetComponent<Animator>();

        if (m_useIK)
        {
            if (m_leftHandPosition == null || m_rightHandPosition == null)
            {
                Debug.LogWarning($"{name} requires hand positions for ledge grabbing to use IK, please assign in inspector", this);
                m_useIK = false;
            }
        }
    }

    private void Update()
    {
        // ReSharper disable PossibleNullReferenceException
        m_animator.SetFloat(Speed, m_speed.Value);
        if (m_jumping != m_isJumping.Value)
        {
            m_jumping = m_isJumping.Value;
            m_animator.SetBool(Jump, m_isJumping.Value);
        }

        if (m_ledgeGrabbing != m_isLedgeGrabbing.Value)
        {
            m_ledgeGrabbing = m_isLedgeGrabbing.Value;
            m_animator.SetBool(LedgeGrab, m_ledgeGrabbing);
        }

        if (m_roll.Value)
        {
            m_roll.Value = false;
            m_animator.SetTrigger(Roll);
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (!m_useIK) return;

        m_animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, m_ledgeGrabbing ? 1 : 0);
        m_animator.SetIKPositionWeight(AvatarIKGoal.RightHand, m_ledgeGrabbing ? 1 : 0);
        m_animator.SetIKPosition(AvatarIKGoal.LeftHand, m_leftHandPosition.position);
        m_animator.SetIKPosition(AvatarIKGoal.RightHand, m_rightHandPosition.position);
    }
}
