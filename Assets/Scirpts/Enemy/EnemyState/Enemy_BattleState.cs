using UnityEngine;

public class Enemy_BattleState : EnemyState
{
    private Transform player;
    private float lastTimeWasInBattle;
    public int tmp;
    public Enemy_BattleState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        
        UpdateAttackTimer();
        
        //player ??= enemy.GetPlayerReference();  //the ?? is mean check if player is null or not if(player == null)
        if (player == null) player = enemy.GetPlayerReference();
        //hàm playerDetection bassiclly bắn 1 tia raycast và check trên ray đó 
        //có chạm object nào được chỉ định không và trong trường hợp này, layermask được truyền vào là Player và Platfrom
        //hàm playerDetection sẽ return về kiểu dữ liệu RayCasthit2D của object đó, trong đó chứa data của ọnect đó bao hàm
        //collider2D; transform;... và ở đây dùng transform để lấy data về vị trí, độ sâu, dộ xoay,... của object
        
        if (shouldRetreat())
        {
            rb.linearVelocity = new Vector2(enemy.retreatVelocity.x * -directionPlayer(), enemy.retreatVelocity.y);
            enemy.HandleFlip(directionPlayer());
        }
    }

    public override void Update()
    {
        base.Update();

        if (enemy.playerDetection())
            UpdateAttackTimer();

        if (BattleTimeOver()) stateMachine.changeState(enemy.idleState);
        
        if (inAttackRange() && enemy.playerDetection()) stateMachine.changeState(enemy.attackState);
        else enemy.setVelocity(enemy.battleSpeed * directionPlayer(), rb.linearVelocity.y);
    }

    private void UpdateAttackTimer() => lastTimeWasInBattle = Time.time;

    private bool BattleTimeOver() => Time.time > lastTimeWasInBattle + enemy.battleTimeDurration;

    private bool inAttackRange() =>  distanceToPlayer() < enemy.attackDistance;

    private bool shouldRetreat() => distanceToPlayer() < enemy.minRetreatDistance;

    private float distanceToPlayer()
    {
        if (player == null) return float.MaxValue;  //if not yet detect player then return INT_MAX như trong C++
        return Mathf.Abs(player.position.x - enemy.transform.position.x);  //return về khoảng cách tuyệt đối char và enemy.
    }

    private int directionPlayer()
    {
        if (player == null) tmp = 0;
        if (Mathf.Abs(player.position.y - enemy.transform.position.y) > 1)
        {
            if (player.position.x - enemy.transform.position.x > 1.5) tmp = 1;
            else if (player.position.x - enemy.transform.position.x < -1.5) tmp = -1;
        }
        else tmp = player.position.x > enemy.transform.position.x ? 1 : -1;

        return tmp;
    }
}
