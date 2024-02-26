using Live2D.Cubism.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parameter : MonoBehaviour
{
    private CubismModel model;
    private float timeCount;



    // Update is called once per frame
    void Update()
    {
        
    }
    private void LateUpdate()
    {
        timeCount += Time.deltaTime * 4;
        float value = Mathf.Sin(timeCount) * 30f;
        CubismParameter parameter = model.Parameters[2];
        parameter.Value = value;
    }
}
