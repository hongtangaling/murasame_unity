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

    // 播放自带动作数据文件
    public void PlayMotion(AnimationClip animationClip)
    {
        if (!cubismMotionController||!animationClip)
        {
            Debug.LogError("控制器或者播放动画的资源为空");
            return;
        }
        cubismMotionController.PlayAnimation(animationClip,isLoop:false);
    }
}
