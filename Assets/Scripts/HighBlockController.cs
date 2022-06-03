using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighBlockController : MonoBehaviour
{
    private void Awake()
    {
        // sets spawn y position for transform
        transform.position = new Vector3(transform.position.x, 2, transform.position.z);
    }
}
