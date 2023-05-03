using System.Linq;
using UnityEditor;
using UnityEngine;
using VRC.Core;

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
    }
}