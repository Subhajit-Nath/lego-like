using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefCubeIndicator : MonoBehaviour
{
    [SerializeField] private Material matGreen;
    [SerializeField] private Material matRed;
    private int cubeLayer;

    private void Awake()
    {
        cubeLayer = LayerMask.NameToLayer("CubeLayer");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == cubeLayer)
        {
            Bounds bounds = GetComponent<Collider>().bounds;
            bounds.size *= 0.9f;
            if (bounds.Intersects(other.bounds))
            {
                SetRed();
            }
            else
            {
                SetGreen();
            }
        }
        else
        {
            SetGreen();
        }
    }

    private void SetGreen()
    {
        GetComponent<Renderer>().sharedMaterial = matGreen;
        Spawner.canSpawn = true;
    }

    private void SetRed()
    {
        GetComponent<Renderer>().sharedMaterial = matRed;
        Spawner.canSpawn = false;
    }

    // private void OnTriggerExit(Collider other)
    // {
    //     if (other.gameObject.layer == cubeLayer)
    //     {
    //         GetComponent<Renderer>().sharedMaterial = matGreen;
    //         Spawner.canSpawn = true;
    //     }
    // }
}
