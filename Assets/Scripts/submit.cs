using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class submit : MonoBehaviour
{
    private InputField inputField;
    private GameObject otherGameObject;
    // Start is called before the first frame update
    void Start()
    {
        inputField = GetComponent<InputField>();
        otherGameObject = GameObject.Find("Murasame");
        inputField.onEndEdit.AddListener(str =>
        {
            // Debug.Log(str);
            // Debug.Log("这里跑了1");
            if (otherGameObject != null)
            {
                // Debug.Log("这里跑了2");
                otherGameObject.GetComponent<Talk>().demo(str);
            }
            inputField.text = "";           //响应结束之后清除输入框
        });
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void demo()
    {
        // string text ;
        if (inputField == null)
        {
            Debug.Log("inputField为空");
            return;
        }
        Debug.Log(inputField.text);       
    }
}
