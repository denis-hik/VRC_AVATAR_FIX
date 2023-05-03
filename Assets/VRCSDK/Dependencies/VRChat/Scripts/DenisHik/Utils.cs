using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Pumkin.Dependencies;
using UnityEngine;
using UnityEngine.Networking;

namespace VRCSDK.Dependencies.VRChat.Scripts.DenisHik
{
    public class Utils
    {
        public static Texture2D Resize(Texture2D texture2D,int targetX,int targetY)
        {
            RenderTexture rt=new RenderTexture(targetX, targetY,24);
            RenderTexture.active = rt;
            Graphics.Blit(texture2D,rt);
            Texture2D result=new Texture2D(targetX,targetY);
            result.ReadPixels(new Rect(0,0,targetX,targetY),0,0);
            result.Apply();
            return result;
        }
    }
}