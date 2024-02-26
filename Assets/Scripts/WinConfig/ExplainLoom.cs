using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class ExplainLoom : MonoBehaviour
{
    static bool LeftMouseDown = false;
    static PointF RelativePosition=new PointF(0f,0f);

    // 消息处理主循环
    void Update()
    {


        //鼠标消息
        if (Input.GetMouseButtonDown(0))
        {
            print("鼠标左键被按下！");
            LeftMouseDown = true;

            RelativePosition.X = Config.PositionXItem.Param - MouseInformation.WorldX;
            RelativePosition.Y = Config.PositionYItem.Param - MouseInformation.WorldY;

        }
        if (Input.GetMouseButtonUp(0))
        {
            print("鼠标左键被松开！");
            LeftMouseDown = false;

        }
        if (Input.GetMouseButtonDown(1))
        {
            print("鼠标右键被按下！");
        }
        if (Input.GetMouseButtonDown(2))
        {
            print("鼠标中键被按下！");
        }
        if (Input.GetMouseButtonDown(3))
        {
            print("鼠标侧键被按下！");
        }

        //Debug.Log(MouseInformation.ChangeColor.r +"  " +MouseInformation.ChangeColor.g+"   "+ MouseInformation.ChangeColor.b);

        if (LeftMouseDown)
        {
            if (MouseInformation.ChangeColor.r == 0 && MouseInformation.ChangeColor.g == 0 && MouseInformation.ChangeColor.b == 0)
            {
            }
            else
            {
                Config.PositionXItem.Param = RelativePosition.X + MouseInformation.WorldX;
                Config.PositionYItem.Param = RelativePosition.Y + MouseInformation.WorldY;

                this.transform.position = new Vector3(Config.PositionXItem.Param, Config.PositionYItem.Param,0);
            }
        }



    }


}
