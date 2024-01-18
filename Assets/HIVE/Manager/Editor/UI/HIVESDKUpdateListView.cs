
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using System.Collections.Generic;


namespace hive.manager.editor
{

    public struct HIVESDKUpdateListViewData {
        public string id;
        public System.DateTime releaseDate;
        public string version;
        public string features;        
    }

    public class HIVESDKUpdateListView {

        MultiColumnHeader header;
        float RowHeight = 20;
        Vector2 scrollBarPosition = Vector2.zero;
        List<HIVESDKUpdateListViewData> items;

        //실제 콘텐츠크기를 스크롤바 크기만큼 제외해준다.
        float scrollbarOffset {
            get {
                return GUI.skin.verticalScrollbar.fixedWidth;
            }
        }

        public enum Column {
            DeployDate,
            Version,
            Feature,
            Apply
        }

        System.Action<Column,int> columnClickEvent;

        public HIVESDKUpdateListView(List<HIVESDKUpdateListViewData> items, MultiColumnHeader header, System.Action<Column,int> columnClickEvent) {
            this.header = header;
            this.RowHeight = header.height +6;
            this.items = items;
            this.columnClickEvent = columnClickEvent;
        }

        public void OnGUI(Rect rect) {
            
            Rect headerRect = new Rect(rect.x,rect.y,rect.width, header.height);
            var defaultFont = MultiColumnHeader.DefaultStyles.columnHeader.font; //backup default font
            MultiColumnHeader.DefaultStyles.columnHeader.font = HIVEEditorUtility.HIVEDefaultFont; //set custom font;
            header.OnGUI(headerRect, 0f);
            MultiColumnHeader.DefaultStyles.columnHeader.font = defaultFont; //repair default font

            Rect scrollRect = new Rect(rect.x,rect.y+headerRect.height,rect.width,rect.height-headerRect.height);

            scrollBarPosition = GUI.BeginScrollView(scrollRect,scrollBarPosition,GetContentsRect(rect),false,true);
            for (int i=0;i<items.Count;i++) {
                DrawRow(rect,i);
            }
            GUI.EndScrollView();


            HIVEEditorUtility.DrawBox(rect, Color.black);
        }

        private Rect GetContentsRect(Rect rect) {
            //var cellRect4 = header.GetCellRect(3,new Rect(0,0,rect.width, header.height));
            return new Rect(0,0,rect.width-scrollbarOffset, RowHeight * items.Count);
        }


        private void DrawRow(Rect rect, int index) {
            // index * RowHeight 열 위치 지정.
            var cellRect1 = header.GetCellRect(0,new Rect(0,index*RowHeight,rect.width, RowHeight));
            var cellRect2 = header.GetCellRect(1,new Rect(0,index*RowHeight,rect.width, RowHeight));
            var cellRect3 = header.GetCellRect(2,new Rect(0,index*RowHeight,rect.width, RowHeight));
            var cellRect4 = header.GetCellRect(3,new Rect(0,index*RowHeight,rect.width, RowHeight));

            if (index%2 == 0) {
                EditorGUI.DrawRect(new Rect(0,index*RowHeight,rect.width,RowHeight), Handles.secondaryColor); //짝수열에 배경색 넣어줌
            }

            var standardStyle = HIVEEditorUtility.HIVELabelStyle.kHIVEStandardContentsStyle;
            standardStyle.alignment = TextAnchor.MiddleCenter;
            var linkLabelStyle = HIVEEditorUtility.HIVELabelStyle.kHIVEStandardContentsLinkStyle;
            linkLabelStyle.alignment = TextAnchor.MiddleCenter;

            var buttonMargin = new Vector2(10,10);
            EditorGUI.LabelField(cellRect1,
                                 items[index].releaseDate.ToString("d", System.Globalization.CultureInfo.InvariantCulture), 
                                 standardStyle);
            EditorGUI.LabelField(cellRect2,items[index].version, standardStyle);
            if (GUI.Button(cellRect3,items[index].features, linkLabelStyle)) {
                columnClickEvent(Column.Feature, index);
            }
            if (GUI.Button(new Rect(cellRect4.x + buttonMargin.x/2 , 
                                    cellRect4.y + buttonMargin.y/2, 
                                    cellRect4.width - buttonMargin.x, 
                                    cellRect4.height - buttonMargin.y), 
                                    HIVEEditorStrings.Instance.updateListApplySelectButton)) {
                columnClickEvent(Column.Apply,index);

            }
        }
    }

    internal class HIVESDKUpdateListHeader : MultiColumnHeader {
		public HIVESDKUpdateListHeader(MultiColumnHeaderState state)
			: base(state)
		{

            canSort = true;
			height = DefaultGUI.defaultHeight;

		}
    }
}