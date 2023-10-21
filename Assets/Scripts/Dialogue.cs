using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//인스펙터창에서 수정가능
[System.Serializable]
public class Dialgoue
{
    [Tooltip("대사 내용")]
    public string[] contexts;

    [Tooltip("이벤트 번호")]
    public string[] number;

    [Tooltip("스킵라인")]
    public string[] skipnum;
    //[Tooltip("이벤트번호")]
    //public string[] number;

}

[System.Serializable]
public class DialgoueEvent
{
    //이벤트 이름
    public string name;

    //public Vector2 line;
    public Dialgoue[] dialgoues;

}