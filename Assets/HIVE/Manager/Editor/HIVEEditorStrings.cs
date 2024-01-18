using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace hive.manager.editor {

	[System.Serializable]
	public class HIVEManagerLangauge : ISerializationCallbackReceiver {
		[SerializeField]
		string ko = "";
		[SerializeField]
		string en = "";

		//ko와 en중에 선택된 랭귀지.
		string text;
		public void OnAfterDeserialize() {
			text = ko.IfOSLanguageCodeMatchStringOrAltString("ko",en);
		}
        
        public void OnBeforeSerialize() { 
		}

		public static implicit operator string(HIVEManagerLangauge langauge) {
            if(langauge != null) return langauge.text;
            return null;
        }
	}

	[System.Serializable]
	public class HIVEEditorStrings {

		static private HIVEEditorStrings _instance;
		static public HIVEEditorStrings Instance {
			get {
				if (_instance == null) {
					// String Asset Path
					var stringJsonPath = Path.Combine(Application.dataPath,
										Path.Combine("HIVE",
										Path.Combine("Manager",
										Path.Combine("Editor",
										Path.Combine("Resources", "HIVEManagerStrings.json")))));
					StreamReader inp_stm = new StreamReader(stringJsonPath);
					while(!inp_stm.EndOfStream)
					{
						string inp_ln = inp_stm.ReadLine( );
						inp_ln = inp_ln.Replace(@"\\",@"\"); //json encoding replace
						_instance = JsonUtility.FromJson<HIVEEditorStrings>(inp_ln);
					}
					inp_stm.Close();
				}
				return _instance;
			}
		}

		static HIVEEditorStrings() {
			//load text
			var instance = Instance;
		}

		//Update List Strings
		public HIVEManagerLangauge updateListHeader;//"안녕하세요, HIVE SDK 매니저입니다.\n신규 버전 업데이트를 빠르고 손 쉽게 진행하세요.";
		public HIVEManagerLangauge updateListNotice;//"1. 아래목록에서 업데이트할 버전을 선택하세요. \n2. HIVE SDK 매니저가 변경 및 신규파일만 추출하여 자동 업데이트를 진행합니다.";
		public HIVEManagerLangauge updateListReleaseDate;//"배포일";
		public HIVEManagerLangauge updateListSdkVersion;//"SDK 버전";
		public HIVEManagerLangauge updateListFeatures;//"업데이트 사항";
		public HIVEManagerLangauge updateListApplyUpdate;//"적용하기";
		public HIVEManagerLangauge updateListApplySelectButton;//"선택";
		public HIVEManagerLangauge updateListAlreadyUseLatestVersion;//"현재 최신 버전을 사용 중 입니다.";
		public HIVEManagerLangauge updateListWouldYouUpdateNewVersionTitle; //업그레이드
		public HIVEManagerLangauge updateListWouldYouUpdateNewVersion;//"HIVE SDK {0}으로 업그레이드 하시겠습니까?";
		public HIVEManagerLangauge updateListNewVersionUpdateHeader;//"HIVE SDK {0} 업데이트 안내";

		// Current Version Popup
		public HIVEManagerLangauge currentVersionCurrent;//"현재 버전";
		public HIVEManagerLangauge currentVersionHIVESDK;//"HIVE SDK {0}";
		public HIVEManagerLangauge currentVersionNewVersionNotice;//"최신 버전이 아닙니다.\nHIVE SDK 매니저에서 신규기능을 확인하고\n버전을 업그레이드 하세요.";
		public HIVEManagerLangauge currentVersionAlreadyLatest;//"최신 버전을 사용 중 입니다.";
		public HIVEManagerLangauge currentVersionLatest;//"최신 버전";
		public HIVEManagerLangauge currentVersionUpdateDate;//"Updated : {0}";

		//UPDATE DONE POPUP
		public HIVEManagerLangauge sdkUpdateDoneSuccessTitle;//"업그레이드 완료!";
		public HIVEManagerLangauge sdkUpdateDoneSuccessContents;//"HIVE SDK 신규 기능별 상세 가이드는\nHIVE 개발자 사이트를 참조해주세요.\nhttps://developers.withhive.com";
		public HIVEManagerLangauge sdkUpdateFailureDataNotValidTitle;//"업그레이드 불가!";
		public HIVEManagerLangauge sdkUpdateFailureDataNotValidContents;//"원본 데이터가 변형되어 업그레이드를 할 수 없습니다.\n 아래 파일을 HIVE에서 제공했던 원래의 상태로 변경한 후\n업그레이드를 진행하세요.";
		public HIVEManagerLangauge sdkUpdateFailureTitle;//업그래이드 실패!
		public HIVEManagerLangauge sdkUpdateFailureContents;//파일 다운로드가 실패하여 업그래이드가 되지 않았습니다. 다시 시도해주세요.\n{0}
		
		//VALIDATION CHECK
		public HIVEManagerLangauge sdkValidityCheckTitle;//"무결성 검증을 시작할까요?";
		public HIVEManagerLangauge sdkValidityCheckContents;//"HIVE SDK 원본 데이터가 변형되면 버그가 발생할 수 있습니다.\n적용된 버전의 무결성 검증을 하고 사전에 버그를 예방하세요.";

		//VALIDATION CHECK DONE
		public HIVEManagerLangauge sdkValidityCheckDoneTitle;//"무결성 검증 완료!";
		public HIVEManagerLangauge sdkVadlidityCheckDoneContentsSuccess;//"무결성 검증 결과 변형된 파일이 없습니다.";
		public HIVEManagerLangauge sdkVadlidityCheckDoneContentsFailure;//"무결성 검증 결과 아래 파일에서 변형된 부분이\n발견되었습니다. HIVE가 제공한 원래의 상태로 변경해주세요.";

		// NOTCIE
		public HIVEManagerLangauge noticeHeader;//"공지 알림";

		//ONE DAY SKIP
		public HIVEManagerLangauge dontShowAgain;//"더 이상 보지 않기";

		//common popup
		public HIVEManagerLangauge popupOK;//"확인";
		public HIVEManagerLangauge popupCancel;//"취소";
		public HIVEManagerLangauge popupStart;//"시작";
		public HIVEManagerLangauge popupGoDeveloper;//"개발자 사이트 가기";
		public HIVEManagerLangauge reconnectDoneTitle;//연결 재시도 완료!
		public HIVEManagerLangauge reconnectDoneContents;//HIVE 매니저와 연결이 재시도 되었습니다. \n지속적인 연결실패 시 플랫폼클라이언트팀에 문의 주세요.
		public HIVEManagerLangauge versionNoticeTitle;//버전 안내
		public HIVEManagerLangauge versionNoticeCurrentVersion;//* 적용된 SDK version : HIVE SDK v{0}
		public HIVEManagerLangauge versionNoticeLatestVersion;//* 최신 SDK version : HIVE SDK v{0}
		public HIVEManagerLangauge versionNoticeManagerVersion;//* HIVE SDK Manager version : v{0}
		public HIVEManagerLangauge versionRepairTitle;//HIVE SDK 복구
		public HIVEManagerLangauge versionRepairContents;//HIVE SDK를 원본상태로 복구합니다. \n의도적으로 수정한 파일이 있다면 미리 백업해주세요.\n복구를 시작하시겠습니까?
		public HIVEManagerLangauge versionRepairSuccessTitle;//HIVE SDK 복구 성공!
		public HIVEManagerLangauge versionRepairSuccessContents;//HIVE SDK를 원본상태로 되돌렸습니다.
		public HIVEManagerLangauge versionRepairFailureTitle;//HIVE SDK 복구 실패!
		public HIVEManagerLangauge versionRepairFailureContents;//HIVE SDK를 원본상태로 복구하는데 실패했습니다. {0}
		public HIVEManagerLangauge versionRepairNotNeedTitle;//HIVE SDK 복구
		public HIVEManagerLangauge versionRepairNotNeedContents;//HIVE SDK를 복구할 필요가 없습니다.
	}
}