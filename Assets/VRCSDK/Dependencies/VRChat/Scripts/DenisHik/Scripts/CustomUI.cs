using System.Linq;
using Pumkin.DataStructures;
using UnityEditor;
using UnityEngine;
using VRC.Core;
using VRC.SDKBase.Editor;

namespace VRCSDK.Dependencies.VRChat.Scripts.DenisHik
{
    public delegate void OnCancel();
    public delegate void OnRetry();
    
    static public class CustomUI
    {
        [MenuItem("Example/Place Selection On Surface")]
        static public void ShowErrorModal(string text, OnCancel onClose, OnRetry onRetry)
        {
            if (EditorUtility.DisplayDialog("Error uploading avatar...", text, "Close", "Retry"))
            {
                onClose();
            }
            else
            {
                onRetry();
            }
        }

        public delegate void onSettings(bool isSettings);
        static public void ShowSettingsButton(bool isSettings, int showPanelOld, int SdkWindowWidth, onSettings onSettings)
        {
            if (GUI.Button(new Rect (SdkWindowWidth - 40 , 10, 30, 30), (Texture2D)EditorGUIUtility.IconContent("Settings").image))
            {
                isSettings = !isSettings;
                if (isSettings)
                {
                    VRCSettings.ActiveWindowPanel = -1;
                }
                else
                {
                    VRCSettings.ActiveWindowPanel = showPanelOld;
                }

                onSettings(isSettings);
            }
        }
        public delegate void onClick();
        static public void ShowUserBlock(onClick onClick)
        {
            GUISkin customSkin;
            if (APIUser.CurrentUser == null)
            {
                return;
            }

            customSkin = (GUISkin) Resources.Load("Styles\\StyleCustom");
            string displayName = APIUser.CurrentUser.displayName;
            string url = APIUser.CurrentUser.currentAvatarImageUrl;
            string icon = APIUser.CurrentUser.userIcon;
            if (icon != null && icon.Length > 5)
            {
                url = icon;
            }

            if (customSkin.FindStyle("Button_Custom") != null)
            {
                if (GUI.Button(new Rect(10, 10, 100, 30), displayName, customSkin.FindStyle("Button_Custom")))
                {
                    onClick();
                }
            }
               
        }
    }
}