using Live2D.Cubism.Framework.LookAt;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubismBodyController : MonoBehaviour, ICubismLookTarget
{
    public Vector3 GetPosition()
    {
        Vector3 targetPosition = Input.mousePosition;
        targetPosition = Camera
            .main
            .ScreenToWorldPoint(new Vector3(
                targetPosition.x,
                targetPosition.y,
                0 - Camera.main.transform.position.z
                ));

        return targetPosition;
    }

    public bool IsActive()
    {
        return gameObject.activeInHierarchy;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
