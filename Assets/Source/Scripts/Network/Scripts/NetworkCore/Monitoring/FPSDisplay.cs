using UnityEngine;

namespace Network.Scripts.NetworkCore.Monitoring
{
    public class FPSDisplay: MonoBehaviour
    {
        public Color color = Color.white;
        public Vector2 padding;
        private int width => Screen.width;
        private int height => Screen.height;
        
        private float deltaTime = 0.0f;
 
        private void Update()
        {
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        }
 
        private void OnGUI()
        {
            GUIStyle style = GUI.skin.GetStyle("Label");
            style.fontSize = (int)(Screen.width / 20f);
            style.fontStyle = FontStyle.Bold;

            GUI.color = color;
            Rect rect = new Rect(  padding.x,  padding.y, width, height);
            style.alignment = TextAnchor.LowerLeft;
            float msec = deltaTime * 1000.0f;
            float fps = 1.0f / deltaTime;
            string text = $"{msec:0.0} ms ({fps:0.} fps)";
            GUI.Label(rect, text, style);
        }
    }
}