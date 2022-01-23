using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// [�e�X�g�p]�e�摜�̃T�C�Y��\�����邾���̃e�L�X�g
/// </summary>
public class DebugUISizeText : MonoBehaviour
{
    //
    private RectTransform parentImage;      // �e�摜��transform
    private RectTransform selfTransform;    // ���g��transform
    private Text selfText;                  // ���g��Text�R���|�[�l���g


    // Start is called before the first frame update
    void Start()
    {
        parentImage = this.transform.parent.GetComponent<RectTransform>();  // �e���擾
        selfTransform = this.GetComponent<RectTransform>();                 // ���g��transform���擾                 
        selfText = this.GetComponent<Text>();                               // Text�R���|�[�l���g���擾
    }

    // Update is called once per frame
    void Update()
    {
        if(parentImage)
        {
            // ���g�̃T�C�Y�𒲐�
            selfTransform.sizeDelta = parentImage.sizeDelta;

            // �e�L�X�g���X�V
            selfText.text = parentImage.sizeDelta.x + " x " + parentImage.sizeDelta.y;
        }
    }
}
