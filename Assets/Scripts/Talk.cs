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
        // 播放
        audioSource = gameObject.AddComponent<AudioSource>();
        dataList = new List<object>();

        RepText = GameObject.Find("RepText");

    }

    public void demo(string content)
    {
        //chatMessage.content = "";
        //chatMessage.role = "";
        // 准备要发送的JSON数据
        ChatMessage chatMessage = new ChatMessage
        {
            role = "user",
            content = content
        };
        // 判断准备要发送的JSON数据中是否有存在数据，不存在数据不执行
        if (chatMessage.content==""|| chatMessage.role=="")
        {
            return;
        }
        // 存在数据将该数据转换为json数据
        dataList.Add(chatMessage);
        string jsonData = JsonConvert.SerializeObject(dataList);
        // 带数据发送请求
        Debug.Log(jsonData);
        StartCoroutine(GetSpeech(jsonData));
        // 请求完毕之后初始化参数
        // chatMessage.content = "";
        // chatMessage.role = "";
    }

    //请求vits语音接口
    IEnumerator DownloadAndConfigureCubismInput(string msg)
    {
    string audioApiUrl = "http://127.0.0.1:23456/voice/vits?text=" + msg + "&id=4"; // 你的音频API地址

    UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(audioApiUrl, AudioType.WAV);

        // 设置超时时间为十秒
        www.timeout = 10;

        // 启动协程等待请求完成
        yield return StartCoroutine(WaitForWebRequest(www));

        // 请求完成后的处理
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

                // 配置CubismAudioMouthInput组件
                ConfigureCubismAudioInput(audioSource);

                // 播放音频
                audioSource.Play();
            }
        }
        else
        {
            // 请求超时
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
        // 获取CubismAudioMouthInput组件
        CubismAudioMouthInput cubismAudioInput = GetComponent<CubismAudioMouthInput>();

        if (cubismAudioInput != null)
        {
            // 将AudioSource设置为输入
            cubismAudioInput.AudioInput = audioSource;

            // 可以根据需要调整其他设置项目，如采样精度、增益和平滑度
            // cubismAudioInput.SamplingQuality = CubismAudioMouthInput.SamplingQuality.High;
            // cubismAudioInput.Gain = 1f;
            // cubismAudioInput.Smoothing = 5;
        }
        else
        {
            Debug.LogError("CubismAudioMouthInput component not found on this GameObject.");
        }
    }



    // 向后端发送请求
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
                
                //获取到信息之后更新文本
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
