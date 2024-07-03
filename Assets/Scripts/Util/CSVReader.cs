using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

public class CSVReader
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    public static List<Dictionary<string, object>> Read(string path) // string -> TextAsset  으로 변경. 파일 링크로 읽기
    {
        var list = new List<Dictionary<string, object>>();
        var data = Resources.Load<TextAsset>(path); // 를 삭제.file 자체가 TextAsset이기 때문

        var lines = Regex.Split(data.text, LINE_SPLIT_RE);

        if (lines.Length <= 1) return list;

        var header = Regex.Split(lines[0], SPLIT_RE);
        for (var i = 1; i < lines.Length; i++)
        {

            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;

            var entry = new Dictionary<string, object>();
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                object finalvalue = value;
                int n;
                float f;
                if (int.TryParse(value, out n))
                {
                    finalvalue = n;
                }
                else if (float.TryParse(value, out f))
                {
                    finalvalue = f;
                }
                entry[header[j]] = finalvalue;
            }
            list.Add(entry);
        }
        return list;
    }
    public static List<Dialog> ReadDialog(TextAsset data)
    {
        var list = new List<Dialog>();

        var lines = Regex.Split(data.text, LINE_SPLIT_RE);

        if (lines.Length <= 1) return list;

        var header = Regex.Split(lines[0], SPLIT_RE);
        for (var i = 1; i < lines.Length; i++)
        {

            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;

            var entry = new Dictionary<string, object>();
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {

                values[j] = values[j].TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
            }
            string[] branchContents = new string[4] { values[4], values[5], values[6], values[7] };

            int[] links = new int[4] { string.IsNullOrEmpty(values[8]) ? 0 : int.Parse( values[8]),
                string.IsNullOrEmpty(values[9]) ? 0 : int.Parse( values[9]),
                string.IsNullOrEmpty(values[10]) ? 0 : int.Parse(values[10]),
                string.IsNullOrEmpty(values[11]) ? 0 : int.Parse(values[11]) };
            list.Add(new Dialog(int.Parse(values[0]), values[1], values[2], int.Parse(values[3]), branchContents, links, values[12], values[13]));
        }
        return list;
    }
}