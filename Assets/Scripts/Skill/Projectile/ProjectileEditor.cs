using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Projectile))]
public class ProjectileEditor : Editor
{
    SerializedProperty ProjectileType;

    SerializedProperty Height;

    SerializedProperty TargetPos;
    SerializedProperty Duration;

    private void OnEnable()
    {
        ProjectileType = serializedObject.FindProperty("ProjType");
        Height = serializedObject.FindProperty("Height");
        TargetPos = serializedObject.FindProperty("TargetPos");
        Duration = serializedObject.FindProperty("Duration");
    }
    public override void OnInspectorGUI()   
    {
        base.OnInspectorGUI();
        Projectile projectile = (Projectile)target;
        EditorGUILayout.BeginHorizontal();  
        GUILayout.FlexibleSpace(); 

        if (GUILayout.Button("투사체 실행", GUILayout.Width(120), GUILayout.Height(30)))
        {
            projectile.Shoot();
        }
        if (GUILayout.Button("투사체 재생성", GUILayout.Width(120), GUILayout.Height(30)))
        {
            projectile.TestRespawn();
        }
        GUILayout.FlexibleSpace();  
        EditorGUILayout.EndHorizontal();  
    }
}
