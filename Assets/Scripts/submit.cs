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
            // Debug.Log("��������1");
            if (otherGameObject != null)
            {
                // Debug.Log("��������2");
                otherGameObject.GetComponent<Talk>().demo(str);
            }
            inputField.text = "";           //��Ӧ����֮����������
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
            Debug.Log("inputFieldΪ��");
            return;
        }
        Debug.Log(inputField.text);       
    }
}
