using System;
using Mirror;
using UnityEngine;

namespace Network.Scripts.NetworkCore.Monitoring
{
    public class PingDisplay : MonoBehaviour
    {
        public Color color = Color.white;
        public Vector2 padding;
        private int width => Screen.width;
        private int height => Screen.height;

        private void OnGUI()
        {
            if (!NetworkClient.active) return;

            GUI.color = color;
            Rect rect = new Rect(padding.x, padding.y, width, height);
            GUIStyle style = GUI.skin.GetStyle("Label");
            style.fontSize = (int)(Screen.width / 20f);
            style.fontStyle = FontStyle.Bold;
            style.alignment = TextAnchor.LowerRight;
            GUI.Label(rect, $"Ping: {Math.Round(NetworkTime.rtt * 1000)}ms", style);
            
        }
    }
}