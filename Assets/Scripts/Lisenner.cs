using Live2D.Cubism.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private CubismModel model;
    private float timeCount;        //ʱ�亯��
   

    void Start()
    {
        model = this.FindCubismModel();     //��ȡ��ǰ��ģ��
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LateUpdate()
    {

        //������28������
        double timeSec = Time.time;
        double value = timeSec * 2 * Math.PI;

        // ����
        CubismParameter parameter = model.Parameters[28];
        parameter.Value = (float)(0.5f + 0.5f * Math.Sin(value / 3.2345));

        //��������ת��Ϊ��������
        Vector3 targetPosition = Input.mousePosition;
        targetPosition = Camera
            .main
            .ScreenToWorldPoint(new Vector3(
                targetPosition.x,
                targetPosition.y,
                0 - Camera.main.transform.position.z
                ));
        //����������ҡ��



        CubismParameter parameters = model.Parameters[29];
        CubismParameter parameters_dowm = model.Parameters[30];
        parameters.Value = targetPosition.x*3f;
        parameters_dowm.Value = targetPosition.y*9f;
    }
}
