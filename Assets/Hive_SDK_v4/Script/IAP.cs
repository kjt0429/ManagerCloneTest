/**
 * @file    IAP.cs
 *
 *  @date		2016-2022
 *  @copyright	Copyright © Com2uS Platform Corporation. All Right Reserved.
 *  @author		ryuvsken, hife
 *  @since		4.0.0
 */
using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;

/**
 * @defgroup IAP
 * @{
 * \~korean HIVE IAP 는 마켓별 구매 프로세스의 개발 비용을 최소화 할 수 있고 구매 로그를 일관성 있게 수집하여 사업, 개발, 관리 부서에 필요한 정보를 제공할 수 있다.<br/> 
 * 현재 HIVE IAP 가 제공하는 결제 마켓은 애플 앱스토어, 구글 플레이 스토어, 원스토어 (https://www.tstore.co.kr), HIVE 러비(Lebi) 을 제공한다.<br/>
 * HIVE 러비(Lebi) 상점은 중국 시장에서 상점을 구성하고 상품을 구매 할 수 있는 시스템으로서 HIVE 에서 자체적으로 운영하고 있다.<br/><br/>
 * \~english HIVE IAP can minimize the development costs of the market-specific purchase process and collect purchase logs consistently to provide information to business, development, and management departments. <br/> * Currently, HIVE IAP supports Apple App Store, Google Play Store, One Store (https://www.tstore.co.kr) and HIVE Lobby (Lebi).<br/>
 * HIVE LIVE (Lebi) store is a system that can organize stores and purchase goods in Chinese market. It is operated by HIVE itself.<br/><br/>
 * \~
 */

namespace hive
{
	/**
	 *  
	 * \~korean HIVE IAP 는 마켓별 구매 프로세스의 개발 비용을 최소화 할 수 있고 구매 로그를 일관성 있게 수집하여 사업, 개발, 관리 부서에 필요한 정보를 제공할 수 있다.<br/> 
	 * 현재 HIVE IAP 가 제공하는 결제 마켓은 애플 앱스토어, 구글 플레이 스토어, 원스토어 (https://www.tstore.co.kr), HIVE 러비(Lebi) 을 제공한다.<br/>
	 * HIVE 러비(Lebi) 상점은 중국 시장에서 상점을 구성하고 상품을 구매 할 수 있는 시스템으로서 HIVE 에서 자체적으로 운영하고 있다.<br/><br/>
	 * \~english HIVE IAP can minimize the development costs of the market-specific purchase process and collect purchase logs consistently to provide information to business, development, and management departments. <br/> * Currently, HIVE IAP supports Apple App Store, Google Play Store, One Store (https://www.tstore.co.kr) and HIVE Lobby (Lebi).<br/>
	 * HIVE LIVE (Lebi) store is a system that can organize stores and purchase goods in Chinese market. It is operated by HIVE itself.<br/><br/>
	 * \~
	 * @author		ryuvsken, hife
	 * @since		4.0.0
	 * @ingroup IAP
	 */
	public class IAP {

		/**
		 * \~korean  HIVE IAP 에서 상점 및 상품의 구성을 위한 정보 조회 결과 통지
		 *  
		 * @param result		API 호출 결과
		 * @param iapTypeList	사용 가능한 마켓 정보의 목록<br/>이 마켓 정보의 목록은 게임 클라이언트에서 사용 가능한 마켓을 의미한다.<br/>마켓 목록이 2개 이상이라면 유저가 상점을 선택할 수 있도록 게임에서 유저에게 노출할 수 있는 화면을 제공해야 한다
		 * \~english Result of information inquiry for HIVE IAP to organize stores and products
		 *  
		 * @param result		API call result
		 * @param iapTypeList	List of available market information <br/> This list of market information means the market available to the game client. <br/>If there is more than one market list, you should provide a screen that allows the user to select the store.
		 * \~
		 * @ingroup IAP
		 */
		public delegate void onIAPMarketInfo(ResultAPI result, List<IAPType> iapTypeList);

		/**
		 * HIVE IAP 백오피스에서 구성한 상점 정보 조회 결과 통지
		 * 
		 * @param result		API 호출 결과
		 * @param iapShop		상점 정보
		 * @param balance		HIVE Lebi 상점의 경우 잔액 정보
		 * \~english Result of shop information inquiry configured in HIVE IAP back office
		 * 
		 * @param result		API call result
		 * @param iapShop		Shop information 
		 * @param balance		Balance information for HIVE Lebi stores
		 * \~
		 * @ingroup IAP
		 */
		public delegate void onIAPShopInfo(ResultAPI result, IAPShop iapShop, int balance);

		/**
		 * \~korean  상품 결제 시도 결과 통지<br/>
		 * 상품은 App Store, Google Store 에 등록된 마켓 상품 (marketPid) 과 HIVE IAP 백오피스에 등록된 HIVE IAP 상품 (gamePid) 으로 구분된다.<br/>
		 * HIVE IAP 의 상품 구매는 HIVE IAP 백오피스에 등록된 상품 (gamePid) 을 요청하게 된다<br/>
		 * (Note: 결재 시도전에 restore() 를 호출하여 미지급된 아이템 지급을 요청해야 한다)
		 *
		 * \~english Result of IAP purchase<br/>
		 * Products ID are classified into two, one is 'marketPid' registered in Apple AppStore or Google PlayStore, the other is gamePid registered in HIVE IAP back office.<br/>
		 * The purchase of the HIVE IAP will require the gamePid registered in the HIVE IAP back office.
		 * (Note: You must call restore() before attempting a new purchase so that you can deliver an item that has not been delivered)
		 * \~
		 * @ingroup IAP
		 * @author imsunghoon
		 */
		public delegate void onIAPPurchaseReceipt(ResultAPI result, IAPReceipt iapReceipt);
	 
		/**
		 * \~korean  HIVE IAP 상품에 대한 미지급된 아이템 지급을 요청 결과 통지<br/>
		 * 상품 구매 과정에서 여러가지 상황으로 인하여 아이템 지급을 실패하는 경우가 발생할 수 있다.<br/>
		 * 이런 상황을 대비하기 위해서 미 지급된 아이템 지급을 요청 해야 한다.
		 * @param result				API call result
		 * @param receiptList 			구매 성공한 상품 정보 리스트
		 *
		 * \~english Result of Restore HIVE IAP product which is not sent to user<br/>
		 * In the process of purchasing a product, it may happen that the item fails to be delivered due to various situations.<br/>
		 * In order to prepare for this situation, you must restore for the item.
		 * 
		 * @param result				API call result
		 * @param receiptList 			구매 성공한 상품 정보 리스트
		 * \~
		 * @ingroup IAP
		 * @author imsunghoon
		 */
		public delegate void onIAPRestoreReceipt(ResultAPI result, List<IAPReceipt> receiptList);

		/**
		 * \~korean  HIVE 러비 상점일 경우 잔액 확인 결과 통지
		 * 
		 * @param result		API 호출 결과
		 * @param balance		HIVE 러비 상점일 경우 잔액
		 * \~english Result of balance request, if it is HIVE Lebi store
		 * 
		 * @param result		API call result
		 * @param balance		Balance of HIVE Lebi store
		 * \~
		 * @ingroup IAP
		 */
		public delegate void onIAPBalanceInfo(ResultAPI result, int balance);

		/**
		 * \~korean  iTunes에서 Promotional In-App-Purchase를 통한 앱구매 예약에 대한 통지
		 * 
		 * @param result		API 호출 결과
		 * @param gamePid		구매 예약한 상점아이템의 gamePid
		 * \~english iTunes에서 Promotional In-App-Purchase를 통한 앱구매 예약에 대한 통지
		 * 
		 * @param result		API call result
		 * @param gamePid		구매 예약한 상점아이템의 gamePid
		 * \~
		 * @ingroup IAP
		 */
		public delegate void onIAPCheckPromotePurchase(ResultAPI result, String gamePid);

		/**
		 * \~korean  IAP 결재 API 초기화 요청<br/>
		 * HIVE IAP 의 초기화를 수행하면 결과 통지시 사용 가능한 마켓 정보의 목록을 전달하게 된다.<br/>
		 * 이 마켓 정보의 목록은 게임 클라이언트에서 사용 가능한 마켓을 의미한다.
		 * 
		 * @param listener	API 결과 통지
		 * \~english Initialize HIVE IAP<br/>
		 * HIVE IAP initialization will deliver a list of available market information in the result.<br/>
		 * This list of market information means the market available on the game client.
		 * 
		 * @param listener	API call result listener
		 * \~
		 * @ingroup IAP
		 */
		public static void initialize(onIAPMarketInfo listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("IAP", "initialize", listener);

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
		 * \~korean  HIVE IAP 에서 상점 및 상품의 구성을 위한 정보를 조회할 수 있다<br/>
		 * 상점은 판매 전략에 따라서 일반 상점, 스팟 (Spot) 상점, 이벤트 (Event) 상점을 노출 할 수 있다.<br/>
		 * 스팟 상점과 이벤트 상점은 HIVE 백오피스에 Location Code 를 설정하고, 게임의 노출 위치에 Location Code 를 삽입한다
		 * 
		 * @param locationCode		백오피스에 설정된 상점 위치 코드</br/>
		 * ex. “DEFAULT,”“MAIN”, “EVENT”, “SPOT”. DEFAULT 상점의 경우 백오피스 내 "상품등록", "시리즈 상품등록"의 모든 상품 목록을 반환한다.
		 * @param listener			API 결과 통지
		 * \~english Return information for the configuration of shops and products<br/>
		 * Stores can be exposed general shops, spot shops and event shops according to their sales strategy.<br/>
		 * Spot shops and event shops set the location code in the HIVE back office and insert the location code at the exposure location of the game.
		 * 
		 * @param locationCode		Store location code set in back office
		 * @param listener			API call result listener
		 * \~
		 * @ingroup IAP
		 */
		public static void getShopInfo(String locationCode, onIAPShopInfo listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("IAP", "getShopInfo", listener);
			jsonParam.AddField ("locationCode", locationCode);

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
		 * \~korean  상품은 App Store, Google Store 에 등록된 마켓 상품 (marketPid) 과 HIVE IAP 백오피스에 등록된 HIVE IAP 상품 (gamePid) 으로 구분된다.<br/>
		 * HIVE IAP 의 상품 구매는 HIVE IAP 백오피스에 등록된 상품 (gamePid) 을 요청하게 된다.<br/>
		 * additionalInfo 의 경우 구매 요청에 대한 보충 정보로 구매 완료 콜백에 다시 포함되어 전송되지만<br/>
		 * 프로모 코드 구매 처럼 앱 외부에서 구매 등의 경우에는 포함될 수 없게 된다.
		 *
		 * @param gamePid			HIVE IAP 시스템에서 관리하는 하나의 상품을 지칭하는 고유 코드
		 * @param additionalInfo	구매 요청에 대한 보충 정보를 포함한 문자열로, 개발자가 지정합니다.
		 * @param listener			API 결과 통지
		 * \~english Purchase Item 
		 * Products ID are classified into two, one is 'marketPid' registered in Apple AppStore or Google PlayStore, the other is 'gamePid' registered in HIVE IAP back office.<br/>
		 * The purchase of the HIVE IAP will require the 'gamePid' registered in the HIVE IAP back office
		 * (Note: You must call restore() before attempting a new purchase so that you can deliver an item that has not been delivered) 
		 *
		 * @param gamePid			Product specific code managed by HIVE IAP system.
		 * @param additionalInfo	구매 요청에 대한 보충 정보를 포함한 문자열로, 개발자가 지정합니다.
		 * @param listener			API call result listener
		 * \~
		 * @ingroup IAP
		 */
		public static void purchase(String gamePid, String additionalInfo, onIAPPurchaseReceipt listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("IAP", "purchaseReceipt", listener);
			jsonParam.AddField ("pid", gamePid);
			jsonParam.AddField ("additionalInfo", additionalInfo);

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
		 * \~korean  HIVE IAP 상품에 대한 미지급된 아이템 지급을 요청한다.<br/>
		 * 상품 구매 과정에서 여러가지 상황으로 인하여 아이템 지급을 실패하는 경우가 발생할 수 있다.<br/>
		 * 이런 상황을 대비하기 위해서 미 지급된 아이템 지급을 요청 해야 한다.<br/>
		 * restore(IAPRestoreListener) API 와 다른점은 추가된 IAPReceipt 로 전달 되며 이 경우 additionalInfo 값도 포함되어 있다.<br/>
		 *
		 * @param listener			API 결과 통지
		 * \~english Restore HIVE IAP product which is not sent to user.<br/>
		 * In the process of purchasing a product, it may happen that the item fails to be delivered due to various situations.<br/>
		 * In order to prepare for this situation, you must restore for the item.
		 * 
		 * @param listener			API call result listener
		 * \~
		 * @ingroup IAP
		 */
		public static void restore(onIAPRestoreReceipt listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("IAP", "restoreReceipt", listener);

			HIVEUnityPlugin.callNative (jsonParam);
		}

		/**
		 * \~korean  러비 상점이나 구글 상점을 선택하기 위한 창을 띄운다. (Android only.)<br/>
		 * 만약 Google Store 와 HIVE 러비(Lebi) 상점을 동시에 사용 가능한 상황이라면 유저가 상점 선택을 할 수 있도록 상점 선택 화면을 노출해야 한다
		 * 
		 * @param listener			API 결과 통지
		 * \~english Launches a window to select a lebi store or a Google Play Store. (Android only.)<br/>
		 * If the Google Play Store and the HIVE LIVE store are available at the same time, you should expose the store selection screen so that the user can select a store.
		 * 
		 * @param listener			API call result listener
		 * \~
		 * @ingroup IAP
		 */
		public static void showMarketSelection(onIAPMarketInfo listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("IAP", "showMarketSelection", listener);

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
		 * \~korean  러비 상점일 경우 잔액 정보 조회. (Android only.)
		 * 
		 * @param listener			API 결과 통지
		 * \~english Request your balance information for HIVE Lebi store. (Android only.)
		 * 
		 * @param listener			API call result listener
		 * \~
		 * @ingroup IAP
		 */
		public static void getBalanceInfo(onIAPBalanceInfo listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("IAP", "getBalanceInfo", listener);

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
		 * \~korean  러비 충전 페이지 노출 (Android only.)
		 * 
		 * @param listener			API 결과 통지
		 * \~english Show HIVE Lebi Store redeem page(Android only.)
		 * 
		 * @param listener			API call result listener
		 * \~
		 * @ingroup IAP
		 */
		public static void showCharge(onIAPBalanceInfo listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("IAP", "showCharge", listener);

			HIVEUnityPlugin.callNative (jsonParam);
		}

		/**
		 * \~korean  iTunes에서 Promotional In-App-Purchase를 통한 앱구매 예약에 대한 통지 등록
		 * 
		 * @param listener			API 결과 통지
		 * \~english iTunes에서 Promotional In-App-Purchase를 통한 앱구매 예약에 대한 통지 등록
		 * 
		 * @param listener			API call result listener
		 * \~
		 * @ingroup IAP
		 */
		public static void checkPromotePurchase(onIAPCheckPromotePurchase listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("IAP", "checkPromotePurchase", listener);

			HIVEUnityPlugin.callNative (jsonParam);	
		}

		public static void executeEngine(JSONObject resJsonObject) {

			String methodName = null;
			resJsonObject.GetField (ref methodName, "method");

			int handlerId = -1;
			resJsonObject.GetField (ref handlerId, "handler");
			object handler = (object)HIVEUnityPlugin.popHandler (handlerId);

			if (handler == null) return;

			if ("initialize".Equals (methodName)) {

				List<IAPType> iapTypeList = new List<IAPType> ();

				JSONObject jsonArray = resJsonObject.GetField ("iapTypeList");
				if (jsonArray != null && jsonArray.count > 0) {

					List<JSONObject> jsonList = jsonArray.list;
					foreach(JSONObject jsonItem in jsonList) {

						String iapTypeName = jsonItem.stringValue;
						if ("APPLE_APPSTORE".Equals (iapTypeName)) {
							iapTypeList.Add (IAPType.APPLE_APPSTORE);
						}
						else if ("GOOGLE_PLAYSTORE".Equals (iapTypeName)) {
							iapTypeList.Add (IAPType.GOOGLE_PLAYSTORE);
						}
						else if ("HIVE_LEBI".Equals (iapTypeName)) {
							iapTypeList.Add (IAPType.HIVE_LEBI);
						}
						else if ("ONESTORE".Equals (iapTypeName)) {
							iapTypeList.Add (IAPType.ONESTORE);
						}
					}
				}

				onIAPMarketInfo listener = (onIAPMarketInfo)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")), iapTypeList);
			}
			else if ("getShopInfo".Equals (methodName)) {

				IAPShop iapShop = new IAPShop(resJsonObject.GetField ("iapShop"));

				int balance = 0;
				resJsonObject.GetField (ref balance, "balance");

				onIAPShopInfo listener = (onIAPShopInfo)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")), iapShop, balance);
			}
			else if ("purchaseReceipt".Equals (methodName)) {
				
				IAPReceipt iapReceipt = new IAPReceipt(resJsonObject.GetField ("iapReceipt"));

				onIAPPurchaseReceipt listener = (onIAPPurchaseReceipt)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")), iapReceipt);
			}
			else if ("restoreReceipt".Equals (methodName)) {

				List<IAPReceipt> receiptList = new List<IAPReceipt> ();

				JSONObject jsonArray = resJsonObject.GetField ("receiptList");
				if (jsonArray != null && jsonArray.count > 0) {
					List<JSONObject> jsonList = jsonArray.list;
					foreach(JSONObject jsonItem in jsonList) {
						IAPReceipt iapReceipt = new IAPReceipt(jsonItem);

						receiptList.Add(iapReceipt);
					}
				}

				onIAPRestoreReceipt listener = (onIAPRestoreReceipt)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")), receiptList);
			}
			else if ("showMarketSelection".Equals (methodName)) {

				List<IAPType> iapTypeList = new List<IAPType> ();

				JSONObject jsonArray = resJsonObject.GetField ("iapTypeList");
				if (jsonArray != null && jsonArray.count > 0) {

					List<JSONObject> jsonList = jsonArray.list;
					foreach(JSONObject jsonItem in jsonList) {

						String iapTypeName = jsonItem.stringValue;
						if ("APPLE_APPSTORE".Equals (iapTypeName)) {
							iapTypeList.Add (IAPType.APPLE_APPSTORE);
						}
						else if ("GOOGLE_PLAYSTORE".Equals (iapTypeName)) {
							iapTypeList.Add (IAPType.GOOGLE_PLAYSTORE);
						}
						else if ("HIVE_LEBI".Equals (iapTypeName)) {
							iapTypeList.Add (IAPType.HIVE_LEBI);
						}
						else if ("ONESTORE".Equals (iapTypeName)) {
							iapTypeList.Add (IAPType.ONESTORE);
						}
					}
				}

				onIAPMarketInfo listener = (onIAPMarketInfo)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")), iapTypeList);
			}
			else if ("getBalanceInfo".Equals (methodName)) {

				int balance = 0;
				resJsonObject.GetField (ref balance, "balance");

				onIAPBalanceInfo listener = (onIAPBalanceInfo)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")), balance);
			}
			else if ("showCharge".Equals (methodName)) {

				int balance = 0;
				resJsonObject.GetField (ref balance, "balance");

				onIAPBalanceInfo listener = (onIAPBalanceInfo)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")), balance);
			}
			else if ("checkPromotePurchase".Equals (methodName)) {

				String gamePid = "";
				resJsonObject.GetField (ref gamePid, "gamePid");

				onIAPCheckPromotePurchase listener = (onIAPCheckPromotePurchase)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")), gamePid);
			}	
		}
	}




	/**
	 * \~korean  결제 마켓 형태
	 * 
	 * \~english Market type
	 * \~
	 * @ingroup IAP
	 * @ingroup hive
	 */
	public enum IAPType {

		APPLE_APPSTORE = 1,			///< Apple App Store
		GOOGLE_PLAYSTORE,			///< Google Play Store
		HIVE_LEBI,					///< HIVE Lebi
		ONESTORE					///< OneStore
	}




	/**
	 * \~korean  HIVE IAP 백오피스에서 구성한 상점 정보
	 * 
	 * \~english Store information configured in the HIVE IAP back office
	 * \~
	 * @ingroup IAP
	 * @author hife
	 */
	public class IAPShop {

		public String locationCode;				///< \~korean 백오피스에 설정된 상점 위치 코드 \~english Shop location code set in back office.
		public int shopId;
		public String shopType;					///< \~korean 백오피스에 설정된 상점 구분 코드 ("MAIN", "EVENT", "SPOT") \~english Type of shop set in back office ("MAIN", "EVENT", "SPOT")
		public String originalJson;				
		public List<IAPProduct> productList;	///< \~korean 상품 정보 목록 \~english Product Information List

		public IAPShop() {
		}

		public IAPShop(String locationCode, int shopId, String shopType, String originalJson, List<IAPProduct> productList) {
			this.locationCode = locationCode;
			this.shopId = shopId;
			this.shopType = shopType;
			this.originalJson = originalJson;
			this.productList = productList;
		}

		public IAPShop(JSONObject resJsonParam) {

			if(resJsonParam == null || resJsonParam.count <= 0)
				return;

			resJsonParam.GetField (ref this.locationCode, "locationCode");
			resJsonParam.GetField (ref this.shopId, "shopId");
			resJsonParam.GetField (ref this.shopType, "shopType");
			resJsonParam.GetField (ref this.originalJson, "originalJson");

			List<IAPProduct> productList = new List<IAPProduct> ();

			JSONObject jsonArray = resJsonParam.GetField ("productList");
			if (jsonArray != null && jsonArray.count > 0) {

				List<JSONObject> jsonList = jsonArray.list;
				foreach(JSONObject jsonItem in jsonList) {
					IAPProduct iapProduct = new IAPProduct(jsonItem);
					productList.Add (iapProduct);
				}
			}
			this.productList = productList;
		}

		public String toString() {

			StringBuilder sb = new StringBuilder();

			sb.Append("IAPShop {\nlocationCode = ");
			sb.Append(this.locationCode);
			sb.Append(", shopId = ");
			sb.Append(this.shopId);
			sb.Append(", shopType = ");
			sb.Append(this.shopType);
			sb.Append(", originalJson = ");
			sb.Append(this.originalJson);
			sb.Append (" }\n");

			return sb.ToString();
		}
	}


	/**
	 * \~korean  HIVE IAP 백오피스에서 생성한 상품 정보<br/>
	 * 마켓에 등록된 상품(Market Pid)과 HIVE IAP 백오피스에 등록한 상품 (Game Pid) 의 정보를 포함한다. 
	 * 
	 * \~english Product information created by HIVE IAP back office<br/>
	 * It include (Market Pid) registered in the market(AppStore, PlayStore) and (Game Pid) registered in the HIVE IAP back office. 
	 * \~
	 * @ingroup IAP
	 * @author hife
	 */
	public class IAPProduct {

		public String gamePid;					///< \~korean HIVE IAP 시스템에서 관리하는 하나의 상품을 지칭하는 고유 코드 \~english Unique code to refer to one product managed by the HIVE IAP system.
		public String marketPid;				///< \~korean 마켓에 등록된 고유 상품 ID		 \~english Unique product ID registered in Market(AppStore, PlayStore)		
		public String currency;					///< \~korean 통화 코드 (ex, KRW / USD)		 \~english Currency code (ex, KRW / USD)		
		public double price;					///< \~korean 상품 가격		 \~english Price		
		public String displayPrice;				///< \~korean 상품 가격 문자열 (ex. $100.00)		 \~english Price string (ex. $100.00)		
		public String originalJson;


		public IAPProduct() {
		}

		public IAPProduct(String gamePid, String marketPid, String currency, double price, String displayPrice
			, String originalJson) {

			this.gamePid = gamePid;
			this.marketPid = marketPid;
			this.currency = currency;
			this.price = price;
			this.displayPrice = displayPrice;
			this.originalJson = originalJson;
		}

		public IAPProduct(JSONObject resJsonParam) {

			if(resJsonParam == null || resJsonParam.count <= 0)
				return;

			resJsonParam.GetField (ref this.gamePid, "gamePid");
			resJsonParam.GetField (ref this.marketPid, "marketPid");
			resJsonParam.GetField (ref this.currency, "currency");
			resJsonParam.GetField (ref this.price, "price");
			resJsonParam.GetField (ref this.displayPrice, "displayPrice");
			resJsonParam.GetField (ref this.originalJson, "originalJson");
		}

		public String toString() {

			StringBuilder sb = new StringBuilder();

			sb.Append("IAPProduct { gamePid = ");
			sb.Append(gamePid);
			sb.Append(", marketPid = ");
			sb.Append(marketPid);
			sb.Append(", currency = ");
			sb.Append(currency);
			sb.Append(", price = ");
			sb.Append(price);
			sb.Append(", displayPrice = ");
			sb.Append(displayPrice);
			sb.Append(", originalJson = ");
			sb.Append(originalJson);
			sb.Append (" }\n");

			return sb.ToString();
		}
	}

	/**
	 * \~korean purchase() 나 restoreReceipt() API 에서 받을 수 있는 결과 정보.<br/>
	 * iapTransactionId 는 HIVE IAP 서버에 영수증이 정상적으로 전달이 되면 값이 있으며<br/>
	 * 게임서버에까지 지급 내역이 전달되지 못하여 실제 게임 클라이언트에 지급이 안되었더라도 HIVEITEM 에서 재지급 하게 된다.
	 *
	 * \~english purchase() 나 restoreReceipt() API 에서 받을 수 있는 결과 정보.<br/>
	 * iapTransactionId 는 HIVE IAP 서버에 영수증이 정상적으로 전달이 되면 값이 있으며<br/>
	 * 게임서버에까지 지급 내역이 전달되지 못하여 실제 게임 클라이언트에 지급이 안되었더라도 HIVEITEM 에서 재지급 하게 된다.
	 * \~
	 * @ingroup IAP
	 */
	public class IAPReceipt {

		public IAPProduct product;			///< 구매 성공한 상품 정보
		public String iapTransactionId;		///< HIVE IAP 서버의 트랜젝션 ID
		public String additionalInfo;		///< 구매 요청시 추가로 입력받은 additionalInfo
		public String originalJson;

		public IAPReceipt() {}

		public IAPReceipt(IAPProduct product, String iapTransactionId, String additionalInfo, String originalJson) {

			this.product = product;
			this.iapTransactionId = iapTransactionId;
			this.additionalInfo = additionalInfo;
			this.originalJson = originalJson;
		}

		public IAPReceipt(JSONObject resJsonParam) {

			if(resJsonParam == null || resJsonParam.count <= 0)
				return;
			
			JSONObject productJson = resJsonParam.GetField("product");
			this.product = new IAPProduct(productJson);

			resJsonParam.GetField (ref this.iapTransactionId, "iapTransactionId");
			resJsonParam.GetField (ref this.additionalInfo, "additionalInfo");
			// resJsonParam.GetField (ref this.originalJson, "originalJson");
		}

		public String toString() {

			StringBuilder sb = new StringBuilder();

			sb.Append("IAPReceipt { product = ");
			sb.Append(product.toString());
			sb.Append(", iapTransactionId = ");
			sb.Append(iapTransactionId);
			sb.Append(", additionalInfo = ");
			sb.Append(additionalInfo);
			// sb.Append(", originalJson = ");
			// sb.Append(originalJson);
			sb.Append (" }\n");

			return sb.ToString();
		}
	}
}


/** @} */



