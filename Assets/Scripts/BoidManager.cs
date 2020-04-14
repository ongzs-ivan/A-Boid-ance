using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    public static BoidManager instance;

    public GameObject boidPrefab;
    public Transform[] spawnPoints;

    [HideInInspector] public BoidSettings managerSettings;

    public Target attractor;        // core fighters
    private List<Mines> repellers;  // mines
    private List<Boid> flock;       // funnels
    
    private Transform spawnParent;
    private int randSpawn = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        flock = new List<Boid>();
        repellers = new List<Mines>();
        spawnParent = this.transform;
    }

    void Update()
    {
        if (flock != null)
        {
            // Funnel update
            for (int i = 0; i < flock.Count; i++)
            {
                for (int j = 0; j < flock.Count; j++)
                {
                    if (flock[i] != flock[j])
                    {
                        flock[i].offset = flock[j].pos - flock[i].pos;
                        float magnitude = flock[i].offset.sqrMagnitude;

                        if (magnitude < (managerSettings.perceptionRadius * managerSettings.perceptionRadius))
                        {
                            flock[i].neighbourBoids++;
                            flock[i].avgAlignment += flock[j].ahead;
                            flock[i].centreCohesion += flock[j].pos;

                            if (magnitude < (managerSettings.avoidanceRadius * managerSettings.avoidanceRadius))
                            {
                                flock[i].avgSeparation -= (flock[i].offset / magnitude);
                            }
                        }
                    }
                }
                flock[i].UpdateBoid();
            }

            // Mines update
            if (repellers.Count > 0 && flock.Count > 0)
            {
                for (int i = 0; i < repellers.Count; i++)
                {
                    for (int j = 0; j < flock.Count; j++)
                    {
                        float newTargetDist = (repellers[i].pos - flock[j].pos).sqrMagnitude;
                        if (newTargetDist < repellers[i].currentTargetDist || newTargetDist < repellers[i].detectionRadius)
                        {
                            repellers[i].SetNewTarget(flock[j]);
                            repellers[i].currentTarget = flock[j];
                        }
                    }
                }
            }

        }
    }

    public void UpdateBoidSettings(BoidSettings newSettings)
    {
        managerSettings = newSettings;
        for (int i = 0; i < flock.Count; i++)
        {
            flock[i].UpdateSettings(managerSettings);
        }
    }

    public void SpawnNewFunnel()
    {
        randSpawn = Random.Range(0, spawnPoints.Length);
        GameObject g = Instantiate(boidPrefab);
        g.transform.parent = spawnParent;
        g.transform.position = spawnPoints[randSpawn].position;
        g.transform.forward = spawnPoints[randSpawn].forward;
        Boid b = g.GetComponent<Boid>();

        if (attractor != null)
        {
            b.BoidInitialize(managerSettings, attractor);
        }
        else
        {
            b.BoidInitialize(managerSettings, null);
        }

        AddNewBoid(b);
    }

    public void RemoveFunnel()
    {
        Boid tempB = flock[0];
        flock.Remove(flock[0]);
        Destroy(tempB.gameObject);
    }

    public void AddNewBoid(Boid newBoid)
    {
        flock.Add(newBoid);
        attractor.AddRepellers(newBoid);
    }

    public void AddNewMine(Mines newMine)
    {
        repellers.Add(newMine);
        for (int i = 0; i < flock.Count; i++)
        {
            flock[i].AddRepellers(newMine);
        }
    }
}
