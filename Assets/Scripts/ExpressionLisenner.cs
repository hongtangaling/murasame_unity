using Live2D.Cubism.Framework.Expression;
using Live2D.Cubism.Framework.Raycasting;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ExpressionLisenner : MonoBehaviour
{
    private CubismExpressionController controller;
    private int index;
    public float expressionDuration = 2.0f;
    private bool isWaiting = false;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CubismExpressionController>();
        index = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            Debug.Log("没有检查到");
            return;
        }
        CubismRaycaster cubismRaycaster = GetComponent<CubismRaycaster>();
        CubismRaycastHit[] cubismRaycastHits = new CubismRaycastHit[4];

        //射线实例
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int hitCount = cubismRaycaster.Raycast(ray, cubismRaycastHits);
        string resText = hitCount.ToString();
        for (int i = 0; i < hitCount; i++)
        {
            resText += "\n" + cubismRaycastHits[i].Drawable.name;
            if ((cubismRaycastHits[i].Drawable.name).ToString() == "ArtMesh70")
            {
                // Debug.Log("狗修金，你好坏啊！");
                controller.CurrentExpressionIndex = 4;
                // Thread.Sleep(5000);
                StartCoroutine(WaitAndResetExpression());

            }
            if ((cubismRaycastHits[i].Drawable.name).ToString() == "HitArea4")
            {
                controller.CurrentExpressionIndex = 5;
                StartCoroutine(WaitAndResetExpression());
            }
        }
        //controller.CurrentExpressionIndex = -1;
        Debug.Log(resText);
        
    }
    IEnumerator WaitAndResetExpression()
    {
        isWaiting = true;
        yield return new WaitForSeconds(expressionDuration);

        // 在等待一段时间后，将表情切换回默认表情
        controller.CurrentExpressionIndex = -1;
        isWaiting = false;
    }
}
