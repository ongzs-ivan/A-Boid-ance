using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public BoidSettings settings;

    // Base Vectors
    [HideInInspector] public Vector3 pos;
    [HideInInspector] public Vector3 ahead;
    [HideInInspector] public Vector3 offset;
    protected Vector3 velocity;

    // Updating values
    protected Vector3 targetOffset;
    protected Vector3 centreOffset;
    protected float speed;
    protected Vector3 dir;
    protected Vector3 acceleration;
    protected Vector3 alignmentForce;
    protected Vector3 cohesionForce;
    protected Vector3 seperationForce;
    [HideInInspector] public bool arrived = false;
    [HideInInspector] public bool returned = false;
    
    [HideInInspector] public Vector3 avgAlignment;
    [HideInInspector] public Vector3 avgSeparation;
    [HideInInspector] public Vector3 centreCohesion;
    [HideInInspector] public int neighbourBoids;

    // Cache
    protected Transform cachedTransform;
    //protected Transform home;
    protected Boid boidTarget;
    protected Transform target;
    protected List<Boid> repellers;

    private void Awake()
    {
        repellers = new List<Boid>();
        cachedTransform = transform;
    }

    public virtual void BoidInitialize()
    {
        // left empty for override for inheritance
    }

    public virtual void BoidInitialize(BoidSettings settings, Boid boidTarget)
    {
        this.boidTarget = boidTarget;
        target = boidTarget.transform;
        this.settings = settings;

        pos = cachedTransform.position;
        ahead = cachedTransform.forward;

        float startSpeed = Random.Range(settings.minSpeed, settings.maxSpeed);
        velocity = transform.forward * startSpeed;
    }

    public void UpdateSettings(BoidSettings newSettings)
    {
        settings = newSettings;
    }

    public virtual void UpdateBoid()
    {
        acceleration = Vector3.zero;

        // Seek
        if (target != null)
        {
            // straight seek
            //targetOffset = target.position - pos;
            //acceleration = Steering(targetOffset) * settings.targetWeight;

            // arrival seek
            targetOffset = target.position - pos;
            acceleration = Arrival(targetOffset) * settings.targetWeight;
        }

        if (neighbourBoids != 0)
        {
            centreCohesion /= neighbourBoids;

            centreOffset = centreCohesion - pos;

            alignmentForce  = Steering(avgAlignment)    * settings.alignWeight;
            cohesionForce   = Steering(centreOffset)    * settings.cohesionWeight;
            seperationForce = Steering(avgSeparation)   * settings.seperateWeight;

            acceleration += alignmentForce;
            acceleration += cohesionForce;
            acceleration += seperationForce;
        }

        if (collisionAvoidance())
        {
            Vector3 collisionAvoidDir = RaycastForObstacles();
            Vector3 collisionAvoidForce = Steering(collisionAvoidDir) * settings.avoidCollisionWeight;
            acceleration += collisionAvoidForce;
        }

        // evading repellers
        if (repellers.Count > 0)
        {
            for (int i = 0; i < repellers.Count; i++)
            {
                float d = (pos - repellers[i].pos).magnitude;
                if (d < settings.fleeRadius)
                {
                    acceleration += Evade(repellers[i]) * settings.evadeWeight;
                }
            }
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

    public bool collisionAvoidance()
    {
        RaycastHit hit;
        if (Physics.SphereCast(pos, settings.boundsRadius, ahead, out hit, settings.collisionAvoidDst, settings.obstacleMask))
        {
            return true;
        }
        return false;
    }
    

    public Vector3 RaycastForObstacles()
    {
        Vector3[] rayDirections = BoidVision.directions;

        for (int i = 0; i < rayDirections.Length; i++)
        {
            Vector3 dir = cachedTransform.TransformDirection(rayDirections[i]);
            Ray ray = new Ray(pos, dir);
            if (!Physics.SphereCast(ray, settings.boundsRadius, settings.collisionAvoidDst, settings.obstacleMask))
            {
                return dir;
            }
        }
        return ahead;
    }

    public virtual Vector3 Steering(Vector3 vector)
    {
        Vector3 v = vector.normalized * settings.maxSpeed - velocity;
        return Vector3.ClampMagnitude(v, settings.maxSteerForce);
    }

    public Vector3 Arrival(Vector3 vector)
    {
        float d = vector.magnitude;

        if (d < 1.0f)
        {
            return Vector3.zero;
        }
        float desiredSpeed = 0;
        if (d < settings.arrivalRadius)
        {
            desiredSpeed = (d / settings.arrivalRadius) * settings.maxSpeed * (1.0f - 0.9f);
        }
        else
        {
            desiredSpeed = settings.maxSpeed;
        }
        vector *= desiredSpeed;
        return Vector3.ClampMagnitude(vector, settings.maxSteerForce);
    }

    private Vector3 Flee(Vector3 targetPos)
    {
        Vector3 desiredVelocity;
        desiredVelocity = this.pos - targetPos;
        if (desiredVelocity.magnitude > settings.fleeRadius)
        {
            return Vector3.zero;
        }
        desiredVelocity.Normalize();
        desiredVelocity *= settings.maxSpeed;
        return (desiredVelocity - velocity);
    }

    public Vector3 Evade(Boid predator)
    {
        float dist = (predator.pos - this.pos).magnitude;
        float lookAhead = settings.maxSpeed;

        Vector3 tempTargetPos = predator.pos + (lookAhead * predator.velocity);
        return Flee(tempTargetPos);
    }

    public void AddRepellers(Boid newBoid)
    {
        repellers.Add(newBoid);
    }
}
