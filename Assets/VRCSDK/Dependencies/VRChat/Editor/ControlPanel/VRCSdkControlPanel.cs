﻿using System.Drawing;
using Pumkin.DataStructures;
using UnityEditor;
using UnityEngine;
using VRC.Core;
using VRC.Editor;
using VRC.SDKBase.Editor;
using VRCSDK.Dependencies.VRChat.Scripts.DenisHik;
using Color = UnityEngine.Color;
using FontStyle = UnityEngine.FontStyle;

[ExecuteInEditMode]
public partial class VRCSdkControlPanel : EditorWindow
{
    public static VRCSdkControlPanel window;

    [MenuItem("VRChat SDK/Show Control Panel", false, 600)]
    static void ShowControlPanel()
    {
        if (!ConfigManager.RemoteConfig.IsInitialized())
        {
            VRC.Core.API.SetOnlineMode(true, "vrchat");
            ConfigManager.RemoteConfig.Init(() => ShowControlPanel());
            return;
        }

        window = (VRCSdkControlPanel)EditorWindow.GetWindow(typeof(VRCSdkControlPanel));
        window.titleContent.text = "VRChat SDK";
        window.minSize = new Vector2(SdkWindowWidth + 4, 600);
        window.maxSize = new Vector2(SdkWindowWidth + 4, 2000);
        window.Init();
        window.Show();
    }

    public static GUIStyle titleGuiStyle;
    public static GUIStyle boxGuiStyle;
    public static GUIStyle infoGuiStyle;
    public static GUIStyle listButtonStyleEven;
    public static GUIStyle listButtonStyleOdd;
    public static GUIStyle listButtonStyleSelected;
    public static GUIStyle scrollViewSeparatorStyle;
    public static GUIStyle searchBarStyle;
    private bool isSettings = false;
    private bool isAcc = false;
    private int showPanelOld = 0;

    void InitializeStyles()
    {
        titleGuiStyle = new GUIStyle();
        titleGuiStyle.fontSize = 15;
        titleGuiStyle.fontStyle = FontStyle.BoldAndItalic;
        titleGuiStyle.alignment = TextAnchor.MiddleCenter;
        titleGuiStyle.wordWrap = true;
        if (EditorGUIUtility.isProSkin)
            titleGuiStyle.normal.textColor = Color.white;
        else
            titleGuiStyle.normal.textColor = Color.black;

        boxGuiStyle = new GUIStyle();
        if (EditorGUIUtility.isProSkin)
        {
            boxGuiStyle.normal.background = CreateBackgroundColorImage(new Color(0.3f, 0.3f, 0.3f));
            boxGuiStyle.normal.textColor = Color.white;
        }
        else
        {
            boxGuiStyle.normal.background = CreateBackgroundColorImage(new Color(0.85f, 0.85f, 0.85f));
            boxGuiStyle.normal.textColor = Color.black;
        }

        infoGuiStyle = new GUIStyle();
        infoGuiStyle.wordWrap = true; ;
        if (EditorGUIUtility.isProSkin)
            infoGuiStyle.normal.textColor = Color.white;
        else
            infoGuiStyle.normal.textColor = Color.black;
        infoGuiStyle.margin = new RectOffset(10, 10, 10, 10);

        listButtonStyleEven = new GUIStyle();
        listButtonStyleEven.margin = new RectOffset(0, 0, 0, 0);
        listButtonStyleEven.border = new RectOffset(0, 0, 0, 0);
        if (EditorGUIUtility.isProSkin)
        {
            listButtonStyleEven.normal.textColor = new Color(0.8f, 0.8f, 0.8f);
            listButtonStyleEven.normal.background = CreateBackgroundColorImage(new Color(0.540f, 0.540f, 0.54f));
        }
        else
        {
            listButtonStyleEven.normal.textColor = Color.black;
            listButtonStyleEven.normal.background = CreateBackgroundColorImage(new Color(0.85f, 0.85f, 0.85f));
        }

        listButtonStyleOdd = new GUIStyle();
        listButtonStyleOdd.margin = new RectOffset(0, 0, 0, 0);
        listButtonStyleOdd.border = new RectOffset(0, 0, 0, 0);
        if (EditorGUIUtility.isProSkin)
        {
            listButtonStyleOdd.normal.textColor = new Color(0.8f, 0.8f, 0.8f);
            //listButtonStyleOdd.normal.background = CreateBackgroundColorImage(new Color(0.50f, 0.50f, 0.50f));
        }
        else
        {
            listButtonStyleOdd.normal.textColor = Color.black;
            listButtonStyleOdd.normal.background = CreateBackgroundColorImage(new Color(0.90f, 0.90f, 0.90f));
        }

        listButtonStyleSelected = new GUIStyle();
        listButtonStyleSelected.normal.textColor = Color.white;
        listButtonStyleSelected.margin = new RectOffset(0, 0, 0, 0);
        if (EditorGUIUtility.isProSkin)
        {
            listButtonStyleSelected.normal.textColor = new Color(0.8f, 0.8f, 0.8f);
            listButtonStyleSelected.normal.background = CreateBackgroundColorImage(new Color(0.4f, 0.4f, 0.4f));
        }
        else
        {
            listButtonStyleSelected.normal.textColor = Color.black;
            listButtonStyleSelected.normal.background = CreateBackgroundColorImage(new Color(0.75f, 0.75f, 0.75f));
        }

        scrollViewSeparatorStyle = new GUIStyle("Toolbar");
        scrollViewSeparatorStyle.fixedWidth = SdkWindowWidth + 10;
        scrollViewSeparatorStyle.fixedHeight = 4;
        scrollViewSeparatorStyle.margin.top = 1;

        searchBarStyle = new GUIStyle("Toolbar");
        searchBarStyle.fixedWidth = SdkWindowWidth;
        searchBarStyle.fixedHeight = 23;
        searchBarStyle.padding.top = 3;

    }

    void Init()
    {
        InitializeStyles();
        ResetIssues();
        InitAccount();
    }

    void OnEnable()
    {
        OnEnableAccount();
        AssemblyReloadEvents.afterAssemblyReload += BuilderAssemblyReload;
    }

    void OnDisable()
    {
        AssemblyReloadEvents.afterAssemblyReload -= BuilderAssemblyReload;
    }

    void OnDestroy()
    {
        AccountDestroy();
    }

    public const int SdkWindowWidth = 518;

    private readonly GUIContent[] _toolbarLabels = new GUIContent[2]
    {
        new GUIContent("Builder"),
        new GUIContent("Content Manager"),
    };

    private readonly bool[] _toolbarOptionsLoggedIn = new bool[2] {true, true};
    private readonly bool[] _toolbarOptionsNotLoggedIn = new bool[2] {false, false};

    void OnGUI()
    {
        if (window == null)
        {
            window = (VRCSdkControlPanel)EditorWindow.GetWindow(typeof(VRCSdkControlPanel));
            InitializeStyles();
        }

        if (_bannerImage == null)
            _bannerImage = Resources.Load<Texture2D>("SDK_Panel_Banner");

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical();

        GUILayout.Box(_bannerImage);

        CustomUI.ShowSettingsButton(isSettings, showPanelOld, SdkWindowWidth, settings =>
        {
            isSettings = settings;
            isAcc = false;
        });
        CustomUI.ShowUserBlock(() =>
        {
            isSettings = false;
            isAcc = !isAcc;
        });


        if (Application.isPlaying)
        {
            GUI.enabled = false;
            GUILayout.Space(20);
            EditorGUILayout.LabelField("Unity Application is running ...\nStop it to access the Control Panel", titleGuiStyle, GUILayout.Width(SdkWindowWidth));
            GUI.enabled = true;
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            return;
        }

        EditorGUILayout.Space();

        EnvConfig.SetActiveSDKDefines();

        int showPanel = GUILayout.Toolbar(VRCSettings.ActiveWindowPanel, _toolbarLabels, (isSettings || isAcc) ? new bool[2] {false, false} : APIUser.IsLoggedIn ? _toolbarOptionsLoggedIn : _toolbarOptionsNotLoggedIn,  null, GUILayout.Width(SdkWindowWidth));

        // Only show Account or Settings panels if not logged in
        if (APIUser.IsLoggedIn == false && showPanel != 3)
        {
            isAcc = true;
        }

        if (showPanel != VRCSettings.ActiveWindowPanel && !isSettings && !isAcc)
        {
            VRCSettings.ActiveWindowPanel = showPanel;
            showPanelOld = showPanel;
        }

        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        if (isSettings)
        {
            ShowSettings();
        }

        if (isAcc)
        {
            ShowAccount();
        }
        else
        {
            switch (showPanel)
            {
                case 0:
                    ShowBuilders();
                    break;
                case 1:
                    ShowContent();
                    break;
                default:
                    ShowBuilders();
                    break;
            }
        }
    }

    [UnityEditor.Callbacks.PostProcessScene]
    static void OnPostProcessScene()
    {
        if (window != null)
            window.Reset();
    }

    private void OnFocus()
    {
        Reset();
    }

    public void Reset()
    {
        ResetIssues();
        // style backgrounds may be nulled on scene load. detect if so has happened
        if((boxGuiStyle != null) && (boxGuiStyle.normal.background == null))
            InitializeStyles();
    }

    [UnityEditor.Callbacks.DidReloadScripts(int.MaxValue)]
    static void DidReloadScripts()
    {
        RefreshApiUrlSetting();
    }
}
