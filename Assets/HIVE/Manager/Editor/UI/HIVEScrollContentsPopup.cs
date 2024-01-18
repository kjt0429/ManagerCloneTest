using UnityEditor;
using UnityEngine;
using System.Collections.Generic;


namespace hive.manager.editor {

    /// <summary>
    /// 가운데 스크롤로 ListView를 보여주는 팝업이다.
    /// 업데이트 실패 혹은 파일 유효성 검사 실패시 출력되는 UI에서 사용된다.
    /// </summary>
    public class HIVEScrollContentsPopup : HIVEEditorWindow<HIVEScrollContentsPopup> {
        const float width = 500;
        const float height = 500;

        const float marginX = 30;
        const float marginY = 30;

        string header;
        string contents;

        List<string> items;

        int selectedIndex = -1;

        Vector2 scrollPos = new Vector2(0,0);

        public delegate void doubleClickItem(HIVEScrollContentsPopup popup ,int itemIndex);
        public event doubleClickItem doubleClick;

        public static void ShowPopup(string header, string contents,List<string> items, doubleClickItem doubleClickCallback) {
            var popup = CreateInstance<HIVEScrollContentsPopup>();
            popup.titleContent = new GUIContent(header);
            popup.FixWindowSize(width, height);
            popup.header = header;
            popup.contents = contents;
            popup.items = items;
            popup.doubleClick = doubleClickCallback;
            popup.ShowUtility();
        }

        private void OnGUI() {
            
            GUIStyle headerStyle = HIVEEditorUtility.HIVELabelStyle.kHIVEHeaderStyle;
            headerStyle.wordWrap = true;
            headerStyle.clipping = TextClipping.Clip;
            
            GUIStyle labelMiddle = HIVEEditorUtility.HIVELabelStyle.kHIVEStandardContentsStyle;
            labelMiddle.wordWrap = true;
            labelMiddle.clipping = TextClipping.Overflow;

            EditorGUI.LabelField(new Rect(marginX,marginY,width,40),header, headerStyle);
            EditorGUI.LabelField(new Rect(marginX,65,width,80),contents, labelMiddle);

            Color color_default = GUI.backgroundColor;
            Color color_selected = Color.gray;

            GUIStyle itemStyle = new GUIStyle(GUI.skin.button);
            itemStyle.alignment = TextAnchor.MiddleLeft;
            itemStyle.active.background = itemStyle.normal.background;
            itemStyle.margin = new RectOffset(0,0,0,0);
            

            var scrollViewRect = new Rect(marginX,150,width-marginX*2,230);
            HIVEEditorUtility.DrawBox(scrollViewRect);
            GUILayout.BeginArea(scrollViewRect);
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos,GUILayout.Width(scrollViewRect.width),GUILayout.Height(scrollViewRect.height));
            for (int i = 0; i < items.Count; i++) {
                GUI.backgroundColor = (selectedIndex== i) ? color_selected : Color.clear;            
                //show a button using the new GUIStyle
                if (GUILayout.Button(items[i], itemStyle)) {
                    if (selectedIndex == i) {
                        // double click
                        if (doubleClick != null) {
                            doubleClick(this, selectedIndex);
                        }
                    }
                    selectedIndex = i;
                    //do something else (e.g ping an object)
                }
                GUI.backgroundColor = color_default; //this is to avoid affecting other GUIs outside of the list
            }
            EditorGUILayout.EndScrollView();
            GUILayout.EndArea();
            

            // BOTTOM BUTTON
            if (GUI.Button(centerHorizontalRect(0,400,DefaultPopupButtonSize.x,DefaultPopupButtonSize.y), getEditorStrings().popupOK)) {
                Close();
            }
        }
    }
}