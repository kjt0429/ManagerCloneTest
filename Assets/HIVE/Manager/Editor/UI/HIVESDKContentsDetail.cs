using UnityEngine;
using UnityEditor;

using System;
using System.Reflection;

namespace hive.manager.editor
{
    public class HIVESDKContentsDetail : HIVEEditorWindow<HIVESDKContentsDetail> {
        
        static float width = 600;
        static float height = 500;
        static float marginX = 30;
        static float marginY = 30;

        string header;
        string updateDetail;
        Vector2 scrollpos = Vector2.zero;

        public static void ShowPopup(string title, string updateDetail) {
            var popup = ShowSingletonPopup();
            popup.titleContent = new GUIContent(title);
            //popup.FixWindowSize(width, height);
            popup.minSize = new Vector2(width,height);
            popup.header = title;
            popup.updateDetail = updateDetail;
        }

        private void OnGUI() {
            // HEADER
            EditorGUI.LabelField(new Rect(marginX,marginY,position.width-marginX*2,70), header,HIVEEditorUtility.HIVELabelStyle.kHIVEHeaderStyle);

            // CONTENTS
            var offset =2;//컨텐츠 박스영역 보정
            var contentsHeight = position.height - 100;
            GUILayout.BeginArea(new Rect(marginX-offset,70,position.width-marginX*2,contentsHeight));
            scrollpos = EditorGUILayout.BeginScrollView(scrollpos,GUILayout.Height(contentsHeight));
            var rect =  GUILayoutUtility.GetRect(new GUIContent(updateDetail),EditorStyles.textArea);
            rect.height = rect.height < contentsHeight ? contentsHeight : rect.height; //최소값 400
            GUI.Label(rect, new GUIContent(updateDetail),EditorStyles.textArea);
            EditorGUILayout.EndScrollView();
            GUILayout.EndArea();
        }
    }   
}