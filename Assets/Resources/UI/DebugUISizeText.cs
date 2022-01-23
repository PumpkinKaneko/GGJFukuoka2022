using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// [テスト用]親画像のサイズを表示するだけのテキスト
/// </summary>
public class DebugUISizeText : MonoBehaviour
{
    //
    private RectTransform parentImage;      // 親画像のtransform
    private RectTransform selfTransform;    // 自身のtransform
    private Text selfText;                  // 自身のTextコンポーネント


    // Start is called before the first frame update
    void Start()
    {
        parentImage = this.transform.parent.GetComponent<RectTransform>();  // 親を取得
        selfTransform = this.GetComponent<RectTransform>();                 // 自身のtransformを取得                 
        selfText = this.GetComponent<Text>();                               // Textコンポーネントを取得
    }

    // Update is called once per frame
    void Update()
    {
        if(parentImage)
        {
            // 自身のサイズを調整
            selfTransform.sizeDelta = parentImage.sizeDelta;

            // テキストを更新
            selfText.text = parentImage.sizeDelta.x + " x " + parentImage.sizeDelta.y;
        }
    }
}
