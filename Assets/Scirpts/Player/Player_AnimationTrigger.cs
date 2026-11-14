using UnityEngine;

public class Player_AnimationTrigger : EntityAnimationTrigger
{
    private Player player;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponentInParent<Player>();
    }

    private void ThrowSword() => player.SkillManager.swordThrow.ThrowSword();
}
