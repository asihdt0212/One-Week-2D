using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SoundManager.Instance.SoundSEPlay("SE1");
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            SoundManager.Instance.SoundBGMPlay("BGM1");
        }
    }
}
