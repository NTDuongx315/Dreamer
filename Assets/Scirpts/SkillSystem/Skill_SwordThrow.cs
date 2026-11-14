using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class Skill_SwordThrow : Skill_Base
{
    [Range(0,10)]
    [SerializeField] private float throwPower = 5;
    [SerializeField] private float swordGravity = 3.5f;
    
    [Header("Trajectory predictions")]
    [SerializeField] private GameObject predictionDot;
    [SerializeField] private int numberOfDots = 20;
    [SerializeField] private float spaceBetweenDots = .5f;
    private Transform[] dots;
    private Vector2 confirmedDirection;

    protected override void Awake()
    {
        base.Awake();
        dots = GenerateDots();
    }

    public void ThrowSword()
    {
        Debug.Log("piuuuuuuuuuuuuu");
    }

    public void PredictedTrajectory(Vector2 direction)
    {
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i].position = GetTrajectorypoint(direction, i * spaceBetweenDots);
        }
    }

    private Vector2 GetTrajectorypoint(Vector2 direction, float time)
    {
        float scalePower = throwPower * 10;
        
        //This gives the initial velocity - which is the starting speed and direction of the throw
        Vector2 initialVelocity = direction * scalePower;

        //Gravity pulls the sword down over time. The longer it's in the air, the more it drops (công thức bài toán ném xiên trong vật lí)
        Vector2 gravityEffect = 0.5f * Physics2D.gravity * swordGravity * (time * time);
        
        //Calculate how far the sword will travel after time by combining the initial throw direction with gravity pull.
        Vector2 predictedPoint = (initialVelocity * time) + gravityEffect;
        Vector2 playerPosition = transform.root.position;
        return playerPosition + predictedPoint;
    }

    public void ConfirmTrajectory(Vector2 direction) => confirmedDirection = direction;

    public void EnableDots(bool enable)
    {
        foreach (Transform t in dots)
            t.gameObject.SetActive(enable);
    }

    private Transform[] GenerateDots()
    {
        Transform[] newDots = new Transform[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            newDots[i] = Instantiate(predictionDot, transform.position, Quaternion.identity, transform).transform;
            newDots[i].gameObject.SetActive(false);
        }
        return newDots;
    }
}
