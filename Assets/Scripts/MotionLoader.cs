using Live2D.Cubism.Framework.Motion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motion : MonoBehaviour
{
    private CubismMotionController  cubismMotionController;
    // Start is called before the first frame update
    void Start()
    {
        cubismMotionController = GetComponent<CubismMotionController>();
    }

    // �����Դ����������ļ�
    public void PlayMotion(AnimationClip animationClip)
    {
        if (!cubismMotionController||!animationClip)
        {
            Debug.LogError("���������߲��Ŷ�������ԴΪ��");
            return;
        }
        cubismMotionController.PlayAnimation(animationClip,isLoop:false);
    }
}
