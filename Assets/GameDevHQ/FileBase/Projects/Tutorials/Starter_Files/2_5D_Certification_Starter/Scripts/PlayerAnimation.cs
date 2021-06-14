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

    private void Start()
    {
        m_animator = GetComponent<Animator>();
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

        if (m_isLedgeGrabbing.Value)
        {
            m_isLedgeGrabbing.Value = false;
            m_animator.SetTrigger(LedgeGrab);
        }
    }
}
