using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GameObject;
using DG.Tweening;

public class HomeCanvasUI : MonoBehaviour
{
    public static HomeCanvasUI instance;
    private Text homeHumanText;

    //テキストアニメーション用
    [SerializeField, Header("数え上げ時間間隔")]
    private float countSpan = default;
    private float textJumpForce = .5f;
    private int textJumpTime = 1;

    //数え上げ終えた時のスケール
    private readonly float bigScale = 2.0f;

    private void Awake()
    {
        if(instance  == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        LoadUI();
    }

    private void LoadUI()
    {
        homeHumanText = Find(HierarchyPath_Game.HomeCanvas.HomeHumanText).GetComponent<Text>();
        homeHumanText.gameObject.SetActive(false);
    }

    //人間カウンター文字セット
    public void SetHumanText(int humanCount, System.Action callback)
    {
        StopAllCoroutines();
        StartCoroutine(HumanCounterAnimation(humanCount, ()=> {
            Debug.Log("カウント終了");
            callback();
        }));
    }

    public void ShowHumanText(bool show)
    {
        homeHumanText.gameObject.SetActive(show);
    }

    //人間カウンター文字切り替えアニメーション
    private IEnumerator HumanCounterAnimation(int humanCount, System.Action callback)
    {
        homeHumanText.text = "";
        homeHumanText.transform.localScale = Vector3.one;
        homeHumanText.color = Color.white;
        ShowHumanText(true);
        int counter = 0;
        while (humanCount > 0)
        {
            counter++;
            humanCount--;
            homeHumanText.text = counter.ToString();
            homeHumanText.transform.DOJump(homeHumanText.transform.position, textJumpForce, textJumpTime, countSpan, false);

            //画像変更処理
            CharactorManager.Instance.ChangeCharacterSprite();

            if (humanCount != 0)
            {
                
                //カウント中
                SoundManager.Instance.SoundSEPlay(SoundDefine.SE_COUNT.key);
            }
            else
            {
                //最後の数字を数え終えた、文字拡大などで演出入れるとよし
                SoundManager.Instance.SoundSEPlay(SoundDefine.SE_COUNT_END.key);
                homeHumanText.transform.DOScale(Vector3.one * bigScale, .5f);
                homeHumanText.color = Color.red;
            }

            yield return new WaitForSeconds(countSpan);
        }

        callback();
    }
}
