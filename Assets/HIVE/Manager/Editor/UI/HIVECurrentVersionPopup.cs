using UnityEngine;
using UnityEditor;


namespace hive.manager.editor
{
    /// <summary>
    /// SDK의 버전표시를 위한 Layout.
    /// 현버전, 최신버전, 최신버전이 출시된 날짜를 표시하는 UI이다.
    /// </summary>
    public class HIVECurrentVersionPopup : HIVEEditorWindow<HIVECurrentVersionPopup> {

        const float width = 350;
        const float height = 350;

        const float marginX = 30;
        const float marginY = 30;

        string currentVersion = "0.0.0";
        string latestVersion = "0.0.0";
        string managerVersion = "0.0.0";
        
        public static void ShowPopup(string currentVersion, string latestVersion, string managerVersion) {
            var popup = ShowSingletonPopup();
            popup.titleContent = new GUIContent("Version");
            popup.FixWindowSize(width,height);
            popup.currentVersion = currentVersion;
            popup.latestVersion = latestVersion;
            popup.managerVersion = managerVersion;
        }

        private void OnGUI() {

            GUIStyle titleStyle = HIVEEditorUtility.HIVELabelStyle.kHIVEHeaderStyle;
            GUIStyle contentStyle = HIVEEditorUtility.HIVELabelStyle.kHIVEStandardContentsStyle;
            contentStyle.wordWrap = true;

            var contentsWidth = width-marginX*2;

            EditorGUI.LabelField(new Rect(marginX,marginY,contentsWidth,30), getEditorStrings().versionNoticeTitle,titleStyle);

            var current = string.Format(getEditorStrings().versionNoticeCurrentVersion,currentVersion);
            var latest = string.Format(getEditorStrings().versionNoticeLatestVersion,latestVersion);
            var manager = string.Format(getEditorStrings().versionNoticeManagerVersion,managerVersion);

            var versionContents = string.Format("{0}\n{1}\n{2}",current,latest,manager);

            EditorGUI.LabelField(new Rect(marginX,marginY+40,contentsWidth,70),versionContents,contentStyle);

            if (latestVersion == currentVersion) {
                EditorGUI.LabelField(new Rect(marginX,marginY+110,contentsWidth,100),getEditorStrings().currentVersionAlreadyLatest,contentStyle);
            } else {
                EditorGUI.LabelField(new Rect(marginX,marginY+110,contentsWidth,100),getEditorStrings().currentVersionNewVersionNotice,contentStyle);
            }

            if (GUI.Button(centerHorizontalRect(0,270,DefaultPopupButtonSize.x,DefaultPopupButtonSize.y), getEditorStrings().popupOK)) {
                Close();
            }
        }
    }
}
