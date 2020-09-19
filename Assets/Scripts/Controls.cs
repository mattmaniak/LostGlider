using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public float playerSpeed = 2.0f;
    public Transform playerTransform;

    void Start()
    {
        
    }

    void Update()
    {
        _movePlayerByKeyboard(new Vector2(Input.GetAxis("Horizontal"),
                                          Input.GetAxis("Vertical")));
    }

    [Obsolete("Use this method only for testing and debugging purposes.")]
    void _movePlayerByKeyboard(Vector2 direction)
    {
        playerTransform.Translate(direction * playerSpeed * Time.deltaTime);
    }

}
