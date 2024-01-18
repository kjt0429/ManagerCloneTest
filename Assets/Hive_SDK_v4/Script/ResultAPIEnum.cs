/**
* @file    ResultAPIEnum.cs
*
*  @date        2018-2022
*  @copyright    Copyright © Com2uS Platform Corporation. All Right Reserved.
*  @author       Daun Joung
*  @since        4.6.0
*/


using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;

/**
* @defgroup ResultAPI
* @{
* \~korean  API 호출에 대한 결과를 담는 클래스<br/><br/>
* \~english Class containing results for API calls<br/><br/>
*/

namespace hive
{
    public partial class ResultAPI {

		public enum Code : int {

            // RESULT API BEGIN
            Success = 0, ///< Success.
			RealNameVerification = 2100, ///< [Common] Real Name Verification: %s
			RefundUser = 2300, ///< [Common] Refund User: %s
			CommonHTTPConnectionException = -1, ///< [Common] HTTP Connection exception.
			CommonHTTPConnectionOpenException = -2, ///< [Common] HTTP Connection open exception.
			CommonHTTPContentEncodingNotSupported = -3, ///< [Common] HTTP Content encoding not supported.
			CommonHTTPDecryptionFailed = -4, ///< [Common] HTTP Decryption failed.
			CommonHTTPResponseException = -5, ///< [Common] HTTP Response exception.
			CommonHTTPInvalidBody = -6, ///< [Common] HTTP body is nil.
			CommonHTTPInvalidJSON = -7, ///< [Common] HTTP body has invalid JSON: %1
			CommonHTTPInvalidURLRequest = -8, ///< [Common] HTTP Failed to create NSMutableURLRequest.
			CommonHTTPInvalidURL = -9, ///< [Common] HTTP Failed to create NSURL from urlPath: %1
			CommonHTTPGzipDecodeFailed = -10, ///< [Common] GZip decoding is failed.
			CommonHTTPNetworkError = -11, ///< [Common] Network error is occurred on handle response: %1
			CommonLibraryMissing = -12, ///< [Common] Library missing: %s
			TestError = -800, ///< [Test] Error message.
			TestWithNSError = -801, ///< [Test] NSError message: %1
			TestWithNSString = -802, ///< [Test] NSString message: %1
			TestWithNSDictionary = -803, ///< [Test] NSDictionary message: %1
			CommonUnknown = -999, ///< [Common] Unknown
			AuthNotInitialized = -1100001, ///< [Auth-Common] Not Initialized
			AuthInvalidServerResponse = -1100002, ///< [Auth-Common] Invalid Server Response
			AuthServerResponseNotSuccessful = -1100003, ///< [Auth-Common] Server Response Not Successful. string: %1
			AuthInvalidUser = -1100004, ///< [Auth-Common] Invalid User
			AuthUserCanceled = -1100005, ///< [Auth-Common] User Canceled
			AuthInProgressLoginLogout = -1100006, ///< [Auth-Common] Login or logout is already in progress
			AuthInvalidSelectedAccountURL = -1100007, ///< [Auth-Common] Invalid Selected AccountURL
			AuthInvalidSelectedVID = -1100008, ///< [Auth-Common] Selected VID is empty or null
			AuthOnRunningV4 = -1100009, ///< [Auth-Common] On Running V4
			AuthInvalidConfigurationXml = -1100010, ///< [Auth-Common] Invalid Configuration Xml
			AuthInvalidParamLoginType = -1100011, ///< [Auth-Common] Invalid Param Login Type: %1
			AuthInvalidParamVID = -1100012, ///< [Auth-Common] Invalid Param Vid: %1
			AuthInvalidParamSessionKey = -1100013, ///< [Auth-Common] Invalid Param Session Key
			AuthInvalidGuestSession = -1100014, ///< [Auth-Common] Invalid Guest Session
			AuthUserInBlacklist = -1100015, ///< [Auth-Common] User In Blacklist: %1
			AuthInvalidAccountSession = -1100016, ///< [Auth-Common] Invalid Account Session
			AuthJsonException = -1100017, ///< [Auth-Common] JSON Parsing failed. error: %1
			AuthCanceled = -1100018, ///< [Auth-Common] Canceled
			AuthDialogAlreadyUsing = -1100019, ///< [Auth-Common] Dialog is already using.
			AuthNetworkErrorShowLoginSelection = -1100020, ///< [Auth-Common] Network error occured on Get My Profile: %1
			AuthNetworkErrorCheckMaintenance = -1100021, ///< [Auth-Common] Network error occured on Check Maintenance
			AuthResponseFailCheckMaintenance = -1100022, ///< [Auth-Common] Bad response on Check Maintenance: %1
			AuthResponseFailMaintenanceDialog = -1100023, ///< [Auth-Common] Failed to show maintenance dialog.
			AuthNetworkErrorProcessLoginType = -1100024, ///< [Auth-Common] Network error occured on Prelogin: %1
			AuthNetworkErrorGuestLogin = -1100025, ///< [Auth-Common] Network error occured on Guest Login: %1
			AuthHIVESocialLoginCancelled = -1100026, ///< [Auth-Common] HIVE social login canceled.
			AuthNetworkErrorLoginCenterLogin = -1100027, ///< [Auth-Common] Network error occured on Login Center Login: %1
			AuthNetworkErrorRequestUpdate = -1100028, ///< [Auth-Common] Network error occured on Request Update: %1
			AuthNetworkErrorRequestAdultConfirm = -1100029, ///< [Auth-Common] Network error occured on Adult Confirm.
			AuthInvalidParamVIDList = -1100030, ///< [Auth-Common] VID list is nil or empty
			AuthAgreementFail_DoExit = -1100031, ///< [Auth-Common] Agreement Fail
			AuthMaintenanceActionDefault_DoExit = -1100032, ///< [Auth-Common] Maintenance Dialog Dismiss with unknown action.
			AuthMaintenanceActionOpenURL_DoExit = -1100033, ///< [Auth-Common] Maintenance Dialog Dismiss with Open URL Button Selected.
			AuthMaintenanceActionExit_DoExit = -1100034, ///< [Auth-Common] Maintenance Dialog Dismiss with Exit Button Selected.
			AuthMaintenanceActionDone = -1100035, ///< [Auth-Common] Maintenance Dialog Dismiss with Done Button Selected.
			AuthMaintenanceTimeover_DoExit = -1100036, ///< [Auth-Common] Maintenance Dialog Dismiss with Timeover.
			AuthUserInBlacklistActionDefault_DoExit = -1100037, ///< [Auth-Common] Blacklist Dialog Dismiss with unknown action.
			AuthUserInBlacklistActionOpenURL_DoExit = -1100038, ///< [Auth-Common] Blacklist Dialog Dismiss with Open URL Button Selected.
			AuthUserInBlacklistActionExit_DoExit = -1100039, ///< [Auth-Common] Blacklist Dialog Dismiss with Exit Button Selected.
			AuthUserInBlacklistActionDone = -1100040, ///< [Auth-Common] Blacklist Dialog Dismiss with Done Button Selected.
			AuthUserInBlacklistTimeover_DoExit = -1100041, ///< [Auth-Common] Blacklist Dialog Dismiss with Timeover.
			AuthInProgressInitialize = -1100042, ///< [Auth-Common] Initialize is already in progress.
			AuthSkipPermissionView = -1100043, ///< [Auth-Common] Can skip the Permission view.
			AuthInProgressRequestPermissionViewData = -1100044, ///< [Auth-Common] Requesting PermissionViewData is already in progress.
			AuthNetworkErrorRequestPermissionViewData = -1100045, ///< [Auth-Common] Network error occjured on requestPermissionViewData
			AuthV4InvalidServerResponse = -1200001, ///< [AuthV4-Common] InvalidServerResponse
			AuthV4OnRunningV1 = -1200002, ///< [AuthV4-Common] On running AuthV1
			AuthV4InProgressSignIn = -1200003, ///< [AuthV4-Common] SigninInProgress
			AuthV4InvalidConfigurationXml = -1200004, ///< [AuthV4-Common] Invalid configuration xml
			AuthV4InvalidSavedPlayerInfo = -1200005, ///< [AuthV4-Common] InvalidSavedPlayerInfo
			AuthV4AlreadyAuthorized = -1200006, ///< [AuthV4-Common] Already authorized: %1
			AuthV4ConflictPlayer = -1200007, ///< [AuthV4-Common] Conflict player
			AuthV4UserInBlacklist = -1200008, ///< [AuthV4-Common] User in blacklist
			AuthV4InvalidSession = -1200009, ///< [AuthV4-Common] Invalid session: %1
			AuthV4ConflictPlayerHandlingFail = -1200010, ///< [AuthV4-Common] ConflictPlayerHandlingFail
			AuthV4InvalidParamDid = -1200011, ///< [AuthV4-Common] Failed to seup. Empty DID.
			AuthV4NotInitialized = -1200012, ///< [AuthV4-Common] Setup HIVE module first.
			AuthV4SessionExist = -1200013, ///< [AuthV4-Common] Already authorized
			AuthV4SessionNotExist = -1200014, ///< [AuthV4-Common] Not exist remain session. Please sign in provider.
			AuthV4InvalidProviderType = -1200015, ///< [AuthV4-Common] Invalid provider type : %1
			AuthV4SigninFirst = -1200016, ///< [AuthV4-Common] Need Sign in first.
			AuthV4ProviderAlreadyConnected = -1200017, ///< [AuthV4-Common] Provider already connected: %1
			AuthV4ProviderAlreadyDisconnected = -1200018, ///< [AuthV4-Common] Provider already disconnected: %1
			AuthV4InvalidParamSelectedPlayerid = -1200019, ///< [AuthV4-Common] InvalidParamSelectedPlayerid
			AuthV4InvalidConflictInfo = -1200020, ///< [AuthV4-Common] Invalid conficlt info : %1
			AuthV4InvalidPlayeridList = -1200021, ///< [AuthV4-Common] Empty playerID List
			AuthV4JsonException = -1200022, ///< [AuthV4-Common] JsonException : %1
			AuthV4InvalidSigninSelection = -1200023, ///< [AuthV4-Common] Invalid signin selection
			AuthV4NotSupportedProviderType = -1200024, ///< [AuthV4-Common] Not supported requested provider type : %1
			AuthV4WebviewDialogError = -1200025, ///< [AuthV4-Common] Webview dialog error: %1
			AuthV4InProgressAuthDialog = -1200026, ///< [AuthV4-Common] Dialog already used.
			AuthV4InvalidParamViewID = -1200027, ///< [AuthV4-Common] Empty View ID.
			AuthV4InvalidParamPlayerID = -1200028, ///< [AuthV4-Common] Invalid player ID: %1
			AuthV4NetworkErrorSigninGuest = -1200029, ///< [AuthV4-Common] Network error occured on Internal Signin Guest. type: %1
			AuthV4ResponseFailProviderUserID = -1200030, ///< [AuthV4-Common] Response error. Provider user ID is empty.
			AuthV4ResponseFailSelectedPlayerID = -1200031, ///< [AuthV4-Common] Response error. Selected player ID is empty or nil
			AuthV4CancelDialog = -1200032, ///< [AuthV4-Common] Dialog canceled by user.
			AuthV4ResponseFailSocialDialog = -1200033, ///< [AuthV4-Common] Response error. Social Dialog.: %1
			AuthV4ProfileNetworkError = -1200034, ///< [AuthV4-Profile] Network error is occurred.
			AuthV4ProfileResponseFail = -1200035, ///< [AuthV4-Profile] Response Fail.
			AuthV4MembershipNetworkError = -1200036, ///< [AuthV4-Membership] Network error is occurred.
			AuthV4MembershipResponseFail = -1200037, ///< [AuthV4-Membership] Response Fail.
			AuthV4ResponseFailSigninProvider = -1200038, ///< [AuthV4-Common] Response Fail on Internal Signin Provider : %1
			AuthV4ResponseFailCheckProvider = -1200039, ///< [AuthV4-Common] Response Fail on CheckProvider : %1
			AuthV4ResponseFailGetFriendList = -1200040, ///< [AuthV4-Common] Response Fail on GetFriendList : %1
			AuthV4ResponseFailSigninGuest = -1200041, ///< [AuthV4-Common] Response Fail on SigninGuest : %1
			AuthV4InProgressConnect = -1200042, ///< [AuthV4-Common] Connection is already in progress
			AuthV4InProgressShowLeaderboard = -1200043, ///< [AuthV4-Common] Showing leaderboard is already in progress
			AuthV4InProgressShowAchievements = -1200044, ///< [AuthV4-Common] Showing achievements is already in progress
			AuthV4PlayerChange = -1200045, ///< [AuthV4-Common] Success, but account is switched to device account.
			AuthV4HelperImplifiedLoginFail = -1200046, ///< [AuthV4Helper-Common] Implified login failed.
			AuthV4PlayerResolved = -1200047, ///< [AuthV4Helper-Common] Player resolved.
			AuthV4AgreementFail_DoExit = -1200048, ///< [AuthV4-Common] Agreement Fail
			AuthV4AgreementFailWithWebviewError_DoExit = -1200049, ///< [AuthV4-Common] Agreement Fail with Webview Error
			AuthV4MaintenanceActionDefault_DoExit = -1200050, ///< [AuthV4-Common] Maintenance Dialog Dismiss with unknown reason
			AuthV4MaintenanceActionOpenURL_DoExit = -1200051, ///< [AuthV4-Common] Maintenance Dialog Dismiss with OpenURL button selected
			AuthV4MaintenanceActionExit_DoExit = -1200052, ///< [AuthV4-Common] Maintenance Dialog Dismiss with Exit button selected
			AuthV4MaintenanceActionDone = -1200053, ///< [AuthV4-Common] Maintenance Dialog Dismiss with Done button selected
			AuthV4MaintenanceTimeover_DoExit = -1200054, ///< [AuthV4-Common] Maintenance Dialog Dismiss with Timeover
			AuthV4NetworkError = -1200055, ///< [AuthV4-Common] Network error occured : %1
			AuthV4InvalidResponseData = -1200056, ///< [AuthV4-Common] Invalid response data : %1
			AuthV4ServerResponseError = -1200057, ///< [AuthV4-Common] Server response error : %1
			AuthV4NeedSignIn = -1200058, ///< [AuthV4-Common] Need sign in.
			AuthV4InProgress = -1200059, ///< [AuthV4-Common] In progress.
			AuthV4InvalidParam = -1200060, ///< [AuthV4-Common] Invalid param : %1
			AuthV4SkipPermissionView = -1200061, ///< [AuthV4-Common] Can skip the Permission view.
			AuthV4InvalidCertification = -1200062, ///< [AuthV4-Common] Invalid HiveCertification.
			AuthV4LastProviderCantDisconnect = -1200063, ///< [AuthV4-Common] AuthV4 Last Provider Cant Disconnect
			AuthV4ServiceShutdown = -1200064, ///< [AuthV4-Common] Service Shutdown : %s
			AuthV4ProviderLoginError = -1200101, ///< [AuthV4-Provider] ProviderLoginError
			AuthV4ProviderLogoutError = -1200102, ///< [AuthV4-Provider] Logout Failed
			AuthV4ProviderNotSupportGetFriends = -1200103, ///< [AuthV4-Provider] Not supported function.
			AuthV4ProviderLoginCancel = -1200104, ///< [AuthV4-Provider] Provider Login canceled.
			AuthV4FacebookUserCanceled = -1200201, ///< [AuthV4-ProviderFacebook] Login action canceled by user.
			AuthV4FacebookResponseFailGetFriends = -1200202, ///< [AuthV4-ProviderFacebook] Response error : %1
			AuthV4FacebookNetworkErrorUploadProfile = -1200203, ///< [AuthV4-ProviderFacebook] Network error occured on upload profile.
			AuthV4FacebookResponseFailUploadProfile = -1200204, ///< [AuthV4-ProviderFacebook] Response error. Failed to upload profile.
			AuthV4FacebookResponseError = -1200205, ///< [AuthV4-ProviderFacebook] Response error : %1
			AuthV4FacebookInvalidResponseData = -1200206, ///< [AuthV4-ProviderFacebook] Invalid response data : %1
			AuthV4FacebookNetworkError = -1200207, ///< [AuthV4-ProviderFacebook] Network error occured : %1
			AuthV4FacebookCancel = -1200208, ///< [AuthV4-ProviderFacebook] Login canceled
			AuthV4GoogleResponseFailLogin = -1200301, ///< [AuthV4-ProviderGoogle] Connection failed
			AuthV4GoogleResponseFailLogout = -1200302, ///< [AuthV4-ProviderGoogle] DisconnetionFailed
			AuthV4GoogleNetworkErrorUploadProfile = -1200303, ///< [AuthV4-ProviderGoogle] Network error is occured on upload profile.
			AuthV4GoogleResponseFailUploadProfile = -1200304, ///< [AuthV4-ProviderGoogle] Response error. Failed to upload profile.
			AuthV4GoogleResponseFailShowAchievements = -1200305, ///< [AuthV4-ProviderGoogle] Response error. Failed to show achievements.
			AuthV4GoogleResponseFailShowLeaderboards = -1200306, ///< [AuthV4-ProviderGoogle] Response error. Failed to show leaderboards.
			AuthV4GoogleNotSupported = -1200307, ///< [AuthV4-ProviderGoogle] Provider google is not supported.
			AuthV4GoogleLoginCancel = -1200308, ///< [AuthV4-ProviderGoogle] Google Login canceled.
			AuthV4GoogleNetworkError = -1200309, ///< [AuthV4-ProviderGoogle] Network Error
			AuthV4GoogleLogout = -1200310, ///< [AuthV4-ProviderGoogle] Logout.
			AuthV4GoogleResponseFailAchievementsReveal = -1200311, ///< [AuthV4-ProviderGoogle] Failed to achievements reveal.
			AuthV4GoogleResponseFailAchievementsUnlock = -1200312, ///< [AuthV4-ProviderGoogle] Failed to achievements unlock.
			AuthV4GoogleResponseFailAchievementsIncrement = -1200313, ///< [AuthV4-ProviderGoogle] Failed to achievements increment.
			AuthV4GoogleResponseFailLeaderboardsSubmitScore = -1200314, ///< [AuthV4-ProviderGoogle] Failed to leaderboards submit score.
			AuthV4GoogleNeedSignIn = -1200315, ///< [AuthV4-ProviderGoogle] Need ProviderGoogle sign in
			AuthV4AppleLoginCancel = -1200401, ///< [AuthV4-ProviderApple] Login canceled
			AuthV4AppleResponseFailLogin = -1200402, ///< [AuthV4-ProviderApple] Response error. login: %1
			AuthV4AppleTimeOut = -1200403, ///< [AuthV4-ProviderApple] Timeout
			AuthV4AppleResponseFailReportScore = -1200404, ///< [AuthV4-ProviderApple] Response error. Failed to report score: %1
			AuthV4AppleInProgressGameCenterVC = -1200405, ///< [AuthV4-ProviderApple] GameCenterViewController in use.
			AuthV4AppleResponseFailLoadAchievements = -1200406, ///< [AuthV4-ProviderApple] Response error. Load achievements failed: %1
			AuthV4AppleResponseFailReportAchievements = -1200407, ///< [AuthV4-ProviderApple] Response error. Report achievements failed: %1
			AuthV4AppleResponseFailResetAchievements = -1200408, ///< [AuthV4-ProviderApple] Response error. Reset achievements failed: %1
			AuthV4AppleNotSupported = -1200409, ///< [AuthV4-ProviderApple] Provider apple is not supported.
			AuthV4AppleInProgress = -1200410, ///< [AuthV4-ProviderApple] In Progress
			AuthV4AppleResponseError = -1200411, ///< [AuthV4-ProviderApple] Response error : %1
			AuthV4AppleCancel = -1200412, ///< [AuthV4-ProviderApple] Canceled
			AuthV4VKResponseFailLogin = -1200501, ///< [AuthV4-ProviderVK] Response Fail on request login : %1
			AuthV4VKInvalidParamSDK = -1200502, ///< [AuthV4-ProviderVK] VK SDK setup failed.
			AuthV4VKNotInitialized = -1200503, ///< [AuthV4-ProviderVK] VK SDK not initialized.
			AuthV4VKCancelLogin = -1200504, ///< [AuthV4-ProviderVK] Request authorization is canceled
			AuthV4VKInvalidSession = -1200505, ///< [AuthV4-ProviderVK] Invalid VK session.
			AuthV4VKResponseFailGetFriends = -1200506, ///< [AuthV4-ProviderVK] Response Fail on get friends: %1
			AuthV4VKResponseFailLogout = -1200507, ///< [AuthV4-ProviderVK] Response Fail on request logout.
			AuthV4VKResponseFailUploadProfile = -1200508, ///< [AuthV4-ProviderVK] Response Fail
			AuthV4VKNetworkErrorUploadProfile = -1200509, ///< [AuthV4-ProviderVK] Network error occured : %1
			AuthV4VKInProgress = -1200510, ///< [AuthV4-ProviderVK] In progress
			AuthV4VKResponseError = -1200511, ///< [AuthV4-ProviderVK] Response error : %1
			AuthV4VKNetworkError = -1200512, ///< [AuthV4-ProviderVK] Network error occured : %1
			AuthV4VKInvalidParam = -1200513, ///< [AuthV4-ProviderVK] Invalid param : %1
			AuthV4VKCancel = -1200514, ///< [AuthV4-ProviderVK] Canceled
			AuthV4VKTokenResponseError = -1200515, ///< [AuthV4-ProviderVK] Token Response Error : %1
			AuthV4WechatInProgressLoginLogout = -1200601, ///< [AuthV4-ProviderWeChat] Login or logout in progress.
			AuthV4WechatResponseFailLogin = -1200602, ///< [AuthV4-ProviderWeChat] Response error. Failed to login: %1
			AuthV4WechatNotSupportedRequest = -1200603, ///< [AuthV4-ProviderWeChat] Request is not supported: %1
			AuthV4WechatResponseFailUserInfo = -1200604, ///< [AuthV4-ProviderWeChat] Request error. failed to get user info: %1
			AuthV4WechatNetworkErrorUserInfo = -1200605, ///< [AuthV4-ProviderWeChat] Request error. NetworkError while getting user info: %1
			AuthV4WechatNetworkError = -1200606, ///< [AuthV4-ProviderWeChat] Network error occured : %1
			AuthV4WechatResponseFail = -1200607, ///< [AuthV4-ProviderWeChat] Response fail : %1
			AuthV4WechatNetworkErrorLogin = -1200608, ///< [AuthV4-ProviderWeChat] NetworkError. Failed to login.
			AuthV4WechatLoginCancel = -1200609, ///< [AuthV4-ProviderWeChat] Login canceled.
			AuthV4WechatInProgress = -1200610, ///< [AuthV4-ProviderWeChat] In progress
			AuthV4WechatResponseError = -1200611, ///< [AuthV4-ProviderWeChat] Response Error : %1
			AuthV4WechatInvalidResponseData = -1200612, ///< [AuthV4-ProviderWeChat] Invalid Response Data : %1
			AuthV4WechatInvalidAppKey = -1200613, ///< [AuthV4-ProviderWeChat] Invalid App Key : %1
			AuthV4HIVEDialogCancel = -1200701, ///< [AuthV4-ProviderHIVE] Dialog is canceled
			AuthV4HIVENetworkErrorUploadProfile = -1200702, ///< [AuthV4-ProviderHIVE] Network error is occured on upload profile.
			AuthV4HIVEResponseFailUploadProfile = -1200703, ///< [AuthV4-ProviderHIVE] Response error. failed to upload profile.
			AuthV4HIVEInProgress = -1200704, ///< [AuthV4-ProviderHIVE] In progress
			AuthV4HIVEResponseError = -1200705, ///< [AuthV4-ProviderHIVE] Response error : %1
			AuthV4HIVENetworkError = -1200706, ///< [AuthV4-ProviderHIVE] Network Error
			AuthV4HIVEInvalidParam = -1200707, ///< [AuthV4-ProviderHIVE] Invalid param : %1
			AuthV4HIVECancel = -1200708, ///< [AuthV4-ProviderHIVE] Cancel
			AuthV4QQInProgressLoginLogout = -1200801, ///< [AuthV4-ProviderQQ] Login or logout in progress.
			AuthV4QQResponseFailLogin = -1200802, ///< [AuthV4-ProviderQQ] Invalid login session.
			AuthV4QQCancelLogin = -1200803, ///< [AuthV4-ProviderQQ] login is canceled.
			AuthV4QQNetworkError = -1200804, ///< [AuthV4-ProviderQQ] Network error occurred.
			AuthV4QQNetworkErrorUploadProfile = -1200805, ///< [AuthV4-ProviderQQ] Network error is occurred on upload profile
			AuthV4QQResponseFailUploadProfile = -1200806, ///< [AuthV4-ProviderQQ] Response error. Failed to upload profile
			AuthV4QQCancelUploadProfile = -1200807, ///< [AuthV4-ProviderQQ] Upload profiel is canceled.
			AuthV4QQResponseFailLogout = -1200808, ///< [AuthV4-ProviderQQ] Response error. Failed to logout.
			AuthV4QQNotInitialized = -1200809, ///< [AuthV4-ProviderQQ] QQ SDK not initialized.
			AuthV4QQInProgress = -1200810, ///< [AuthV4-ProviderQQ] In progress
			AuthV4QQResponseError = -1200811, ///< [AuthV4-ProviderQQ] Response error
			AuthV4QQInvalidResponseData = -1200812, ///< [AuthV4-ProviderQQ] Invalid response data : %1
			AuthV4QQInvalidParam = -1200813, ///< [AuthV4-ProviderQQ] Invalid param : %1
			AuthV4QQCancel = -1200814, ///< [AuthV4-ProviderQQ] Cancel
			AuthV4SignInAppleUnknown = -1200901, ///< [AuthV4-ProviderSignInApple] Unknown
			AuthV4SignInAppleCanceled = -1200902, ///< [AuthV4-ProviderSignInApple] Canceled
			AuthV4SignInAppleFailed = -1200903, ///< [AuthV4-ProviderSignInApple] Failed
			AuthV4SignInAppleInvalidResponse = -1200904, ///< [AuthV4-ProviderSignInApple] Invalid Response
			AuthV4SignInAppleNotHandled = -1200905, ///< [AuthV4-ProviderSignInApple] Not Handled
			AuthV4SignInAppleNotSupported = -1200906, ///< [AuthV4-ProviderSignInApple] Not Supported
			AuthV4LineInvalidParam = -1201001, ///< [AuthV4-ProviderLine] Invalid Param
			AuthV4LineCancel = -1201002, ///< [AuthV4-ProviderLine] Cancel
			AuthV4LineResponseError = -1201003, ///< [AuthV4-ProviderLine] Response Error
			AuthV4LineNetworkError = -1201004, ///< [AuthV4-ProviderLine] Network Error
			AuthV4TwitterResponseError = -1201101, ///< [AuthV4-ProviderTwitter] Response Error
			AuthV4TwitterInvalidParam = -1201102, ///< [AuthV4-ProviderTwitter] Invalid Param
			AuthV4WeverseInProgressLoginLogout = -1201201, ///< [AuthV4-ProviderWeverse] Login or Logout in progress.
        	AuthV4WeverseResponseFailLogin = -1201202, ///< [AuthV4-ProviderWeverse] Response error. Failed to login: %s
			AuthV4WeverseNotSupported = -1201203, ///< [AuthV4-ProviderWeverse] Not supported: %s
			AuthV4WeverseNetworkError = -1201204, ///< [AuthV4-ProviderWeverse] Network error occurred : %s
			AuthV4WeverseLoginCancel = -1201205, ///< [AuthV4-ProviderWeverse] Login canceled.
			AuthV4WeverseResponseError = -1201206, ///< [AuthV4-ProviderWeverse] Response Error : %s
			AuthV4NotRegisteredDevice = -1201300, ///< [AuthV4-DeviceManagement] Not Registered Device.          
			AuthV4HuaweiNotInitialized = -1201401, ///< [AuthV4-ProviderHuawei] Huawei SDK not initialized.
			AuthV4HuaweiInProgress = -1201402, ///< [AuthV4-ProviderHuawei] In progress
			AuthV4HuaweiInvalidParam = -1201403, ///< [AuthV4-ProviderHuawei] Invalid Param
			AuthV4HuaweiLoginCancel = -1201404, ///< [AuthV4-ProviderHuawei] Login Cancel
			AuthV4HuaweiNetworkError = -1201405, ///< [AuthV4-ProviderHuawei] Network Error
			AuthV4HuaweiResponseError = -1201406, ///< [AuthV4-ProviderHuawei] Response Error
			SocialResponseFailDismissDialog = -2000001, ///< [Social-Common] InvalidParamClose
			SocialCancelDismissDialog = -2000002, ///< [Social-Common] Canceled
			SocialCancelConnect = -2000003, ///< [Social-Common] Canceled social connect.
			SocialResponseFailConnect = -2000004, ///< [Social-Common] Fail to connect to social.
			SocialCancelGetPictureFromGallery = -2000005, ///< [Social-Common] Canceled to get picture from gallery.
			SocialResponseFailGetPictureFromGallery = -2000006, ///< [Social-Common] Failed to get picture from gallery.
			SocialCancelSharePhoto = -2000007, ///< [Social-Common] Canceled to share photo.
			SocialResponseFailSharePhoto = -2000008, ///< [Social-Common] Failed to share photo.
			SocialInvalidParam = -2000009, ///< [Social-Common] Invalid param : %s
			SocialProviderNotInitialized = -2000010, ///< [Social-Common] Social Provider Not Initialized.
			SocialNeedSignIn = -2000011, ///< [Social-Common] Need signIn.
			SocialGoogleNotInitialized = -2000101, ///< [Social-Google] Not initialized. initialize user first.
			SocialGoogleResponseFailGetProfile = -2000102, ///< [Social-Google] Fail to get profile. check response
			SocialGoogleInProgressConnect = -2000103, ///< [Social-Google] In progress. connect.
			SocialGoogleResponseFailConnect = -2000104, ///< [Social-Google] Response error. Failed to connect.
			SocialGoogleCancelGetProfile = -2000105, ///< [Social-Google] Get profile is canceled.
			SocialGoogleCancelConnect = -2000106, ///< [Social-Google] Connect is canceled.
			SocialGoogleNetworkErrorUpdateServerFlag = -2000107, ///< [Social-Google] Network error is occured on update server flag.
			SocialGoogleResponseFailUpdateServerFlag = -2000108, ///< [Social-Google] Response error. Failed to update server flag.
			SocialFacebookNotInitialized = -2000201, ///< [Social-Facebook] Not initialized. initialize user first.
			SocialFacebookResponseFailGetProfile = -2000202, ///< [Social-Facebook] Fail to get profile. check response: %1
			SocialFacebookCancelGetProfile = -2000203, ///< [Social-Facebook] Get profile canceled.
			SocialFacebookResponseFailGetFriends = -2000204, ///< [Social-Facebook] Fail to get friends. check response: %1
			SocialFacebookCancelGetFriends = -2000205, ///< [Social-Facebook] Get friends canceled.
			SocialFacebookResponseFailSendMessage = -2000206, ///< [Social-Facebook] Fail to send message. check response: %1
			SocialFacebookCancelSendMessage = -2000207, ///< [Social-Facebook] Send message canceled.
			SocialFacebookMessageDialogShowFail = -2000208, ///< [Social-Facebook] Fail to show message dialog.
			SocialFacebookResponseFailShowInvitation = -2000209, ///< [Social-Facebook] Fail to show invite dialog: %1
			SocialFacebookCancelShowInvitation = -2000210, ///< [Social-Facebook] Show invite dialog canceled
			SocialFacebookInvalidParamPost = -2000211, ///< [Social-Facebook] Invalid paramter for posting to facebook.
			SocialFacebookPostDialogShowFail = -2000212, ///< [Social-Facebook] Fail to show post dialog.
			SocialFacebookShareFail = -2000213, ///< [Social-Facebook] Fail to share: %1
			SocialFacebookShareCancelled = -2000214, ///< [Social-Facebook] Sharing canceled.
			SocialFacebookSendInvitationFail = -2000215, ///< [Social-Facebook] Fail to send invitation: %1
			SocialFacebookSendInvitationCancelled = -2000216, ///< [Social-Facebook] Send invitation is canceled.
			SocialFacebookCancelPost = -2000217, ///< [Social-Facebook] Posting is canceled.
			SocialFacebookResponseFailPost = -2000218, ///< [Social-Facebook] Response error. Failed to posting.
			SocialFacebookOperationException = -2000219, ///< [Social-Facebook] Operation exception is occured.
			SocialFacebookServiceException = -2000220, ///< [Social-Facebook] Service exception is occured.
			SocialFacebookException = -2000221, ///< [Social-Facebook] Exception is occured.
			SocialHIVENotInitialized = -2000301, ///< [Social-HIVE] Not initialized. initialize user first.
			SocialHIVENetworkErrorGetMyProfile = -2000302, ///< [Social-HIVE] Network error occured on Get My Profile: %1
			SocialHIVEInvalidParamSetMyProfile = -2000303, ///< [Social-HIVE] Invalid parameter. Comment is required
			SocialHIVENetworkErrorSetMyProfile = -2000304, ///< [Social-HIVE] Network error occured on Set My Profile: %1
			SocialHIVENetworkErrorGetFriends = -2000305, ///< [Social-HIVE] Network error occured on Get Friends: %1
			SocialHIVEInvalidSession = -2000306, ///< [Social-HIVE] Invalid session. Only HIVE user is permitted
			SocialHIVEInvalidParamVID = -2000307, ///< [Social-HIVE] Invalid parameter. Valid VID is required.
			SocialHIVENetworkErrorGetVIDByUIDList = -2000308, ///< [Social-HIVE] Network error occured on Get VID By UID List: %1
			SocialHIVENetworkErrorGetUIDByVIDList = -2000309, ///< [Social-HIVE] Network error occured on Get UID By VID List: %1
			SocialHIVEInvalidParamContentSendMessage = -2000310, ///< [Social-HIVE] Invalid parameter. Message content is required.
			SocialHIVENetworkErrorSendMessage = -2000311, ///< [Social-HIVE] Network error occured on Send Message: %1
			SocialHIVEInvalidParamReceiptSendMessage = -2000312, ///< [Social-HIVE] Invalid parameter. Need valid receipts for Send Message.
			SocialHIVEInvalidParamUID = -2000313, ///< [Social-HIVE] Invalid parameter. UID is empty or nil
			SocialHIVEInProgressSocialDialog = -2000314, ///< [Social-HIVE] In progress. SocialDialog is already showing.
			SocialHIVESocialDialogClosed = -2000315, ///< [Social-HIVE] SocialDialog is closed.
			SocialHIVENetworkErrorGetBadgeInfo = -2000316, ///< [Social-HIVE] Network error occured on Get Badge Info: %1
			SocialHIVEResponseFailGetMyProfile = -2000317, ///< [Social-HIVE] Response error is occured on Get My Profile: %1
			SocialHIVEResponseFailSetMyProfile = -2000318, ///< [Social-HIVE] Response error is occured on Set My Profile: %1
			SocialHIVEResponseFailGetFriends = -2000319, ///< [Social-HIVE] Response error is occured on Get Friends: %1
			SocialHIVEResponseFailGetVID = -2000320, ///< [Social-HIVE] Response error occured on Get VID: %1
			SocialHIVEResponseFailInvalidVIDList = -2000321, ///< [Social-HIVE] Server response vid list does not match.
			SocialHIVEResponseFailGetUID = -2000322, ///< [Social-HIVE] Response error occured on Get UID: %1
			SocialHIVEResponseFailInvalidUIDList = -2000323, ///< [Social-HIVE] Server response uid list does not match.
			SocialHIVEResponseFailSendMessage = -2000324, ///< [Social-HIVE] Response error is occured on Send Message: %1
			SocialHIVEResponseFailGetBadgeInfo = -2000325, ///< [Social-HIVE] Response error is occured on Get badge: %1
			SocialHIVEResponseFailDialogWebView = -2000326, ///< [Social-HIVE] WebView error is occured in social dialog: %1
			SocialHIVEResponseFailSocialDialog = -2000327, ///< [Social-HIVE] Social dialog error is occured.
			SocialHIVEInvalidParamFriendType = -2000328, ///< [Social-HIVE] Requested friend type %1 is not supported.
			SocialHIVEResponseFailGetProfiles = -2000329, ///< [Social-HIVE] Response error. Failed to get profiles.
			SocialHIVEInvalidParamGetProfiles = -2000330, ///< [Social-HIVE] Invalid param get profiles.
			SocialHIVEResponseFail = -2000331, ///< [Social-HIVE] Response error. 
			SocialHIVEInvalidParamSendMessage = -2000332, ///< [Social-HIVE] Invalid parameter. Message content is required.
			SocialHIVEInvalidParamSendInvitationMessage = -2000333, ///< [Social-HIVE] Invalid parameter. Send invitation message.
			SocialHIVEResponseFailGetPictureFromGallery = -2000334, ///< [Social-HIVE] Response error. Failed to get picture from gallery.
			SocialHIVEResponseFailGetPictureFromCamera = -2000335, ///< [Social-HIVE] Response error. Failed to get picture from camera.
			SocialHIVENetworkErrorSendInvitationMessage = -2000336, ///< [Social-HIVE] Network error is occured on send invitation message.
			PromotionNotInitialized = -3000001, ///< [Promotion] Setup HIVE module first.
			PromotionAlreadyShowing = -3000002, ///< [Promotion] Promotion view already shown.
			PromotionNetworkErrorShowCustomContents = -3000003, ///< [Promotion] Network error occured on Show Custom Contents: %1
			PromotionNetworkErrorShowOfferwall = -3000004, ///< [Promotion] Network error occured on Show Offerwall
			PromotionShowDialogFail = -3000005, ///< [Promotion] Fail to show Dialog. Please check data from promotion server: %1
			PromotionResponseFailGetViewInfo = -3000006, ///< [Promotion] Response error. Failed to get view info
			PromotionNetworkError = -3000007, ///< [Promotion] Network error occured : %1
			PromotionServerResponseError = -3000008, ///< [Promotion] Server response error : %1
			PromotionInvalidResponseData = -3000009, ///< [Promotion] Invalid response data : %1
			PromotionShareFailed = -3000010, ///< [Promotion] Fail to share.
			UserEngagementResponseFail = -3200001, ///< [Promotion UE] Response error.
			UserEngagementAlreadySetReady = -3200002, ///< [Promotion UE] Already in ready to UE.
			UserEngagementHandlerNotRegistered = -3200003, ///< [Promotion UE] Handler not registered.
			UserEngagementNotLogined = -3200004, ///< [Promotion UE] Unauthorized User. Please do login first.
			UserEngagementEmptyCouponId = -3200005, ///< [Promotion UE] Coupon value is empty.
			UserEngagementEmptyMarketPid = -3200006, ///< [Promotion UE] Market pid value is empty.
			UserEngagementListenerNotRegistered = -3200007, ///< [Promotion UE] Listener is not registered.
			PromotionStartPlayback = -3300001, ///< [Promotion MV] Start playback.
			PromotionFinishPlayback = -3300002, ///< [Promotion MV] Finish playback.
			PromotionYTPlayerError = -3300003, ///< [Promotion MV] Youtube player get error : %1
			PromotionCancelPlayback = -3300004, ///< [Promotion MV] Cancel playback.
			PushNotInitialized = -4000001, ///< [Push] Setup HIVE module first.
			PushInvalidParamLocalPush = -4000002, ///< [Push] Invalid param : %1
			PushInvalidParamRemotePush = -4000003, ///< [Push] Invalid param : %1
			PushNetworkError = -4000004, ///< [Push] Network error occured : %1
			PushAgeGateUnder13 = -4000005, ///< [Push] Push can not use under 13 years old people because of age verification system.
			PushInvalidResponseData = -4000006, ///< [Push] Invalid response data : %1
			PushServerResponseError = -4000007, ///< [Push] Server response error : %1
			PushNeedSignIn = -4000008, ///< [Push] Need signin.
			IAPNotInitialize = -6000001, ///< [IAP] Not Initialized. Initialize User First.
			IAPAlreadyInInitialize = -6000002, ///< [IAP] Already IAP Initialized.
			IAPNetworkError = -6000003, ///< [IAP] Network error occured: %1
			IAPNotSupportedMarket = -6000004, ///< [IAP] Invalid or NOT supported market identifier. (%1)
			IAPNeedLogin = -6000005, ///< [IAP] Unauthorized User. Please do login first.
			IAPNeedShopInitialize = -6000006, ///< [IAP] Not Shop Initialized. Initialize Shop First.
			IAPNotSupportedOSVersion = -6000007, ///< [IAP] Unsupported OS Version.
			IAPNeedRestore = -6000008, ///< [IAP] Unfinished transactions: %1 exists. Need restore.
			IAPNothingToRestore = -6000010, ///< [IAP] Nothing to restore.
			IAPFailRestore = -6000011, ///< [IAP] Fail to restore.
			IAPRestrictPayments = -6000012, ///< [IAP] This device is not able or allowed to make payments.
			IAPNetworkJsonException = -6000013, ///< [IAP] Json Exception during IAP Network
			IAPMarketNotSupportedAPI = -6000014, ///< [IAP] Market did not support this API
			IAPFailMarketInitialize = -6000015, ///< [IAP] Market data is nothing
			IAPResponseError = -6000016, ///< [IAP] Network response exception is occured.
			IAPDialogActionDefault_DoExit = -6000017, ///< [IAP] IAP Dialog Dismiss with unknown reason
			IAPDialogActionOpenURL_DoExit = -6000018, ///< [IAP] IAP Dialog Dismiss with OpenURL button selected
			IAPDialogActionExit_DoExit = -6000019, ///< [IAP] IAP Dialog Dismiss with Exit button selected
			IAPDialogActionDone = -6000020, ///< [IAP] IAP Dialog Dismiss with Done button selected
			IAPDialogTimeover_DoExit = -6000021, ///< [IAP] IAP Dialog Dismiss with Timeover
			IAPInProgressPurchasing = -6000101, ///< [IAP] Already in progress purchasing.
			IAPInProgressRestoring = -6000102, ///< [IAP] Already in progress restoring.
			IAPInProgressCheckPromotePurchase = -6000103, ///< [IAP] Already in progress check promote purchase.
			IAPInProgressConnectingAppStore = -6000104, ///< [IAP] Already in progress connecting App Store.
			IAPInProgressMarketSelection = -6000105, ///< [IAP] Already in progress market selection.
			IAPAppStoreError = -6000201, ///< [IAP] App Store Error: %1
			IAPAppStoreResponseEmpty = -6000202, ///< [IAP] Fail to get response data from App Store. Response data is empty.
			IAPCannotFindGamePID = -6000301, ///< [IAP] Can not find game pid matching with market pid, PID is empty or nil.
			IAPProductNotExist = -6000302, ///< [IAP] Can not find product in IAP Server: %1
			IAPEmptyMarketPID = -6000303, ///< [IAP] Fail to request purchase product. MarketPID is empty or nil.
			IAPFailCreateSKPayment = -6000304, ///< [IAP] SKPayment create failed.
			IAPEmptyTransaction = -6000305, ///< [IAP] Fail to request purchase. Purchased transaction is empty.
			IAPEmptyProduct = -6000306, ///< [IAP] Can not find market product using marker pid: %1
			IAPCancelPayment = -6000307, ///< [IAP] Failed to purchase(canceled): %1
			IAPFailPayment = -6000308, ///< [IAP] Failed to purchase
			IAPInvalidMarketPID = -6000309, ///< [IAP] Failed to convert to MarketPIDs to PID set.
			IAPPurchaseParamJsonException = -6000310, ///< [IAP] Purchase parameter json exception is occured.
			IAPShopInfoParamJsonException = -6000311, ///< [IAP] Shopinfo parameter json exception is occured.
			IAPBadgeParamJsonException = -6000312, ///< [IAP] Badge parameter json exception is occured.
			IAPInvalidParamEmptyMarketPID = -6000401, ///< [IAP] Game pid is empty.
			IAPInvalidParamLocationCode = -6000402, ///< [IAP] Location code is empty
			IAPBlockedUser = -6000501, ///< [IAP] This user has been blocked: %1
			IAPPromoCodeMatchMultiMarketPID = -6000502, ///< [IAP] Multiple product matched in market pid: %1
			IAPPromoCodeNotMatchMarketPID = -6000503, ///< [IAP] No product matched in market pid: %1
			IAPPromoCodeAlreadyUsed = -6000504, ///< [IAP] This Promotion code already used: %1
			IAPAppleReceiptNotConnected = -6000505, ///< [IAP] Apple receipt verifying server can not connected: %1
			IAPServerDefaultError = -6000506, ///< [IAP] IAP server error: %1
			IAPInitializeMarketListIsEmpty = -6000601, ///< [IAP] Resopnse failed. Market list is empty.
			IAPInitializeMarketURLISEmpty = -6000602, ///< [IAP] Resopnse failed. Market url is empty.
			IAPEmptyMarketURL = -6000603, ///< [IAP] Selected market URL is empty.
			IAPNotSelectedMarket = -6000604, ///< [IAP] Failed to select market.
			IAPPlayStoreLaunchPurchaseFlowException = -6000701, ///< [IAP] PlayStore launchpurchaseflow exception is occured.
			IAPPlayStoreSetupFail = -6000702, ///< [IAP] PlayStore start setup fail
			IAPPlayStoreQueryInventoryFail = -6000703, ///< [IAP] PlayStore query inventory fail when initialize
			IAPPlayStorePending = -6000704, ///< [IAP] PlayStore Billing state is pending.
			IAPOneStoreProductListEmpty = -6000801, ///< [IAP] OneStore Product List is empty.
			IAPOneStoreProductNetworkError = -6000802, ///< [IAP] OneStore requestProductInfo network fail
			IAPOneStoreProductInfoError = -6000803, ///< [IAP] OneStore requestProductInfo fail.
			IAPOneStoreInvalidRequestID = -6000804, ///< [IAP] OneStore requestID is invalid
			IAPOneStorePurchaseError = -6000805, ///< [IAP] OneStore purchase fail
			IAPOneStoreNetworkNullError = -6000806, ///< [IAP] Onestore Network response null error
			IAPOneStoreNetworkInvalidError = -6000807, ///< [IAP] Onestore Network response invalid error
			IAPLebiInitializeNetworkError = -6000901, ///< [IAP] Lebi Initialize fail
			IAPLebiInitializeJsonException = -6000902, ///< [IAP] Lebi initialize json exception is occured.
			IAPLebiPurchaseNetworkError = -6000903, ///< [IAP] Lebi purchase fail
			IAPLebiPurchaseJsonException = -6000904, ///< [IAP] Lebi purchase json exception is occured.
			IAPLebiVerifyOrderNetworkError = -6000905, ///< [IAP] Lebi verifyorder fail
			IAPLebiVerifyOrderJsonException = -6000906, ///< [IAP] Lebi verifyorder json exception is occured.
			IAPLebiBalanceNetworkError = -6000907, ///< [IAP] Lebi balance fail
			IAPLebiBalanceParamJsonException = -6000908, ///< [IAP] Lebi balance json exception is occured.
			IAPLebiPostException = -6000909, ///< [IAP] Lebi post exception is occured.
			IAPLebiInternalRequestException = -6000910, ///< [IAP] Lebi Internal request exception is occured.
			IAPLebiRestoreNetworkError = -6000911, ///< [IAP] Lebi restore create invalid response data
			IAPV4NotInitialize = -6100001, ///< [IAPv4] Setup HIVE module first.
			IAPV4NetworkError = -6100002, ///< [IAPv4] Network error occured: %1
			IAPV4NotSupportedMarket = -6100003, ///< [IAPv4] Market not supported.
			IAPV4NeedSignIn = -6100004, ///< [IAPv4] Unauthorized User. Please do sign in first.
			IAPV4NeedMarketConnect = -6100005, ///< [IAPv4] Shop not initialized. Initialize shop first.
			IAPV4NeedRestore = -6100006, ///< [IAPv4] Unfinished transactions: %1 exists. Need restore.
			IAPV4NothingToRestore = -6100007, ///< [IAPv4] Nothing to restore.
			IAPV4FailToRestore = -6100008, ///< [IAPv4] Fail to restore.
			IAPV4RestrictPayments = -6100009, ///< [IAPv4] This device not able or allowed to make payments.
			IAPV4FailMarketConnect = -6100010, ///< [IAPv4] Invalid market data : %1
			IAPV4ResponseError = -6100011, ///< [IAPv4] Invalid response data : %1
			IAPV4MarketNotSupportedAPI = -6100012, ///< [IAPv4] Market did not support this API
			IAPV4PendingPurchase = 6100013, ///< [IAPv4] A pending purchase occurs. : %s
			IAPV4InProgressMarketConnect = -6100101, ///< [IAPv4] Already in progress market connect.
			IAPV4InProgressPurchasing = -6100102, ///< [IAPv4] Already in progress purchasing.
			IAPV4InProgressRestoring = -6100103, ///< [IAPv4] Already in progress restoring.
			IAPV4InProgressCheckPromote = -6100104, ///< [IAPv4] Already in progress check promote purchase.
			IAPV4InProgressConnectAppStore = -6100105, ///< [IAPv4] Already in progress connect App Store.
			IAPV4InProgressMarketSelect = -6100106, ///< [IAPv4] Already in progress market select.
			IAPV4AppStoreError = -6100201, ///< [IAPv4] App store error: %1
			IAPV4AppStoreResponseEmpty = -6100202, ///< [IAPv4] Fail to get response data from App Store. Response data is empty.
			IAPV4ProductNotExsitInAppStore = -6100203, ///< [IAPv4] Can not find product: %1
			IAPV4FinishMarketPidEmpty = -6100301, ///< [IAPv4] Empty finished market PID.
			IAPV4PromoteMarketPidEmpty = -6100302, ///< [IAPv4] Empty market pid.
			IAPV4FailCreateSKPayment = -6100303, ///< [IAPv4] Fail to create payment.
			IAPV4FailToConvertNSSet = -6100304, ///< [IAPv4] Fail to conver to market pids to pid set.
			IAPV4InvalidLogType = -6100305, ///< [IAPv4] Invalid log type.
			IAPV4ProductNotExist = -6100306, ///< [IAPv4] Failed to request purchase product. Market product not found.
			IAPV4RequestProductJsonException = -6100307, ///< [IAPv4] Reqeust product json exception occured.
			IAPV4PurchaseParamJsonException = -6100308, ///< [IAPv4] Purchase parameter json exception occured.
			IAPV4RequestMarketJsonException = -6100309, ///< [IAPv4] Reqeust market json exception occured.
			IAPV4ProductInfoJsonException = -6100310, ///< [IAPv4] Product info json exception occured.
			IAPV4CancelPayment = -6100311, ///< [IAPv4] Failed to purchase(canceled)
			IAPV4FailPayment = -6100312, ///< [IAPv4] Failed to purchase
			IAPV4EmptyParamMarketPID = -6100401, ///< [IAPv4] Empty market pid.
			IAPV4EmptyMarketList = -6100501, ///< [IAPv4] Empty Market list from IAP server.
			IAPV4EmptyMarketURL = -6100502, ///< [IAPv4] Response failed. Market selection URL is emptry or nil.
			IAPV4MarketPidListEmptyInIAPServer = -6100503, ///< [IAPv4] Failed to request product indentifiers. MarketPID is empty or nil.
			IAPV4EmptyProductList = -6100504, ///< [IAPv4] IAP need get product info. get product info or showPayment API.
			IAPV4ProductNotExistInIAPServer = -6100505, ///< [IAPv4] Invalid parameter. Unknown market product identifier: %1
			IAPV4PlayStoreLaunchPurchaseFlowException = -6100701, ///< [IAPv4] PlayStore launchpurchaseflow exception occured.
			IAPV4PlayStoreSetupFail = -6100702, ///< [IAPv4] PlayStore start setup fail
			IAPV4PlayStoreQueryInventoryFail = -6100703, ///< [IAPv4] PlayStore query inventory fail when initialize
			IAPV4PlayStoreFinishFail = -6100704, ///< [IAPv4] PlayStore Finish fail.
			IAPV4PlayStorePending = -6100705, ///< [IAPv4] PlayStore Billing state is pending.
			IAPV4OneStoreV5RemoteException = -6100801, ///< [IAPv4] IAPV4OneStoreV5RemoteException
			IAPV4OneStoreV5SecurityException = -6100802, ///< [IAPv4] IAPV4OneStoreV5SecurityException
			IAPV4OneStoreV5NeedUpdateException = -6100803, ///< [IAPV4]OneStoreV5NeedUpdateException
			IAPV4OneStoreCancelPayment = -6100804, ///< [IAPV4] Cancel Payment
			IAPV4OneStoreNetworkError = -6100805, ///< [IAPV4] Can not connect to the OneStore
			IAPV4OneStoreResponseError = -6100806, ///< [IAPV4] Error response in OneStore
			IAPV4OneStoreProductNotExist = -6100807, ///< [IAPV4] Product not in sale
			IAPV4OneStoreFailPayment = -6100808, ///< [IAPV4] Failed to purchase
			IAPV4OneStoreError = -6100809, ///< [IAPV4] Error in OneStore %1
			IAPV4OneStoreNeedRestore = -6100810, ///< [IAPV4] Already owned the item
			IAPV4OneStoreNothingToRestore = -6100811, ///< [IAPV4] Did not owned the item
			IAPV4OneStoreUnablePayment = -6100812, ///< [IAPV4] Unable Payment. Check your credit.
			IAPV4OneStoreNeedSignIn = -6100813, ///< [IAPV4] Need OneStore signIn
			IAPV4OneStoreException = -6100814, ///< [IAPV4] Can not use OneStore
			IAPV4OneStoreInvalidResponseData = -6100815, ///< [IAPV4] Get invalid data from OneStore
			IAPV4OneStoreInvalidParam = -6100816, ///< [IAPV4] Invalid parameter.
			IAPV4OneStoreFinishFail = -6100817, ///< [IAPV4] OneStore Finish fail.
			IAPV4OneStoreRefund = -6100818, ///< [IAPv4] OneStore Refund.
			IAPV4LebiInitializeNetworkError = -6100901, ///< [IAPv4] Lebi Initialize fail
			IAPV4LebiInitializeJsonException = -6100902, ///< [IAPv4] Lebi initialize json exception occured.
			IAPV4LebiPurchaseNetworkError = -6100903, ///< [IAPv4] Lebi purchase fail
			IAPV4LebiPurchaseJsonException = -6100904, ///< [IAPv4] Lebi purchase json exception occured.
			IAPV4LebiVerifyOrderNetworkError = -6100905, ///< [IAPv4] Lebi verifyorder fail
			IAPV4LebiVerifyOrderJsonException = -6100906, ///< [IAPv4] Lebi verifyorder json exception is occured.
			IAPV4LebiBalanceNetworkError = -6100907, ///< [IAPv4] Lebi balance fail
			IAPV4LebiBalanceParamJsonException = -6100908, ///< [IAPv4] Lebi balance json exception is occured.
			IAPV4LebiPostException = -6100909, ///< [IAPv4] Lebi post exception is occured.
			IAPV4LebiInternalRequestException = -6100910, ///< [IAPv4] Lebi Internal request exception is occured.
			IAPV4LebiRestoreNetworkError = -6100911, ///< [IAPv4] Lebi restore create invalid response data
			IAPV4LebiCancel = -6100912, ///< [IAPv4] Lebi Charge cancel.
			IAPV4LebiFinishFail = -6100913, ///< [IAPv4] Lebi Finish fail.
			IAPV4CancelMarketSelect = -6100914, ///< [IAPv4] market select cancel.
			IAPV4ServerResponseError = -6100915, ///< [IAPv4] Server response error is occured. : %1
			IAPV4HiveStoreSuccess = -6110000, ///< [IAPv4] Desktop Platform HiveStore Success
			PlatformHelperOSNotSupported = -7000001, ///< [PlatformHelper] %1 is not supported.
			PlatformHelperOSVersionNotSupported = -7000002, ///< [PlatformHelper] %1 OS version is not supported.
			PlatformHelperEmptyPermissions = -7000003,  ///< [PlatformHelper] Permissions is empty. : %1
			DataStoreNotExistKey = -8000001, ///< [DataStore] Key is not exist.
    		DataStoreNotExistColumn = -8000002, ///< [DataStore] Column Family(table) is not exist.
    		DataStoreNotExistPublicKey = -8000003, ///< [DataStore] Public Key is not exist.
    		DataStoreNotInitialized = -8000004, ///< [DataStore] Need Auth Setup.
    		DataStoreNeedSignIn = -8000005, ///< [DataStore] Need Sign In.
    		DataStoreDisabled = -8000006, ///< [DataStore] DataStore disabled.
    		DataStoreResponseError = -8000007, ///< [DataStore] Server resposne error.
    		DataStoreInvalidParam = -8000008, ///< [DataStore] Invalid parameter.
			DataStoreGameIsBeingInspected = -8000009, ///< [DataStore] Game is being inspected.
			// RESULT API END
		}

		public enum ErrorCode : int {

            SUCCESS = 0,
            // RESTORE_NOT_OWNED = 10,		//(복구 할 게 없을 경우, 실패 아님)

            /**
			* API 호출에는 성공했으나 결과값을 소유하고 있지 않을 경우, 실패 아님<br/>
			* 현재는 iOS AppStore 용으로 checkPromotePurchase() 에서 발생
			*
			* @since 4.4.1
			*/
            NOT_OWNED = 10,

            /**
			* 결제는 성공했으나 게임서버 이상 등으로 아직 지급이 보류된 상태. IAP 서버에서 자동으로 재지급 된다.<br/>
			* iapTransactionId 는 존재하며, 클라이언트는 트랜젝션 종료.<br/>
			* IAP.purchase(), IAP.restore(), IAP.restoreReceipt() 에서 발생
			*/
            ITEM_DELIVERY_DELAYED = 11,

            /**
			* 결제를 Offline에서 진행하도록 요청함.<br/>
			* 클라이언트는 트랜젝션 종료.<br/>
			* IAP.purchase() 에서 발생
			*/
            ITEM_PENDING = 12,

            /**
			* 이미 인증되어 있는 상태 (서버 1001) <br/>
			* AuthV4.connect() 에서 발생
			*/
            AUTHORIZED = 20,
            IAPSUCCESS = 90,   //  Desktop Platform HIVE Store.
            INVALID_PARAM = -1,
            NOT_SUPPORTED = -2,
            IN_PROGRESS = -3,
            TIMEOUT = -4,
            NETWORK = -5,
            CANCELED = -6,
            NEED_INITIALIZE = -7,
            RESPONSE_FAIL = -8,
            INVALID_SESSION = -9,
            NEED_RESTORE = -10,

            /**
			* 다른 플레이어와 인증되어 있는 상태 (서버 1002) <br/>
			* AuthV4.connect() 에서 발생
			*/
            CONFLICT_PLAYER = -11,

            /**
			* 제재 상태의 유저 <br/>
			* AuthV4.signIn(), AuthV4.checkBlacklist() 에서 발생
			*/
            BLACKLIST = -12,

            /**
			* 프로모션 코드용 마켓PID에 대한 활성화된 HIVE IAP상품이 하나도 없거나<br/>
	 		* 여러개 활성화 되어있을 경우 등 백오피스 설정이 잘못 되어있는 상태<br/>
	 		* IAP.purchase(), IAP.restore(), IAP.restoreReceipt() 에서 발생
	 		*/
            DEVELOPER_ERROR = -13,

            /**
	 		* 동일한 프로모션의 프로모션 코드를 중복 사용하였을 경우<br/>
	 		* IAP.purchase(), IAP.restore(), IAP.restoreReceipt() 에서 발생
	 		*/
            DUPLICATED_PROMOTION_CODE = -14,

            PLAYER_CHANGE = -15,

			/**
			* Hive 웹뷰에서 정식 유저 탈퇴 시 서버 신호에 따라<br/>
			* SocialHiveDialog 콜백으로 탈퇴 여부를 전달하기 위한 응답값 (GCPSDK4-844)
			*/
			USER_OUT = -16,

			/**
			* 종료 이벤트 처리가 필요한 상태
			*/
			NEED_EXIT = -17,

			UNDEFINED = -98,

            UNKNOWN = -99,
        }
    }
}
