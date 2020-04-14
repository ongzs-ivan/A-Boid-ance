using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : Boid
{
    [HideInInspector] public bool isAlive = true;
    public GameObject minePrefab;
    public Transform mineParent;
    
    private WaitForSeconds noDelay = new WaitForSeconds(0.0f);
    private WaitForSeconds mineDelay = new WaitForSeconds(7.5f);
    private int currentMineCount = 0;
    private int maxMineCount = 10;

    private void Awake()
    {
        cachedTransform = transform;
    }

    private void Start()
    {
        repellers = new List<Boid>();
        BoidInitialize();
        StartCoroutine(UpdateTarget());
        StartCoroutine(MineSpawning());
    }

    public override void BoidInitialize()
    {
        pos = cachedTransform.position;
        ahead = cachedTransform.forward;

        float startSpeed = Random.Range(settings.minSpeed, settings.maxSpeed);
        velocity = transform.forward * startSpeed;
    }

    public IEnumerator UpdateTarget()
    {
        while(isAlive)
        {
            acceleration = Vector3.zero;
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

            yield return noDelay;
        }
        yield return null;
    }

    // spawns mines every given interval
    public IEnumerator MineSpawning()
    {
        while (isAlive)
        {
            if (currentMineCount < maxMineCount)
            {
                Instantiate(minePrefab, this.transform.position + (Vector3.forward * -1.5f), Quaternion.Inverse(this.transform.rotation), mineParent);
                Mines newMine = minePrefab.GetComponent<Mines>();
                BoidManager.instance.AddNewMine(newMine);
                currentMineCount++;
                yield return mineDelay;
            }
        }
        yield return null;
    }

    
}
