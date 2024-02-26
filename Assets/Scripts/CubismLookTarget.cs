using Live2D.Cubism.Core;
using Live2D.Cubism.Framework.LookAt;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CubismLookTarget : MonoBehaviour, ICubismLookTarget
{

    public Vector3 GetPosition()
    {
        //if (Input.GetAxisRaw("Mouse X") == 0 || Input.GetAxisRaw("Mouse Y") == 0)
        //{
        //    return Vector3.zero;
        //}
        //else
        //{
        // —€«Ú∏˙ÀÊ Û±Í“∆∂Ø
        Vector3 targetPosition = Input.mousePosition;
            targetPosition = Camera
                .main
                .ScreenToWorldPoint(new Vector3(
                    targetPosition.x, 
                    targetPosition.y, 
                    0 - Camera.main.transform.position.z
                    ));
        return targetPosition;
        //}
    }

    private void LateUpdate()
    {
        
    }
    public bool IsActive()
    {
        return gameObject.activeInHierarchy;
    }
}
