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
        ShowHumanText(true);
        int counter = 0;
        while (humanCount > 0)
        {
            counter++;
            humanCount--;
            homeHumanText.text = counter.ToString();
            homeHumanText.transform.DOJump(homeHumanText.transform.position, textJumpForce, textJumpTime, countSpan, false);
            yield return new WaitForSeconds(countSpan);
        }
        
        callback();
    }
}
