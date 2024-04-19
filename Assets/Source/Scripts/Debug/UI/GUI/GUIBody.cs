using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Playstrom.Core.GameDebug
{
   public class GUIBody
   {
      protected GUISkin GUISkin;

      private const float REFERENCE_SCREEN_SIZE_WIDTH = 1080f;
      private const float REFERENCE_SCREEN_SIZE_HEIGHT = 1920f;
      private const float REFERENCE_SIZE_FONT = 52f;
      private const float REFERENCE_SIZE_BUTTON_HEIGHT = 60f;
      private const float REFERENCE_SIZE_BUTTON_WIDTH = 500f;

      protected float SizeFont => Screen.height > Screen.width
         ? REFERENCE_SIZE_FONT * (Screen.height / REFERENCE_SCREEN_SIZE_HEIGHT)
         : REFERENCE_SIZE_FONT * (Screen.width / REFERENCE_SIZE_BUTTON_WIDTH);

      protected float Width => REFERENCE_SIZE_BUTTON_WIDTH * (Screen.width / REFERENCE_SCREEN_SIZE_WIDTH);

      ///(1-1080f/Screen.width));
      // protected float Width => Screen.width / 2f;///(1-1080f/Screen.width));
      //protected float Width => 1080f/Screen.width*Screen.width/2f;
      protected float Height => REFERENCE_SIZE_BUTTON_HEIGHT * (Screen.height / REFERENCE_SCREEN_SIZE_HEIGHT);
      //protected float Height =>  Screen.height/(20f*(1920f/Screen.height));
      //protected float Height => 1920f/Screen.height*Screen.height/30f;

      protected List<CmdEvent> listCmdActions;

      protected virtual void Init(params object[] parameters)
      {

      }

      protected virtual void GUIStyleConstruct()
      {

      }

      public virtual void Init(List<CmdEvent> listCmdActions, GUISkin guiSkin)
      {
         GUISkin = guiSkin;
         this.listCmdActions = listCmdActions;
         GUIStyleConstruct();
      }

      public virtual void Draw()
      {

      }
   }
}