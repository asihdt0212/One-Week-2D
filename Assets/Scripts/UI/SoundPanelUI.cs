using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundPanelUI : MonoBehaviour
{
    [SerializeField, Header("BGMSlider")]
    private Slider bgmSlider;

    [SerializeField, Header("SESlider")]
    private Slider seSlider;

    //基礎ボリューム値を下げておく
    float soundVolumeRate = .5f;

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
