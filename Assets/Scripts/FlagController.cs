using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagController : MonoBehaviour
{
    private void Awake()
    {
        // Sets spawn y position for transform
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }
}
