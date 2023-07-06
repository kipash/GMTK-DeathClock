using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

[InitializeOnLoad]
public class ToolbarCustom
{
    static ToolbarSettings settings;
    static ToolbarCustom()
    {
        UnityToolbarExtender.ToolbarCallback.OnToolbarGUILeft += OnLeftGUI;
        UnityToolbarExtender.ToolbarCallback.OnToolbarGUIRight += OnRightGUI;

        var settingsGuid = AssetDatabase.FindAssets("t:ToolbarSettings").FirstOrDefault();
        if(settingsGuid != null)
        {
            settings = AssetDatabase.LoadAssetAtPath<ToolbarSettings>(AssetDatabase.GUIDToAssetPath(settingsGuid));
        }
        else
        {
            settings = ScriptableObject.CreateInstance<ToolbarSettings>();
            AssetDatabase.CreateAsset(settings, "Assets/ToolbarSettings.asset");
        }
    }

    static void OnLeftGUI()
    {
        GUILayout.BeginHorizontal(GUILayout.Width(Screen.width * .5f - 180));
        GUILayout.FlexibleSpace();

        GUILayout.Label(PlayerSettings.productName);

        GUILayout.Space(20);

        var icon = PlayerSettings.GetIconsForTargetGroup(BuildTargetGroup.Unknown).FirstOrDefault();
        if (icon != null)
            DrawTexture(icon, leftAligned: false, addSpace: true, 30, 30, new Vector2(3, -3));

        GUILayout.Space(20);

        var gmtkLogoPath = AssetDatabase.GUIDToAssetPath("fee2ca0a5120a944c9a94cbeda162f8a");
        var gmtkLogo = AssetDatabase.LoadAssetAtPath<Texture>(gmtkLogoPath);
        var logoScale = 0.4f;

        DrawTexture(gmtkLogo, leftAligned: false, addSpace: true, 128 * logoScale, 64 * logoScale, new Vector2(2, 0));

        GUILayout.Label($"Theme: {settings.Theme}");

        GUILayout.Space(20);

        var culturue = CultureInfo.GetCultureInfo("en-US");
        var format = "yyyy/MM/dd_HH:mm:ss";
        var deadline = DateTime.ParseExact(settings.DeadlineDateTime, format, culturue);

        var diff = deadline.Subtract(DateTime.UtcNow);

        var deathClockStyle = new GUIStyle(EditorStyles.label);
        deathClockStyle.fontSize = 16;
        deathClockStyle.normal.textColor = Color.gray;


        GUILayout.Label($"{diff.TotalHours:00}:{diff.Minutes:00}:{diff.Seconds:00}", deathClockStyle, GUILayout.Width(70));

        GUILayout.Space(20);

        GUILayout.EndHorizontal();
    }

    static void OnRightGUI()
    {
        GUILayout.BeginHorizontal();

        GUILayout.Space(10);

        var buttonStyle = new GUIStyle(EditorStyles.miniButton);
        buttonStyle.normal.textColor = Color.white;
        buttonStyle.fontSize = 14;
        buttonStyle.alignment = TextAnchor.UpperCenter;

        int spacing = 10;

        if (settings.ListScenes && EditorBuildSettings.scenes.Length != 0)
        {
            buttonStyle.fontStyle = FontStyle.Bold;

            foreach (var x in EditorBuildSettings.scenes)
            {
                var name = Path.GetFileNameWithoutExtension(x.path);
                if (GUILayout.Button(name, buttonStyle))
                    OpenScene(x.path);

                GUILayout.Space(spacing);
            }

            GUILayout.Label("|");

            GUILayout.Space(spacing);
        }

        buttonStyle.fontStyle = FontStyle.Normal;

        foreach (var x in settings.CustomLinks)
        {
            if (GUILayout.Button(x.Name, buttonStyle))
                Application.OpenURL(x.URL);

            GUILayout.Space(spacing);
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    static void OpenScene(string path)
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene(path);
    }

    static void DrawTexture(Texture tex, bool leftAligned, bool addSpace, float width, float height, Vector2 offset)
    {
        GUILayout.Label("");
        var rect = GUILayoutUtility.GetLastRect();

        rect.width = width;
        rect.height = height;
        rect.x -= (leftAligned ? rect.width : 0) - offset.x;
        rect.y -= 4 - offset.y;

        if (leftAligned && addSpace)
            GUILayout.Space(rect.width);

        GUI.DrawTexture(rect, tex);

        if (!leftAligned && addSpace)
            GUILayout.Space(rect.width);
    }
}