using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mines : Boid
{
    [HideInInspector] public float currentTargetDist = 0.0f;
    [HideInInspector] public bool isAlive = true;

    private WaitForSeconds noDelay = new WaitForSeconds(0.0f);

    public Boid currentTarget;
    public float detectionRadius;

    private void Awake()
    {
        cachedTransform = transform;
    }

    private void Start()
    {
        BoidInitialize();
        StartCoroutine(UpdateMines());
    }

    public override void BoidInitialize()
    {
        pos = cachedTransform.position;
        ahead = cachedTransform.forward;

        float startSpeed = Random.Range(settings.minSpeed, settings.maxSpeed);
        velocity = -transform.forward * startSpeed;
    }

    public void SetNewTarget(Boid newTarget)
    {
        boidTarget = newTarget;
        target = boidTarget.transform;
    }

    public override void UpdateBoid()
    {
        acceleration = Vector3.zero;

        if (target != null)
        {
            targetOffset = target.position - pos;
            acceleration = Arrival(targetOffset) * settings.targetWeight;
        }

        velocity += acceleration * Time.deltaTime;
        speed = velocity.magnitude;
        dir = velocity / speed;
        speed = Mathf.Clamp(speed, settings.minSpeed, settings.maxSpeed);
        velocity = dir * speed;

        cachedTransform.position += velocity * Time.deltaTime;
        cachedTransform.forward = dir;
        pos = cachedTransform.position;
        ahead = dir;
    }

    public IEnumerator UpdateMines()
    {
        while (isAlive)
        {
            acceleration = Vector3.zero;

            if (target != null)
            {
                targetOffset = target.position - pos;
                acceleration = Arrival(targetOffset) * settings.targetWeight;
            }

            if (collisionAvoidance())
            {
                Vector3 collisionAvoidDir = RaycastForObstacles();
                Vector3 collisionAvoidForce = Steering(collisionAvoidDir) * settings.avoidCollisionWeight;
                acceleration += collisionAvoidForce;
            }

            velocity += acceleration * Time.deltaTime;
            speed = velocity.magnitude;
            dir = velocity / speed;
            speed = Mathf.Clamp(speed, settings.minSpeed, settings.maxSpeed);
            velocity = dir * speed;

            cachedTransform.position += velocity * Time.deltaTime;
            cachedTransform.forward = dir;
            pos = cachedTransform.position;
            ahead = dir;
            yield return noDelay;
        }
        yield return null;
    }
}
