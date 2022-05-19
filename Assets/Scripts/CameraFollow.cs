using System.Runtime.CompilerServices;
using System;
using System.Globalization;
using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public UnityEngine.Vector3 offset = new UnityEngine.Vector3(0.2F, 0.0F, -10F);
    public float dampingTime = 0.3F;
    public UnityEngine.Vector3 velocity = UnityEngine.Vector3.zero;

    void Awake()
    {
        Application.targetFrameRate = 60;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera(true);
    }

    public void ResetCameraPosition()
    {
        MoveCamera(false);
    }

    void MoveCamera(bool smooth)
    {
        UnityEngine.Vector3 destination = new UnityEngine.Vector3(
            target.position.x - offset.x,
            offset.y,
            offset.z
        );

        if(smooth)
        {
            this.transform.position = UnityEngine.Vector3.SmoothDamp(
                this.transform.position,
                destination,
                ref velocity,
                dampingTime
            );
        }
        else 
        {
            this.transform.position = destination;
        }
    }
}
