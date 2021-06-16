using Unity.Mathematics;
using UnityEngine;

public class LedgeIKSnap : MonoBehaviour
{
    public bool useIK = true;

    public float distanceToCheck = 1.5f;

    public Transform LeftHand;
    public Transform RightHand;

    public bool leftHandIK;
    public bool rightHandIK;

    public Vector3 leftHandTargetPosition;
    public Vector3 rightHandTargetPosition;

    private Animator m_animator;

    void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (!useIK)
            return;

        RaycastHit LHit;
        RaycastHit RHit;
        Debug.DrawRay(LeftHand.position, LeftHand.forward * distanceToCheck, Color.blue);
        Debug.DrawRay(RightHand.position, RightHand.forward * distanceToCheck, Color.cyan);

        // Left Hand
        if (Physics.Raycast(LeftHand.position,
                            LeftHand.forward,
                            out LHit, distanceToCheck))
        {
            leftHandIK = true;
            leftHandTargetPosition = LHit.point;
        }
        else
        {
            leftHandIK = false;
        }

        //Right Hand
        if (Physics.Raycast(RightHand.position,
                            RightHand.forward,
                            out RHit, distanceToCheck))
        {
            rightHandIK = true;
            rightHandTargetPosition = RHit.point;
        }
        else
        {
            rightHandIK = false;
        }
    }

    // private void OnAnimatorIK(int layerIndex)
    // {
    //     if (!useIK)
    //         return;
    //
    //     if (leftHandIK)
    //     {
    //         m_animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
    //         m_animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
    //         m_animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTargetPosition);
    //         m_animator.SetIKRotation(AvatarIKGoal.LeftHand, quaternion.identity);
    //     }
    //
    //     if (rightHandIK)
    //     {
    //         m_animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
    //         m_animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
    //         m_animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTargetPosition);
    //         m_animator.SetIKRotation(AvatarIKGoal.RightHand, quaternion.identity);
    //     }
    // }
}
