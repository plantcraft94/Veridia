using UnityEngine;

public class AnimationEventRelay : MonoBehaviour
{
    private AiAttack aiAttack;

    void Awake()
    {
        aiAttack = GetComponentInParent<AiAttack>();
    }

    public void EnableSwordHitbox(int index)
    {
        if (aiAttack != null)
            aiAttack.EnableSwordHitbox(index);
    }

    public void DisableSwordHitbox(int index)
    {
        if (aiAttack != null)
            aiAttack.DisableSwordHitbox(index);
    }
}
