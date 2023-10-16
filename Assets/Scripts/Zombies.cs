using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombies : MonoBehaviour
{
    NavMeshAgent agent;

    // Raycast
    private bool isChaseMode;
    private float ray_dist = 10.0f; // Raycast Distance
    private RaycastHit hit1, hit2, hit3, hit4, hit5, hit6, hit7; // Racast Hits
    private Vector3 layPos;
    [SerializeField] private LayerMask layerMask = -1; // Layer Mask

    private void Awake()
    {
        isChaseMode = false;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void FixedUpdate()
    {
        layPos = transform.position + new Vector3(0, 1.3f, 0);
        // Setting Raycast
        Ray[] rays = {
            new Ray(layPos, (transform.forward + transform.right * -0.6f).normalized),
            new Ray(layPos, (transform.forward + transform.right * -0.4f).normalized),
            new Ray(layPos, (transform.forward + transform.right * -0.2f).normalized),
            new Ray(layPos, transform.forward),
            new Ray(layPos, (transform.forward + transform.right * 0.2f).normalized),
            new Ray(layPos, (transform.forward + transform.right * 0.4f).normalized),
            new Ray(layPos, (transform.forward + transform.right * 0.6f).normalized),
        };

        // Raycast Hits
        if (Physics.Raycast(rays[0], out hit1, ray_dist))
        {
            
        }
        if (Physics.Raycast(rays[1], out hit2, ray_dist))
        {

        }
        if (Physics.Raycast(rays[2], out hit3, ray_dist))
        {

        }
        if (Physics.Raycast(rays[3], out hit4, ray_dist))
        {

        }
        if (Physics.Raycast(rays[4], out hit5, ray_dist))
        {

        }
        if (Physics.Raycast(rays[5], out hit6, ray_dist))
        {

        }
        if (Physics.Raycast(rays[6], out hit7, ray_dist))
        {

        }

        DebugLayCastLine();
    }

    private void DebugLayCastLine()
    {
        Color cur_color;
        if (isChaseMode) cur_color = Color.red;
        else cur_color = Color.green;

        Debug.DrawLine(layPos, layPos + (transform.forward + transform.right * -0.6f).normalized * hit1.distance, cur_color);
        Debug.DrawLine(layPos, layPos + (transform.forward + transform.right * -0.4f).normalized * hit2.distance, cur_color);
        Debug.DrawLine(layPos, layPos + (transform.forward + transform.right * -0.2f).normalized * hit3.distance, cur_color);
        Debug.DrawLine(layPos, layPos + transform.forward * hit4.distance, cur_color);
        Debug.DrawLine(layPos, layPos + (transform.forward + transform.right * 0.2f).normalized * hit5.distance, cur_color);
        Debug.DrawLine(layPos, layPos + (transform.forward + transform.right * 0.4f).normalized * hit6.distance, cur_color);
        Debug.DrawLine(layPos, layPos + (transform.forward + transform.right * 0.6f).normalized * hit7.distance, cur_color);
    }
}
