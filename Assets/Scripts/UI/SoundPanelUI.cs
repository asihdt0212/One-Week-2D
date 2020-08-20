using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundPanelUI : MonoBehaviour
{
    public static SoundPanelUI instance;

    [SerializeField, Header("BGMSlider")]
    private Slider bgmSlider;

    [SerializeField, Header("SESlider")]
    private Slider seSlider;

    //基礎ボリューム値を下げておく
    float soundVolumeRate = .5f;

    private void Awake()
    {
        if(instance == null) { instance = this; }
        else { Destroy(this.gameObject); }
    }

    private void OnEnable()
    {
        bgmSlider.value = PlayerPrefs.GetFloat(SoundDefine.BGM_VOLUME, .5f);
        seSlider.value = PlayerPrefs.GetFloat(SoundDefine.SE_VOLUME, .5f);
        ChangedValue(true);
        ChangedValue(false);
    }

    public void SaveVolume()
    {
        PlayerPrefs.SetFloat(SoundDefine.BGM_VOLUME, bgmSlider.value);
        PlayerPrefs.SetFloat(SoundDefine.SE_VOLUME, seSlider.value);
    }

    public void ChangedValue(bool BGM)
    {
        var kind = (BGM == true) ? SoundManager.Kind.BGM : SoundManager.Kind.SE;
        float value = 0;
        if (kind == SoundManager.Kind.BGM) 
        {
            value = bgmSlider.value;
        }
        else
        {
            value = seSlider.value;
        }
        if (SoundManager.Instance != null) 
        {
            SoundManager.Instance.ChangeVolume(kind, value * soundVolumeRate);
        };
    }
}
