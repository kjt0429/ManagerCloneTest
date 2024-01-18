using UnityEditor;
using UnityEngine;

namespace hive.manager.editor {

    /// <summary>
    /// 하루안보기 버튼이 들어간 Popup
    /// 하루안보기 구현은 이벤트를 받아서 처리하는 쪽에서 직접 구현해주어야한다.
    /// </summary>
    public class HIVENoticeDetail : HIVEEditorWindow<HIVENoticeDetail> {
        const float width = 400;
        const float height = 380;
        const float marginX = 30;
        const float marginY = 30;

        const float buttonAreaHeight = 60;

        string header;
        string contents;
        string popupIdentifier;
        bool dontShowAgain;
        bool isShowDontShowButton;

        Vector2 scrollbar = Vector2.zero;

        public delegate void SaveDontShowAgainDelegate(HIVENoticeDetail alert,bool isDontShowAgain, string popupIdentifier);
        [SerializeField]
        public event SaveDontShowAgainDelegate  saveDontShow;

        public static void ShowPopup(string header, string contents) {
            var popup = CreateInstance<HIVENoticeDetail>();
            popup.titleContent = new GUIContent(header);
            //popup.FixWindowSize(width, height);
            popup.position = new Rect(popup.position.x,popup.position.y, width, height);
            popup.minSize = new Vector2(width,height);
            popup.popupIdentifier = "";
            popup.header = header;
            popup.contents = contents;
            popup.isShowDontShowButton = false;
            popup.ShowUtility();
        }

        public static void ShowPopup(string header, string contents, string popupIdentifier, SaveDontShowAgainDelegate callback) {
            var popup = CreateInstance<HIVENoticeDetail>();
            popup.titleContent = new GUIContent(header);
            //popup.FixWindowSize(width, height);
            popup.position = new Rect(popup.position.x,popup.position.y, width, height);
            popup.minSize = new Vector2(width,height);
            popup.popupIdentifier = popupIdentifier;
            popup.header = header;
            popup.contents = contents;
            popup.saveDontShow += callback;
            popup.isShowDontShowButton = true;
            popup.ShowUtility();
        }

        private void OnGUI() {
            GUIStyle headerStyle = HIVEEditorUtility.HIVELabelStyle.kHIVEHeaderStyle;
            
            GUIStyle labelMiddle = HIVEEditorUtility.HIVELabelStyle.kHIVEStandardContentsStyle;
            labelMiddle.wordWrap = true;

            var windowW = position.width;
            var windowH = position.height;
            var layoutContentsWidth = windowW-marginX*2;

            var buttonAreaMargin = 80;
            var layoutContentsHeigh = windowH-marginY-buttonAreaHeight-buttonAreaMargin;
            if (isShowDontShowButton == false) {
                layoutContentsHeigh = windowH-marginY*2-40;
            }

            EditorGUI.LabelField(new Rect(marginX,marginY,layoutContentsWidth,marginY),header,headerStyle);

            //var scrollHeight = 200;
            GUILayout.BeginArea(new Rect(marginX,60,layoutContentsWidth,layoutContentsHeigh));
            scrollbar = EditorGUILayout.BeginScrollView(scrollbar,GUILayout.Height(layoutContentsHeigh));
            var rect =  GUILayoutUtility.GetRect(new GUIContent(contents),labelMiddle);
            rect.height = rect.height < 100 ? 100 : rect.height; //최소값 400
            GUI.Label(new Rect(0,rect.y,rect.width,rect.height), new GUIContent(contents),labelMiddle);
            EditorGUILayout.EndScrollView();
            GUILayout.EndArea();


            // BOTTOM BUTTON

            if(isShowDontShowButton) {
                DrawDontShow();
            }
        }

        private void DrawDontShow() {
            var windowW = position.width;
            var windowH = position.height;
            var layoutContentsWidth = windowW-marginX*2;

            var buttonAreaScopeRect = new Rect(marginX,
                                                windowH-marginY-buttonAreaHeight,
                                                layoutContentsWidth,
                                                buttonAreaHeight);
            using(var a = new GUILayout.AreaScope(buttonAreaScopeRect)) {
                using(var v = new GUILayout.VerticalScope()){
                    using(var h = new GUILayout.HorizontalScope()){
                        GUILayout.FlexibleSpace(); 
                        if (GUILayout.Button(getEditorStrings().popupOK,GUILayout.Width(DefaultPopupButtonSize.x), GUILayout.Height(DefaultPopupButtonSize.y))) {
                            if (saveDontShow != null) {
                                saveDontShow(this, dontShowAgain, popupIdentifier);
                            }
                            
                            if (dontShowAgain) {
                                // Unity 리어샘블시 saveDontShow이벤트 정상호출 불가.
                                // TODO: 임시처리.. 추후 구현 추상화 요망.
                                // 원래라면 HiVEManager와의 종속성은 발생하면 안됨.
                                HIVEManager.setDontShowNotice(popupIdentifier);
                            }

                            Close(); 
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.FlexibleSpace();
                    using(var h = new GUILayout.HorizontalScope()){
                        GUILayout.FlexibleSpace();
                        dontShowAgain = GUILayout.Toggle(dontShowAgain, getEditorStrings().dontShowAgain);
                        GUILayout.FlexibleSpace();
                    }
                }
            }
        }
    }
}