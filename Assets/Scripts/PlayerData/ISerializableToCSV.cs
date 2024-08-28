using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CSV로 변환하기 위한 인터페이스입니다.
/// </summary>
public interface ISerializableToCSV
{

    public string ToCSV();
    
    public void FromCSV(string data);
}
