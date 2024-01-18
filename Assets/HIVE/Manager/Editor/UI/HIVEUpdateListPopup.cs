
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace hive.manager.editor {
    
    /// <summary>
    /// 업데이트 목록을 출력해주는 UI.
    /// </summary>
    public class HIVEUpdateListPopup : HIVEEditorWindow<HIVEUpdateListPopup> {
        MultiColumnHeader header;
        MultiColumnHeaderState state;
    
        MultiColumnHeaderState.Column[] columns;
        HIVESDKUpdateListView updateListView;

        static float width = 660;
        static float height = 570;

        static float marginX = 30;
        static float marginY = 30;

        static HIVEUpdateListPopup instance;

        enum Column {
            releaseDate,
            versionOfSdk,
            description,
            applyButton
        }

        System.Action<HIVEUpdateListPopup, string> applySDK;
        System.Action<HIVEUpdateListPopup, string> showDetailFeature;

        List<HIVESDKUpdateListViewData> items;
        public static void ShowPopup(List<HIVESDKUpdateListViewData> items,
                                    System.Action<HIVEUpdateListPopup, string> applySDK,
                                    System.Action<HIVEUpdateListPopup, string> showDetailFeature) {

            if (instance == null) {
                instance = CreateInstance<HIVEUpdateListPopup>();
            }
            instance.titleContent = new GUIContent("UpdateListView");
            instance.items = items;
            instance.FixWindowSize(width,height);
            instance.BuildMultiColumnView();
            instance.applySDK = applySDK;
            instance.showDetailFeature = showDetailFeature;
            instance.ShowUtility();
        }

        void OnGUI() {
            // HEADER
            EditorGUI.LabelField(new Rect(marginX,marginY,width-marginX*2,70),getEditorStrings().updateListHeader,HIVEEditorUtility.HIVELabelStyle.kHIVEHeaderStyle);

            //NOTICE
            var noticeStyle = HIVEEditorUtility.HIVELabelStyle.kHIVEStandardContentsStyle;
            noticeStyle.alignment = TextAnchor.MiddleLeft;
            EditorGUI.LabelField(new Rect(marginX,80,width-marginX*2,60),getEditorStrings().updateListNotice,noticeStyle);

            updateListView.OnGUI(new Rect(marginX,160,width-marginX*2,380));
            if (!(items != null && items.Count > 0)) {
                //OR ALREADY LATEST VERSION
                EditorGUI.LabelField(centerHorizontalRect(0,250,width,60),getEditorStrings().updateListAlreadyUseLatestVersion,HIVEEditorUtility.HIVELabelStyle.kHIVEMiddleSizeContentsStyleCenter);
            }
        }
    
        void BuildMultiColumnView() {
            columns = new MultiColumnHeaderState.Column[] {
                defaultColumn(Column.releaseDate),
                defaultColumn(Column.versionOfSdk),
                defaultColumn(Column.description),
                defaultColumn(Column.applyButton)
            };
            state = new MultiColumnHeaderState(columns);
            header = new MultiColumnHeader(state);
            header.ResizeToFit();

            updateListView = new HIVESDKUpdateListView(items, header, (HIVESDKUpdateListView.Column column, int index)=> {
                if(column == HIVESDKUpdateListView.Column.Feature) {
                    
                    showDetailFeature(this, items[index].id);
                } else if (column == HIVESDKUpdateListView.Column.Apply) {
                    applySDK(this, items[index].id);
                }
            });
        }

        MultiColumnHeaderState.Column defaultColumn(Column column) {
            var columnObject = new MultiColumnHeaderState.Column();
            columnObject.headerTextAlignment = TextAlignment.Center;
            columnObject.autoResize = true;
            columnObject.canSort = false;
            columnObject.minWidth = 100;

            switch(column) {
                case Column.releaseDate:
                columnObject.headerContent = new GUIContent(getEditorStrings().updateListReleaseDate);
                break;
                case Column.versionOfSdk:
                columnObject.headerContent = new GUIContent(getEditorStrings().updateListSdkVersion);
                break;
                case Column.description:
                columnObject.headerContent = new GUIContent(getEditorStrings().updateListFeatures);
                columnObject.width = 140;
                break;
                case Column.applyButton:
                columnObject.headerContent = new GUIContent(getEditorStrings().updateListApplyUpdate);
                break;
            }
            return columnObject;
        }
    }
}