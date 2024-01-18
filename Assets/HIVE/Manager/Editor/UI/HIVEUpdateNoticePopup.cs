using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace hive.manager.editor
{

    /// <summary>
    /// 공지사항 데이터용 클래스.
    /// </summary>
    [System.Serializable]
    public struct HIVENoticeContents {
        public string id;
        public string title;
    // public string link;
        public System.DateTime date;
        public HIVENoticeContents(string id,string title, System.DateTime date) {
            this.id = id;
            this.title = title;
        // this.link = link;
            this.date = date;
        }
    }

    /// <summary>
    /// 공지사항을 받아서 출력해준다.
    /// 한번에 최대 6개의 공지사항이 출력가능하다.
    /// </summary>
    public class HIVEUpdateNoticePopup : HIVEEditorWindow<HIVEUpdateNoticePopup> {

        [SerializeField]
        List<HIVENoticeContents> contents;   

        const int maxContentsSize = 6;

        float marginX = 30;
        float marginY = 30;

        System.Action<HIVEUpdateNoticePopup, string> contentsClickCallback;

        public static void ShowPopup(List<HIVENoticeContents> contents, System.Action<HIVEUpdateNoticePopup, string> contentsClickCallback) {
            var popup = ShowSingletonPopup();
            popup.titleContent = new GUIContent("HIVEUpdateNoticePopup");
            popup.FixWindowSize(470,530);
            popup.contentsClickCallback = contentsClickCallback;
            popup.contents = contents;
        }

        protected Rect getCellRect(int row, int col, int rowCount, int colCount, Rect rect) {
            float cellWidth = rect.width/rowCount;
            float celHeight = rect.height/colCount;

            return new Rect(
                rect.x + row * cellWidth, 
                rect.y + col * celHeight, 
                cellWidth, 
                celHeight
            );
        }

        protected void DrawTable(int row, int col, Rect rect) {
            HIVEEditorUtility.DrawBox(rect);

            for (int x=1; x<row; x++) {
                var line = getCellRect(x, 0, row, col, rect);
                Handles.DrawAAPolyLine(2, new Vector3[] {
                        new Vector2(line.x,rect.y),
                        new Vector2(line.x,rect.y + rect.height)});
            }

            for (int y=1; y<col; y++) {
                var line = getCellRect(0, y, row, col, rect);
                Handles.DrawAAPolyLine(2, new Vector3[] {
                        new Vector2(rect.x,line.y),
                        new Vector2(rect.x + rect.width,line.y)});
            }
        }

        private void OnGUI() {
            //Draw Header
            EditorGUI.LabelField(new Rect(marginX,marginY,position.width,100),getEditorStrings().noticeHeader, HIVEEditorUtility.HIVELabelStyle.kHIVEHeaderStyle);

            if (contents == null || contents.Count <= 0) {
                return;
            }

            //테이블 그리는 방식 수정하기.
            var width = 400;
            var column1width =width * 0.7f;
            var column2width =width * 0.3f;
            var contentsColumnSize = centerHorizontalRect(-(width)/2+column1width/2,70,column1width,430);
            var dateTimeColumnSize = centerHorizontalRect((width)/2-column2width/2,70,column2width,430);
            //Draw Table
            DrawTable(1,6, contentsColumnSize); // 1 - row
            DrawTable(1,6, dateTimeColumnSize);// 2 - row

            //Draw Table Contents
            GUIStyle linkStyle = HIVEEditorUtility.HIVELabelStyle.kHIVEStandardContentsLinkStyle;
            linkStyle.alignment = TextAnchor.MiddleCenter;
            linkStyle.stretchWidth = false;
            linkStyle.wordWrap = true;

            //date style 
            var dateStyle = HIVEEditorUtility.HIVELabelStyle.kHIVEStandardContentsStyle;
            dateStyle.alignment = TextAnchor.MiddleCenter;

            for (int i=0; i<maxContentsSize; i++) {
                if (contents == null || (contents != null && contents.Count <= i)) {
                    break;
                }
                var title = getCellRect(0,i,1,6,contentsColumnSize);
                var date = getCellRect(0,i,1,6,dateTimeColumnSize);

                EditorGUIUtility.AddCursorRect(title, MouseCursor.Link);

                if (GUI.Button(title, contents[i].title, linkStyle)) {
                    contentsClickCallback(this, contents[i].id);
                }
                EditorGUI.LabelField(date,contents[i].date.ToString("MM/dd/yyyy\n HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture), dateStyle);
            }
        }    }   
}