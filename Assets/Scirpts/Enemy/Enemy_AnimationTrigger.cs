using UnityEngine;

public class Enemy_AnimationTrigger : EntityAnimationTrigger
{
    private Enemy enemy;

    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponentInParent<Enemy>();
    }

    private void EnableCounterWindow()
    {
        enemy.EnableCounterWindow(true);
    }

    private void DisableCounterWindow()
    {
        enemy.EnableCounterWindow(false);
    }
}
