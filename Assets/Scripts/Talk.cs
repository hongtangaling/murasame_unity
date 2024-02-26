using Live2D.Cubism.Core;
using Live2D.Cubism.Framework.Motion;
using Live2D.Cubism.Framework.MouthMovement;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Talk : MonoBehaviour
{
    static private string msg = "";
    private AudioSource audioSource;
    string url = "http://localhost:8080/murasamesing/Change";
    List<object> dataList;
    private GameObject RepText;


    static public string keyword;


    void Start()
    {
        // ����
        audioSource = gameObject.AddComponent<AudioSource>();
        dataList = new List<object>();

        RepText = GameObject.Find("RepText");

    }

    public void demo(string content)
    {
        //chatMessage.content = "";
        //chatMessage.role = "";
        // ׼��Ҫ���͵�JSON����
        ChatMessage chatMessage = new ChatMessage
        {
            role = "user",
            content = content
        };
        // �ж�׼��Ҫ���͵�JSON�������Ƿ��д������ݣ����������ݲ�ִ��
        if (chatMessage.content==""|| chatMessage.role=="")
        {
            return;
        }
        // �������ݽ�������ת��Ϊjson����
        dataList.Add(chatMessage);
        string jsonData = JsonConvert.SerializeObject(dataList);
        // �����ݷ�������
        Debug.Log(jsonData);
        StartCoroutine(GetSpeech(jsonData));
        // �������֮���ʼ������
        // chatMessage.content = "";
        // chatMessage.role = "";
    }

    //����vits�����ӿ�
    IEnumerator DownloadAndConfigureCubismInput(string msg)
    {
    string audioApiUrl = "http://127.0.0.1:23456/voice/vits?text=" + msg + "&id=4"; // �����ƵAPI��ַ

    UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(audioApiUrl, AudioType.WAV);

        // ���ó�ʱʱ��Ϊʮ��
        www.timeout = 10;

        // ����Э�̵ȴ��������
        yield return StartCoroutine(WaitForWebRequest(www));

        // ������ɺ�Ĵ���
        if (www.isDone)
        {
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + www.error);
            }
            else
            {
                AudioClip audioClip = DownloadHandlerAudioClip.GetContent(www);
                audioSource.clip = audioClip;

                // ����CubismAudioMouthInput���
                ConfigureCubismAudioInput(audioSource);

                // ������Ƶ
                audioSource.Play();
            }
        }
        else
        {
            // ����ʱ
            Debug.LogError("Request timed out after 10 seconds.");
        }

        www.Dispose();
    }

    IEnumerator WaitForWebRequest(UnityWebRequest www)
    {
        yield return www.SendWebRequest();
    }

    void ConfigureCubismAudioInput(AudioSource audioSource)
    {
        // ��ȡCubismAudioMouthInput���
        CubismAudioMouthInput cubismAudioInput = GetComponent<CubismAudioMouthInput>();

        if (cubismAudioInput != null)
        {
            // ��AudioSource����Ϊ����
            cubismAudioInput.AudioInput = audioSource;

            // ���Ը�����Ҫ��������������Ŀ����������ȡ������ƽ����
            // cubismAudioInput.SamplingQuality = CubismAudioMouthInput.SamplingQuality.High;
            // cubismAudioInput.Gain = 1f;
            // cubismAudioInput.Smoothing = 5;
        }
        else
        {
            Debug.LogError("CubismAudioMouthInput component not found on this GameObject.");
        }
    }



    // ���˷�������
    public IEnumerator GetSpeech(string _sendData)
    {
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(_sendData);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(data);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            string _msg = request.downloadHandler.text;
            JObject jsonData = JObject.Parse(_msg);
            if ((string)jsonData["stateNum"]=="200")
            {
                keyword = (string)jsonData["data"]["keyword"];
                Debug.Log(keyword);
                
                //��ȡ����Ϣ֮������ı�
                RepText.GetComponent<Text>().text = keyword;

                Debug.Log((string)jsonData["data"]["content"]);
                StartCoroutine(DownloadAndConfigureCubismInput((string)jsonData["data"]["content"]));
            }
        }
    }

    [System.Serializable]
    public class ChatMessage
    {
        public string role;
        public string content;
        
    }
    public class ResData
    {
        public string stateNum;
        public class data
        {
            public string rid;
            public string content;
            public string keyword;
            public string emotion;

        }

    }
}
