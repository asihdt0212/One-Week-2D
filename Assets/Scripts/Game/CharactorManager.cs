using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorManager : MonoBehaviour
{
    public Pattern[] Patterns;
    //Patternリスト
    List<Pattern> ListPattern; 

    // Start is called before the first frame update
    void Start()
    {

        //
        ListPattern = new List<Pattern>();

        ListPattern.Add(new MovePattern());

        ListPattern[0].Move();

        ListPattern.Add(new MovePattern2());

        ListPattern[1].Move();


    }

    // Update is called once per frame
    void Update()
    {

    }
}
