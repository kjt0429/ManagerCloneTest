using UnityEditor;
using UnityEngine;
using System.Collections;

namespace hive.manager.editor
{
    public static class HIVEEditorUtility {

#if UNITY_EDITOR_OSX
        static string FONTNAME = "Apple SD Gothic Neo";
#else
        static string FONTNAME = "Malgun";
#endif

        public static Font HIVEDefaultFont {
            get {
                return Font.CreateDynamicFontFromOSFont(FONTNAME,10);
            }
        }

        /// <summary>
        /// 자주 사용되는 Label스타일을 정의함.
        /// 라벨의 TextAnchor는 기본 MiddleCenter로 정렬됨.
        /// </summary>
        public static class HIVELabelStyle {
            
            public static GUIStyle kHIVEHeaderStyle {
                get {
                    GUIStyle style = new GUIStyle(GUI.skin.label);
                    style.font = HIVEDefaultFont;
                    style.fontSize = 15;
                    // style.alignment = TextAnchor.MiddleCenter;
                    return style;
                }
            }

            public static GUIStyle kHIVEStandardContentsStyle {
                get {
                    GUIStyle style = new GUIStyle(GUI.skin.label);
                    style.font = HIVEDefaultFont;
                    style.fontSize = 12;
                    //style.alignment = TextAnchor.MiddleCenter;
                    return style;
                }
            }

            // 표준사이즈보다 한단계 큰 스타일의 라벨폰트
            public static GUIStyle kHIVEMiddleSizeContentsStyle {
                get {
                    GUIStyle style = new GUIStyle(GUI.skin.label);
                    style.font = HIVEDefaultFont;
                    style.fontSize = 13;
                    //style.alignment = TextAnchor.MiddleCenter;
                    return style;
                }
            }

            public static GUIStyle kHIVEMiddleSizeContentsStyleCenter {
                get {
                    GUIStyle style = kHIVEMiddleSizeContentsStyle;
                    style.alignment = TextAnchor.MiddleCenter;
                    return style;
                }
            }

            public static GUIStyle kHIVEStandardContentsLinkStyle {
                get {
                    var style = HIVELabelStyle.kHIVEStandardContentsStyle;
                    style.normal.textColor = new Color(0x00 / 255f, 0x78 / 255f, 0xDA / 255f, 1f);
                    return style;
                }
            }
            
        }

        public static void DrawBox(Rect rect) {
            HIVEEditorUtility.DrawBox(rect, GUI.contentColor);
        }

        public static void DrawBox(Rect rect, Color color) {
            Vector2 topleft  = new Vector2(rect.x,            rect.y);
            Vector2 topright = new Vector2(rect.x+rect.width, rect.y);
            Vector2 botleft  = new Vector2(rect.x,            rect.y+rect.height);
            Vector2 botright = new Vector2(rect.x+rect.width, rect.y+rect.height);

            var top = new Vector3[]{topleft, topright};
            var left = new Vector3[]{topright, botright};
            var right = new Vector3[]{botleft, botright};
            var bottom = new Vector3[]{topleft, botleft};

            var oldColor = Handles.color;
            Handles.color = color;
            Handles.DrawAAPolyLine(2, top);
            Handles.DrawAAPolyLine(2, left);
            Handles.DrawAAPolyLine(2, right);
            Handles.DrawAAPolyLine(2, bottom);
            Handles.color = oldColor;
        }
        
        public static void EditorGUILabel () {
        }
    }
}