﻿using System.Linq;
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
            
            GUISkin customSkin = (GUISkin) Resources.Load("Styles\\StyleCustom");
            Texture2D icon = (Texture2D) Resources.Load("Images\\Settings Normal");
            Texture2D iconOn = (Texture2D) Resources.Load("Images\\Settings Active");
            
            if (GUI.Button(
                    new Rect (SdkWindowWidth - 50 , 10, 50, 30),
                    isSettings ? iconOn : icon, 
                    !isSettings ? customSkin.GetStyle("Button_Custom_settings_active") : customSkin.GetStyle("Button_Custom_settings"))
                )
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
        static public void ShowUserBlock(bool isActive, onClick onClick)
        {
            GUISkin customSkin;
            if (APIUser.CurrentUser == null)
            {
                return;
            }

            customSkin = (GUISkin) Resources.Load("Styles\\StyleCustom");
            Texture2D icon = (Texture2D) Resources.Load("Images\\user");
            string displayName = APIUser.CurrentUser.displayName;
            string url = APIUser.CurrentUser.currentAvatarImageUrl;
            string iconURL = APIUser.CurrentUser.userIcon;
            if (icon != null && iconURL.Length > 5)
            {
                url = iconURL;
            }

            if (customSkin.FindStyle("Button_Custom") != null)
            {
                if (GUI.Button(
                        new Rect(10, 10, 50, 30),
                        icon, 
                        !isActive ? customSkin.GetStyle("Button_Custom_settings_active") : customSkin.GetStyle("Button_Custom_settings"))
                        )
                {
                    onClick();
                }
            }
               
        }

        static public void ShowUpdateButton(onClick onClick)
        {
            GUIStyle style = new GUIStyle()
            {
                fixedWidth = 15,
                fixedHeight = 15,
                margin = new RectOffset(0, 10, 0, 0),
            };
            Texture2D repeatIcon = (Texture2D) Resources.Load("Images\\Refresh");
            
            if (GUILayout.Button(repeatIcon, style))
            {
                onClick();
            }
        }
        static public void ShowCopyButton(onClick onClick)
        {
            GUIStyle style = new GUIStyle()
            {
                fixedWidth = 15,
                fixedHeight = 15,
                margin = new RectOffset(0, 10, 0, 0),
            };
            Texture2D repeatIcon = (Texture2D) Resources.Load("Images\\Documents");
            
            if (GUILayout.Button(repeatIcon, style))
            {
                onClick();
            }
        }
        static public void ShowDeleteButton(int x, int y, onClick onClick)
        {
            GUIStyle style = new GUIStyle()
            {
                fixedWidth = 15,
                fixedHeight = 15,
                margin = new RectOffset(0, 10, 0, 0),
            };
            Texture2D deleteIcon = (Texture2D) Resources.Load("Images\\Delete");
            
            if (x > 0 || y > 0)
            {
                if (GUI.Button(new Rect(x, y, 30, 30), deleteIcon))
                {
                    onClick();
                }
            } else {
                if (GUILayout.Button(deleteIcon, style))
                {
                    onClick();
                }
            }
            
            
        }
        static public void ShowPlayButton(int x, int y, onClick onClick)
        {
            GUIStyle style = new GUIStyle()
            {
                fixedWidth = 15,
                fixedHeight = 15,
                margin = new RectOffset(0, 10, 0, 0),
            };
            Texture2D deleteIcon = (Texture2D) Resources.Load("Images\\Play");
            
            if (x > 0 || y > 0)
            {
                if (GUI.Button(new Rect(x, y, 30, 30), deleteIcon))
                {
                    onClick();
                }
            } else {
                if (GUILayout.Button(deleteIcon, style))
                {
                    onClick();
                }
            }
            
            
        }
        
        static public void ShowCopyIDButton(int x, int y, onClick onClick)
        {
            GUIStyle style = new GUIStyle()
            {
                fixedWidth = 15,
                fixedHeight = 15,
                margin = new RectOffset(0, 10, 0, 0),
            };
            Texture2D deleteIcon = (Texture2D) Resources.Load("Images\\ID");
            
            if (x > 0 || y > 0)
            {
                if (GUI.Button(new Rect(x, y, 30, 30), deleteIcon))
                {
                    onClick();
                }
            } else {
                if (GUILayout.Button(deleteIcon, style))
                {
                    onClick();
                }
            }
            
            
        }
    }
}