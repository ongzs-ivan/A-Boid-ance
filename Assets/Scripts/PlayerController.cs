using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 25.0f;

    private BoidManager manager;
    private float rotationSpeed;

    private void Awake()
    {
        manager = BoidManager.instance;
    }

    private void Update()
    {
        
        PlayerRotation();
        
        // LMB -> Spawn funnel
        if (Input.GetMouseButtonDown(0))
        {
            manager.SpawnNewFunnel();
        }
        
        // RMB -> Queue funnel to return
        if (Input.GetMouseButtonDown(1))
        {
            manager.RemoveFunnel();
        }
    }

    // movement
    private void PlayerRotation()
    {
        rotationSpeed = speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.W))
        {
            transform.Rotate(1.0f * -rotationSpeed, 0.0f, 0.0f);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0.0f, 1.0f * -rotationSpeed, 0.0f);
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.Rotate(1.0f * rotationSpeed, 0.0f, 0.0f);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0.0f, 1.0f * rotationSpeed, 0.0f);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(0.0f, 0.0f , 1.0f * rotationSpeed);
        }

        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(0.0f, 0.0f, 1.0f * -rotationSpeed);
        }
    }
}
