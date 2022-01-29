using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class PlayerHPBar : MonoBehaviour
{
    // 最大HPと現在のHP
    int maxHp = 155;
    int currentHp;

    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        // Sliderを満タンにする
        slider.value = 1;
        // 現在のHPを最大HPと同じにする
        currentHp = maxHp;
        Debug.Log("Start currentHp : " + currentHp);
    }

    // ColliderオブジェクトのIsTriggerにチェックを入れること
    private void OntriggerEnter(Collider collider)
    {
        // Enemyタグのオブジェクトに触れると発動
        if (collider.gameObject.tag == "Enemy")
        {
            // ダメージは1~100の中でランダムで決める
            int damage = Random.Range(1, 100);
            Debug.Log("damage : " + damage);

            // 現在のHPからダメージをひく
            currentHp = currentHp - damage;
            Debug.Log("After currentHp : " + currentHp);

            // 最大HPにおける現在のHPをSliderに反映
            // int同士の割り算は小数点以下は0になるので、(float)をつけてfloatの変数として振舞わせる
            slider.value = (float)currentHp / (float)maxHp; ;
            Debug.Log("slider.value : " + slider.value);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
