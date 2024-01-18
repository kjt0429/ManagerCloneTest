/**
 * @file            IAPV4.h
 * 
 * @date            2016-2023
 * @copyright       Copyright (C) 2023 Com2uS Platform Corporation. All Right Reserved.
 * @author          hife
 * @since           4.4.0
 * 
 */
using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;

/**
 * @defgroup IAPV4
 * @{
 */

namespace hive
{
    /**
     * \~korean
     * 현재 Hive IAP 가 제공하는 결제 마켓은 애플 앱스토어, 구글 플레이스토어, 원스토어, 삼성 갤럭시스토어, 아마존 앱스토어, 
     * 페이스북 클라우드 게임, 화웨이 앱커넥트, PG, Steam, Hive 러비(Lebi) 를 제공한다.<br/>
     * Hive 러비(Lebi) 상점은 중국 시장에서 상점을 구성하고 상품을 구매 할 수 있는 시스템으로서 Hive 에서 자체적으로 운영하고 있다.<br/><br/>
     * \~english
     * Currently, Hive IAP supports Apple AppStore, Google PlayStore, ONEStore, Samsung GalaxyStore,
     * Amazon AppStore, Facebook Cloud Games, Huawei AppConnect, PG, Steam and Hive Lebi.<br/>
     * Hive Lebi store is a system that can organize stores and purchase goods in Chinese market.
     * It is operated by Hive itself.<br/><br/>
     * \~
     * @since        4.4.0
     * @ingroup IAPV4
     * @author imsunghoon
     */
    public class IAPV4 {

        /**
         *  \~korean
         * @brief HIVE IAPV4 에서 마켓의 구성을 위한 정보 조회 결과 통지
         *
         * @param result        API 호출 결과
         * @param iapV4TypeList    사용 가능한 마켓 정보의 목록<br/>이 마켓 정보의 목록은 게임 클라이언트에서 사용 가능한 마켓을 의미한다.<br/>마켓 목록이 2개 이상이라면 유저가 상점을 선택할 수 있도록 게임에서 유저에게 노출할 수 있는 화면을 제공해야 한다
         *
         *  \~english
         * @brief HIVE IAPV4 Market information request result callback
         *
         * @param result        Result of API call
         * @param iapV4TypeList    사용 가능한 마켓 정보의 목록<br/>
         * This list of market information means the market available on the game client.<br/>
         * If there is more than one market list, you must provide a screen that display the available market list to the user so that the user can select a store. <br/>
         *
         *  \~
         * @see #marketConnect(onIAPV4MarketInfo)
         *
         * @ingroup IAPV4
         */
        public delegate void onIAPV4MarketInfo(ResultAPI result, List<IAPV4Type> iapV4TypeList);

        /**
         *  \~korean
         * HIVE IAPV4 상품 정보 조회 결과 통지
         *
         * @param result            API 호출 결과
         * @param iapV4ProductList  상품 정보
         * @param balance            HIVE Lebi 마켓의 경우 잔액 정보
         *
         *  \~english
         * HIVE IAPV4 Product Information request callback
         *
         * @param result            Result of API call
         * @param iapV4ProductList  Product Information
         * @param balance            Balance information for HIVE Lebi Market If Lebi Market enabled.
         *
         *  \~
         * @see getProductInfo(onIAPV4ProductInfo)
         *
         * @ingroup IAPV4
         */
        public delegate void onIAPV4ProductInfo(ResultAPI result, List<IAPV4Product> iapV4ProductList, int balance);

        /**
        *  \~korean
        * @brief 상품 결제 시도 결과 통지<br/>
        * App Store, Google Store 등 에 등록된 마켓 상품(marketPid)의 구매를 요청 한다.<br/>
        * iapPayload 의 경우 구매 요청에 대한 보충 정보로 구매 후 영수증 검증에서 확인할 수 있다.<br/>
        * 프로모 코드 구매 처럼 앱 외부에서 구매 등의 경우에는 포함될 수 없게 된다.
        *
        * @param result            API 호출 결과
        * @param iapV4Receipt      구매 성공한 상품 정보 와 영수증
        *
        *  \~english
        * @brief Purchase request result callback<br/>
        * Request purchase of market product (marketPid) registered in App Store, Google Store, etc.<br/>
        * iapPayload is supplementary information for the purchase request, and it can be checked in Receipt Verification after purchase, <br/>
        * but, in case of purchasing outside of the app, such as a promo code purchase etc., it can not be included in verification result.
        *
        * @param result            Result of API call
        * @param iapV4Receipt      It includes the purchased product information and receipt.
        *
        *  \~
        * @see #purchase(String, String, IAPV4PurchaseListener)
        * @ingroup IAPV4
        */
        public delegate void onIAPV4Purchase(ResultAPI result, IAPV4Receipt iapV4Receipt);

        /**
         *  \~korean
         * @brief HIVE IAPV4 상품에 대한 미지급된 아이템 지급을 요청 결과 통지<br/>
         * 상품 구매 과정에서 여러가지 상황으로 인하여 아이템 지급을 실패하는 경우가 발생할 수 있다.<br/>
         * 이런 상황을 대비하기 위해서 미 지급된 아이템 지급을 요청 해야 한다.
         *
         * @param result            API 호출 결과
         * @param iapv4ReceiptList  구매 성공한 상품 정보 와 iapPayload 를 포함한 정보
         *
         *  \~english
         * @brief HIVE IAPV4 Restore request result callback<br/>
         * It may happen that the item delivery fails due to various situations in the product purchase process.<br/>
         * In order to prepare for this situation, you should request to deliver the item that has not been delivered.
         *
         * @param result            Result of API call
         * @param iapv4ReceiptList  It includes the purchased product information and iapPayload
         *
         *  \~
         * @see #restore(onIAPV4Restore)
         *
         * @ingroup IAPV4
         */
        public delegate void onIAPV4Restore(ResultAPI result, List<IAPV4Receipt> iapv4ReceiptList);

        /**
         *  \~korean
         * @brief HIVE IAP v4에서 상품에 대한 지급 완료 후 결제 트랜잭션을 종료하기 위한 요청 결과 통지<br/>
         *
         * @param result        API 호출 결과
         * @param marketPid    트랜잭션이 종료된 상품의 marketPid
         *
         *  \~english
         * @brief HIVE IAP v4 Transaction Finish request result callback<br/>
         *
         * @param result        Result of API call
         * @param marketPid     marketPid of the product whose transaction ended
         *  
         *  \~
         * @see #transactionFinish(String, onIAPV4TransactionFinish)
         *
         * @ingroup IAPV4
         */
        public delegate void onIAPV4TransactionFinish(ResultAPI result, String marketPid);

        /**
         *  \~korean
         * @brief HIVE IAP v4에서 상품에 대한 지급 완료 후 복수 개의 결제 트랜잭션을 종료하기 위한 요청 결과 통지<br/>
         *
         * @param resultList        트랜잭션의 종료 결과 목록
         * @param marketPidList        트랜잭션이 종료된 상품의 marketPid 목록
         *
         *  \~english
         * @brief HIVE IAP v4 Multiple Transaction Finish request result callback<br/>
         *
         * @param resultList        Result list of the transaction end
         * @param marketPidList        marketPid list of the transaction end
         *
         *  \~
         * @see #transactionMultiFinish(List, onIAPV4TransactionMultiFinish)
         */
        public delegate void onIAPV4TransactionMultiFinish(List<ResultAPI> resultList, List<String> marketPidList);

#if !UNITY_EDITOR && UNITY_ANDROID

        /**
         *  \~korean
         * @brief HIVE 러비 상점일 경우 잔액 확인 결과 통지
         *
         * @param result        API 호출 결과
         * @param balance        HIVE 러비 상점일 경우 잔액
         *
         *  \~english
         * @brief HIVE Lebi Balance request result callback, if it is a Rebi store.
         *
         * @param result        Result of API call
         * @param balance        HIVE Lebi balance
         *
         *  \~
         * @ingroup IAPV4
         */
        public delegate void onIAPV4Balance(ResultAPI result, int balance);

#endif

        public delegate void onIAPV4CheckPromotePurchase(ResultAPI result, String marketPid);


        /**
         * Google In-App Offers
         *
         * @param responseCode 2(NAVIGATE_TO_DEV_APP_STORE) 일 경우 유저가 제안(Offer)를 확인하고 사용하기를 원한 상황입니다.
         * 개발자는 유저가 구매할 인앱 항목을 선택할 수 있는 게임/앱 내 상점으로 (유저가 해당 화면에 없는 경우) 유저를 이동해야 합니다.
         * 0 은 아무런 액션을 하지 않아도 되고, 음수는 에러상황 입니다.
         */
        public delegate void onIAPV4ShowInAppMessages(int responseCode);


        /**
        *  \~korean
        * @brief IAPV4 결재 API 초기화 요청<br/>
        * HIVE IAPV4 의 초기화를 수행하면 결과 통지시 사용 가능한 마켓 정보의 목록을 전달하게 된다.<br/>
        * 이 마켓 정보의 목록은 게임 클라이언트에서 사용 가능한 마켓을 의미한다.
        *
        * @param listener    API 결과 통지
        *
        *  \~english
        * @brief IAPV4 It initialize IAPV4 Purchase module.<br/>
        * When HIVE IAPV4 is initialized, it sends a list of available market information in the result callback.<br/>
        * This list of market information means the market available on the game client.
        *
        * @param listener    Result of API call
        *
        *  \~
        * @ingroup IAPV4
        */
        public static void marketConnect(onIAPV4MarketInfo listener) {

            JSONObject jsonParam = HIVEUnityPlugin.createParam("IAPV4", "marketConnect", listener);

            HIVEUnityPlugin.callNative (jsonParam);    
        }

        /**
        *  \~korean
        * @brief Hive IAPV4 에서 마켓 상품의 구성을 위한 정보를 조회할 수 있다. <br/>
        *
        * @param listener            API 결과 통지
        * 
        *  \~english
        * @brief Hive IAPV4 It returns information for composition of the market product.<br/>
        *
        * @param listener            Result of API call
        * 
        *  \~
        * @ingroup IAPV4
        */
        public static void getMarketProductInfo(List<String> marketPidList, onIAPV4ProductInfo listener) {

            JSONObject jsonParam = HIVEUnityPlugin.createParam("IAPV4", "getMarketProductInfo", listener);

            JSONObject jsonArray = new JSONObject();

            if(marketPidList != null) {
                foreach(String marketPid in marketPidList) {

                    jsonArray.Add(marketPid.ToString());
                }
            }

            jsonParam.AddField ("marketPidList", jsonArray);

            HIVEUnityPlugin.callNative (jsonParam);    
        }

        /**
        *  \~korean
        * @brief HIVE IAPV4 에서 상품의 구성을 위한 정보를 조회할 수 있다. <br/>
        *
        * @param listener            API 결과 통지
        * 
        *  \~english
        * @brief HIVE IAPV4 It returns information for composition of the product.<br/>
        *
        * @param listener            Result of API call
        * 
        *  \~
        * @ingroup IAPV4
        */
        public static void getProductInfo(onIAPV4ProductInfo listener) {

            JSONObject jsonParam = HIVEUnityPlugin.createParam("IAPV4", "getProductInfo", listener);

            HIVEUnityPlugin.callNative (jsonParam);    
        }


        /**
        *  \~korean
        * @brief HIVE IAPV4 에서 구독형 상품의 구성을 위한 정보를 조회할 수 있다. <br/>
        *
        * @param listener            API 결과 통지
        * 
        *  \~english
        * @brief HIVE IAPV4 It returns information for composition of the product.<br/>
        *
        * @param listener            Result of API call
        * 
        *  \~
        * @ingroup IAPV4
        * @since 4.11.6
        */
        public static void getSubscriptionProductInfo(onIAPV4ProductInfo listener) {

            JSONObject jsonParam = HIVEUnityPlugin.createParam("IAPV4", "getSubscriptionProductInfo", listener);

            HIVEUnityPlugin.callNative (jsonParam);    
        }

        /**
        *  \~korean
        * @brief App Store, Google Store 등 에 등록된 마켓 상품(marketPid)의 구매를 요청 한다.<br/>
        * iapPayload 의 경우 구매 요청에 대한 보충 정보로 구매 후 영수증 검증에서 확인할 수 있다.<br/>
        * 프로모 코드 구매 처럼 앱 외부에서 구매 등의 경우에는 포함될 수 없게 된다.
        *
        * @param marketPid         App Store, Google Store 등 에 등록된 하나의 상품을 지칭하는 고유 코드
        * @param iapPayload        구매 요청에 대한 보충 정보를 포함한 문자열로, 개발자가 지정합니다.
        * @param listener          API 결과 통지
        *
        *  \~english
        * @brief Request purchase of market product (marketPid) registered in App Store, Google Store, etc.<br/>
        * iapPayload is supplementary information for the purchase request, and it can be checked in Receipt Verification after purchase, <br/>
        * but, in case of purchasing outside of the app, such as a promo code purchase etc., it can not be included in verification result.
        *
        * @param marketPid         Unique code registered on the App Store, Google Store or etc.
        * @param iapPayload        A string containing supplementary information about the purchase request, specified by the developer.
        * @param listener          Result of API call
        *
        *  \~
        * @ingroup IAPV4
        */
        public static void purchase(String marketPid, String iapPayload, onIAPV4Purchase listener) {

            JSONObject jsonParam = HIVEUnityPlugin.createParam("IAPV4", "purchase", listener);
            jsonParam.AddField ("marketPid", marketPid);
            jsonParam.AddField ("iapPayload", iapPayload);

            HIVEUnityPlugin.callNative (jsonParam);    
        }
        
        /**
        *  \~korean
        * @brief App Store, Google Store 등 에 등록된 마켓 상품(marketPid)의 구매를 요청 한다.<br/>
        * iapPayload 의 경우 구매 요청에 대한 보충 정보로 구매 후 영수증 검증에서 확인할 수 있다.<br/>
        * 프로모 코드 구매 처럼 앱 외부에서 구매 등의 경우에는 포함될 수 없게 된다.
        * 구독형 상품을 업그레이드하거나 다운그레이드를 수행하는 경우 기존에 구독 중인 상품의 고유코드 oldMarketPid를 필수로 넣어야 한다.
        *
        * @param marketPid         App Store, Google Store 에 등록된 하나의 상품을 지칭하는 고유 코드
        * @param oldMarketPid      기존에 구독 중인 상품의 고유코드
        * @param iapPayload        구매 요청에 대한 보충 정보를 포함한 문자열로, 개발자가 지정합니다.
        * @param listener          API 결과 통지
        *
        *  \~english
        * @brief Request purchase of market product (marketPid) registered in App Store, Google Store, etc.<br/>
        * iapPayload is supplementary information for the purchase request, and it can be checked in Receipt Verification after purchase, <br/>
        * but, in case of purchasing outside of the app, such as a promo code purchase etc., it can not be included in verification result.
        * When upgrading or downgrading a subscription-type product, the unique code oldMarketPid of the existing subscription product must be entered.
        *
        * @param marketPid         Unique code registered on the App Store or Google Store.
        * @param oldMarketPid      Unique code of existing subscription product
        * @param iapPayload        A string containing supplementary information about the purchase request, specified by the developer.
        * @param listener          Result of API call
        *
        *  \~
        * @ingroup IAPV4
        * @since 4.11.6
        */
        public static void purchaseSubscriptionUpdate(String marketPid, String oldMarketPid, String iapPayload, onIAPV4Purchase listener) {

            JSONObject jsonParam = HIVEUnityPlugin.createParam("IAPV4", "purchaseSubscriptionUpdate", listener);
            jsonParam.AddField ("marketPid", marketPid);
            jsonParam.AddField ("oldMarketPid", oldMarketPid);
            jsonParam.AddField ("iapPayload", iapPayload);

            HIVEUnityPlugin.callNative (jsonParam);    
        }

        /**
        *  \~korean
        * @brief App Store, Google Store 등 에 등록된 마켓 상품(marketPid)의 구매를 요청 한다.<br/>
        * iapPayload 의 경우 구매 요청에 대한 보충 정보로 구매 후 영수증 검증에서 확인할 수 있다.<br/>
        * 프로모 코드 구매 처럼 앱 외부에서 구매 등의 경우에는 포함될 수 없게 된다.
        * 구독형 상품을 업그레이드하거나 다운그레이드를 수행하는 경우 기존에 구독 중인 상품의 고유코드 oldMarketPid를 필수로 넣어야 한다.
        *
        * @param param             구매시 필요한 prameter 의 모음
        * @param listener          API 결과 통지
        *
        *  \~english
        * @brief Request purchase of market product (marketPid) registered in App Store, Google Store, etc.<br/>
        * iapPayload is supplementary information for the purchase request, and it can be checked in Receipt Verification after purchase, <br/>
        * but, in case of purchasing outside of the app, such as a promo code purchase etc., it can not be included in verification result.
        * When upgrading or downgrading a subscription-type product, the unique code oldMarketPid of the existing subscription product must be entered.
        *
        * @param param             A collection of parameters required for purchase.
        * @param listener          Result of API call
        *
        *  \~
        * @ingroup IAPV4
        * @since 4.16.4.0
        */
        public static void purchaseSubscription(IAPV4PurchaseParam param, onIAPV4Purchase listener)
        {

            JSONObject jsonParam = HIVEUnityPlugin.createParam("IAPV4", "purchaseSubscription", listener);
            jsonParam.AddField("param", param.toJson());

            HIVEUnityPlugin.callNative(jsonParam);
        }

        /**
        *  \~korean
        * @brief HIVE IAPV4 상품에 대한 미지급된 아이템 지급을 요청한다.<br/>
        * 상품 구매 과정에서 여러가지 상황으로 인하여 아이템 지급을 실패하는 경우가 발생할 수 있다.<br/>
        * 이런 상황을 대비하기 위해서 미 지급된 아이템 지급을 요청 해야 한다.<br/>
        *
        * @param listener            API 결과 통지
        *
        *  \~english
        * @brief HIVE IAPV4  It restore the item that has not been delivered.<br/>
        * It may happen that the item delivery fails due to various situations in the product purchase process.<br/>
        * In order to prepare for this situation, you should request to deliver the item that has not been delivered.
        *
        * @param listener            API 결과 통지
        *
        *  \~
        * @ingroup IAPV4
        */
        public static void restore(onIAPV4Restore listener) {

            JSONObject jsonParam = HIVEUnityPlugin.createParam("IAPV4", "restore", listener);

            HIVEUnityPlugin.callNative (jsonParam);    
        }

        /**
        *  \~korean
        * @brief HIVE IAPV4 상품에 대한 미지급된 구독 상품 지급을 요청한다.<br/>
        * 상품 구매 과정에서 여러가지 상황으로 인하여 구독 상태를 갱신하지 못할 수 있다.<br/>
        * 구독 상태를 확인할 수 없는 경우, 구독 상태 복구를 요청 해야 한다.<br/>
        *
        * @param listener            API 결과 통지
        *
        *  \~english
        * @brief HIVE IAPV4  It restore the item that has not been delivered.<br/>
        * 
        * 
        *
        * @param listener            Result of API call
        *
        *  \~
        * @ingroup IAPV4
        * @since 4.11.6
        */
        public static void restoreSubscription(onIAPV4Restore listener) {

            JSONObject jsonParam = HIVEUnityPlugin.createParam("IAPV4", "restoreSubscription", listener);

            HIVEUnityPlugin.callNative (jsonParam);    
        }

        /**
        *  \~korean
        * @brief HIVE IAP v4에서 지급이 완료된 상품에 대한 구매 영수증에 대한 트랜잭션 완료 처리를 요청한다.<br/>
        * 구매나 복구 기능 사용 후 아이템 지급이 완료된 시점에 이 API를 호출하여 마켓에 구매 완료 처리를 해야한다.<br/>
         * 
        * @warning 만약, 지급 완료 후에도 마켓으로의 구매 완료 처리를 하지 않으면, 미지급된 상품으로 남는다.<br/>
        * 미지급된 상품은 transactionFinish API 호출 전까지 계속 복구가 되므로, 영수증 중복 여부를 확인해야 한다.<br/>
        * 
        * @param marketPid    지급이 완료되어 트랜잭션 완료 처리를 위한 상품의 고유 ID
        * @param listener    API 결과 통지
        *  
        *  \~english
        * @brief HIVE IAP v4 It requests the transaction completion processing for the purchase receipt of the item that have been delivered.<br/>
        * At the completion of item delivery after purchasing or restoring, this API should be called to complete the purchase on the market.<br/>
         * 
        * @warning If you do not notify the completion of the purchase to the market after payment has been made, it will be left as an unpaid item..<br/>
        * Since the unpaid items will continue to be recovered until the transactionFinish API call, you should check for duplicate receipts.<br/>
        * 
        * @param marketPid    The unique ID of the item for which the transaction is to be completed.
        * @param listener    Result of API call
        *
        *  \~
        * @see onIAPV4TransactionFinish
        * @see #transactionMultiFinish(List, onIAPV4TransactionMultiFinish)
        * 
        * @ingroup IAPV4
        */
        public static void transactionFinish(String marketPid, onIAPV4TransactionFinish listener) {

            JSONObject jsonParam = HIVEUnityPlugin.createParam("IAPV4", "transactionFinish", listener);
            jsonParam.AddField ("marketPid", marketPid);

            HIVEUnityPlugin.callNative (jsonParam);    
        }

        /**
        *  \~korean
        * @brief HIVE IAP v4에서 지급이 완료된 상품에 대한 구매 영수증에 대한 복수 개의 트랜잭션 완료 처리를 요청한다.<br/>
        * 구매나 복구 기능 사용 후 아이템 지급이 완료된 시점에 이 API를 호출하여 마켓에 구매 완료 처리를 해야한다.<br/>
        * 
        * @warning 만약, 지급 완료 후에도 마켓으로의 구매 완료 처리를 하지 않으면, 미지급된 상품으로 남는다.<br/>
        * 미지급된 상품은 transactionFinish API 호출 전까지 계속 복구가 되므로, 영수증 중복 여부를 확인해야 한다.<br/>
        * 
        * @param marketPidList    지급이 완료되어 트랜잭션 완료 처리를 위한 상품의 고유 ID 목록
        * @param listener            API 결과 통지
        * 
        *  \~english
        * @brief HIVE IAP v4 It requests the multiple transaction completion processing for the purchase receipt of the item that have been delivered.<br/>
        * At the completion of item delivery after purchasing or restoring, this API should be called to complete the purchase on the market.<br/>
         * 
        * @warning If you do not notify the completion of the purchase to the market after payment has been made, it will be left as an unpaid item..<br/>
        * Since the unpaid items will continue to be recovered until the transactionFinish API call, you should check for duplicate receipts.<br/>
        * 
        * @param marketPidList    The list of unique ID of the item for which the transaction is to be completed.
        * @param listener    Result of API call
        * 
        *  \~
        * @see onIAPV4TransactionMultiFinish
        * @see #transactionFinish(String, onIAPV4TransactionFinish)
        * 
        * @ingroup IAPV4
        */
        public static void transactionMultiFinish(List<String> marketPidList, onIAPV4TransactionMultiFinish listener) {

            JSONObject jsonParam = HIVEUnityPlugin.createParam("IAPV4", "transactionMultiFinish", listener);

            JSONObject jsonArray = new JSONObject();

            if(marketPidList != null) {
                foreach(String marketPid in marketPidList) {

                    jsonArray.Add(marketPid.ToString());
                }
            }

            jsonParam.AddField ("marketPidList", jsonArray);

            HIVEUnityPlugin.callNative (jsonParam);    
        }

#if !UNITY_EDITOR && UNITY_ANDROID

        /**
        *  \~korean
        * @brief 러비 상점이나 구글 상점을 선택하기 위한 창을 띄운다.<br/>
        * 만약 Google Store 와 HIVE 러비(Lebi) 상점을 동시에 사용 가능한 상황이라면 유저가 상점 선택을 할 수 있도록 상점 선택 화면을 노출해야 한다
        *
        * @param listener            API 결과 통지
        *
        *  \~english
        * @brief It shows a UI to select a Lebi store or a Google store..<br/>
        * If the Google Store and the HIVE Lebi store are available at the same time, you should expose the store selection screen to allow the user to select a store.
        *
        * @param listener            Result of API call
        * 
        *  \~
        * @ingroup IAPV4
        */
        public static void showMarketSelection(onIAPV4MarketInfo listener) {

            JSONObject jsonParam = HIVEUnityPlugin.createParam("IAPV4", "showMarketSelection", listener);

            HIVEUnityPlugin.callNative (jsonParam);    
        }

        /**
        *  \~korean
        * @brief 러비 상점일 경우 잔액 정보 조회.
        *
        * @param listener            API 결과 통지
        *
        *  \~english
        * @brief It request Lebi balance information.
        *
        * @param listener            Result of API call
        *
        *  \~
        * @ingroup IAPV4
        */
        public static void getBalanceInfo(onIAPV4Balance listener) {

            JSONObject jsonParam = HIVEUnityPlugin.createParam("IAPV4", "getBalanceInfo", listener);

            HIVEUnityPlugin.callNative (jsonParam);    
        }

        /**
        *  \~korean
        * @brief 러비 충전 페이지 노출
        *
        * @param listener            API 결과 통지
        * 
        *  \~english
        * @brief It shows Lebi refill page.
        *
        * @param listener            Result of API call
        *
        *  \~
        * @ingroup IAPV4
        */
        public static void showCharge(onIAPV4Balance listener) {

            JSONObject jsonParam = HIVEUnityPlugin.createParam("IAPV4", "showCharge", listener);

            HIVEUnityPlugin.callNative (jsonParam);    
        }
#endif    

        public static void checkPromotePurchase(onIAPV4CheckPromotePurchase listener) {

            JSONObject jsonParam = HIVEUnityPlugin.createParam("IAPV4", "checkPromotePurchase", listener);

            HIVEUnityPlugin.callNative (jsonParam);    
        }    

        /**
        * @brief 현재 선택된 마켓 타입을 반환한다. <br/>
        *
        * 얻어온 마켓 타입으로 구매를 진행하도록 한다.<br/>
        *
        * @see #marketConnect(IAPV4MarketInfoListener)
        *
        * @ingroup IAPV4
        */
        public static IAPV4Type getSelectedMarket() {

            JSONObject jsonParam = HIVEUnityPlugin.createParam("IAPV4", "getSelectedMarket", null);

            JSONObject resJsonObject = HIVEUnityPlugin.callNative (jsonParam);

            IAPV4Type iapV4Type = IAPV4Type.NOT_SELECTED;
            string iapV4TypeName = null;
            resJsonObject.GetField (ref iapV4TypeName, "iapv4Type");

            if (iapV4TypeName != null) {
                iapV4Type = getIAPV4Type(iapV4TypeName);
            } else {
                iapV4Type = IAPV4Type.NOT_SELECTED;
            }
            
            return iapV4Type;
        }

        /**
        * @brief 현재 계정의 UUIDv3 값을 반환한다. <br/>
        *
        * Returns the Hive account ID converted to a UUID version 3. return null, if not logged in.
        *
        * @ingroup IAPV4
        * @since 4.16.3.0
        *
        * @return accountUuid
        */
        public static string getAccountUuid()
        {

            JSONObject jsonParam = HIVEUnityPlugin.createParam("IAPV4", "getAccountUuid", null);

            JSONObject resJsonObject = HIVEUnityPlugin.callNative(jsonParam);


            string iapV4TypeName = null;
            resJsonObject.GetField(ref iapV4TypeName, "accountUuid");

            return iapV4TypeName;
        }


        /**
         * Google In-App Offers
         *
         * @param categoryId 3(PROMOTIONAL)
         *
         * @param listener Int(responseCode) 가 2(NAVIGATE_TO_DEV_APP_STORE) 일 경우 유저가 제안(Offer)를 확인하고 사용하기를 원한 상황입니다.
         * 개발자는 유저가 구매할 인앱 항목을 선택할 수 있는 게임/앱 내 상점으로 (유저가 해당 화면에 없는 경우) 유저를 이동해야 합니다.
         * 0 은 아무런 액션을 하지 않아도 되고, 음수는 에러상황 입니다.
         *
         * @since 4.21.0.0
         */
        public static void showInAppMessages(int categoryId, onIAPV4ShowInAppMessages listener)
        {
            JSONObject jsonParam = HIVEUnityPlugin.createParam("IAPV4", "showInAppMessages", listener);
            jsonParam.AddField("categoryId", categoryId);
            HIVEUnityPlugin.callNative(jsonParam);
        }


        public static IAPV4Type getIAPV4Type(String iapV4TypeName) {

            if ("NOT_SELECTED".Equals (iapV4TypeName)) {
                return IAPV4Type.NOT_SELECTED;
            }
            else if ("APPLE_APPSTORE".Equals (iapV4TypeName)) {
                return IAPV4Type.APPLE_APPSTORE;
            }
            else if ("GOOGLE_PLAYSTORE".Equals (iapV4TypeName)) {
                return IAPV4Type.GOOGLE_PLAYSTORE;
            }
            else if ("HIVE_LEBI".Equals (iapV4TypeName)) {
                return IAPV4Type.HIVE_LEBI;
            }
            else if ("ONESTORE".Equals (iapV4TypeName)) {
                return IAPV4Type.ONESTORE;
            }
            else if ("AMAZON_APPSTORE".Equals (iapV4TypeName)) {
                return IAPV4Type.AMAZON_APPSTORE;
            }
            else if ("SAMSUNG_GALAXYSTORE".Equals (iapV4TypeName)) {
                return IAPV4Type.SAMSUNG_GALAXYSTORE;
            }
            else if ("HUAWEI_APPGALLERY".Equals (iapV4TypeName)) {
                return IAPV4Type.HUAWEI_APPGALLERY;
            }
            else if ("FUNTAP".Equals (iapV4TypeName)) {
                return IAPV4Type.FUNTAP;
            }
            else if ("OPPO_APPMARKET".Equals (iapV4TypeName)) {
                return IAPV4Type.OPPO_APPMARKET;
            }
            else if ("VIVO_APPSTORE".Equals(iapV4TypeName))
            {
                return IAPV4Type.VIVO_APPSTORE;
            }
            else if ("TENCENT_MYAPP".Equals(iapV4TypeName))
            {
                return IAPV4Type.TENCENT_MYAPP;
            }
            else if ("XIAOMI_APPSTORE".Equals(iapV4TypeName))
            {
                return IAPV4Type.XIAOMI_APPSTORE;
            }
            else if ("HUAWEI_APPGALLERY_CHINA".Equals(iapV4TypeName))
            {
                return IAPV4Type.HUAWEI_APPGALLERY_CHINA;
            }
            else if ("FACEBOOK_CLOUD_GAME".Equals(iapV4TypeName))
            {
                return IAPV4Type.FACEBOOK_CLOUD_GAME;
            }
            else if ("HIVESTORE".Equals(iapV4TypeName))
            {
                return IAPV4Type.HIVESTORE;
            }
            else if ("STEAM".Equals(iapV4TypeName))
            {
                return IAPV4Type.STEAM;
            }
            else if ("NOWGG".Equals(iapV4TypeName))
            {
                return IAPV4Type.NOWGG;
            }

            return IAPV4Type.GOOGLE_PLAYSTORE;
        }

        public static IAPV4Receipt IAPV4ReceiptJsonTo4Receipt(JSONObject iapV4ReceiptJson) {

            if (iapV4ReceiptJson == null)
                return null;

            IAPV4Receipt iapV4Receipt = null;

            String iapV4TypeStr = "";
            iapV4ReceiptJson.GetField(ref iapV4TypeStr, "type");
            IAPV4Type type = getIAPV4Type(iapV4TypeStr);

            if (type == IAPV4Type.HIVESTORE) {
                iapV4Receipt = new IAPV4Receipt();
            }
            else {
                iapV4Receipt = new IAPV4Receipt(iapV4ReceiptJson);
            }
            
            return iapV4Receipt;
        }

        public static void executeEngine(JSONObject resJsonObject) {

            String methodName = null;
            resJsonObject.GetField (ref methodName, "method");

            int handlerId = -1;
            resJsonObject.GetField (ref handlerId, "handler");
            object handler = (object)HIVEUnityPlugin.popHandler (handlerId);

            if (handler == null) return;

            if ("marketConnect".Equals (methodName)
#if !UNITY_EDITOR && UNITY_ANDROID            
            || "showMarketSelection".Equals (methodName)
#endif
            ) {

                List<IAPV4Type> iapV4TypeList = new List<IAPV4Type> ();

                JSONObject jsonArray = resJsonObject.GetField ("iapV4TypeList");
                if (jsonArray != null && jsonArray.count > 0) {
                    List<JSONObject> jsonList = jsonArray.list;
                    foreach (JSONObject jsonItem in jsonList) {

                        String iapV4TypeName = jsonItem.stringValue;
                        IAPV4Type iapV4Type = getIAPV4Type(iapV4TypeName);

                        iapV4TypeList.Add(iapV4Type);
                    }
                }

                onIAPV4MarketInfo listener = (onIAPV4MarketInfo)handler;
                listener (new ResultAPI (resJsonObject.GetField ("resultAPI")), iapV4TypeList);
            }
            else if ("getMarketProductInfo".Equals (methodName) || "getProductInfo".Equals (methodName) || "getSubscriptionProductInfo".Equals (methodName)) {

                List<IAPV4Product> iapV4ProductList = new List<IAPV4Product> ();

                JSONObject jsonArray = resJsonObject.GetField ("iapV4ProductList");
                if (jsonArray != null && jsonArray.count > 0) {
                    List<JSONObject> jsonList = jsonArray.list;
                    foreach(JSONObject jsonItem in jsonList) {
                        IAPV4Product iapV4Product = new IAPV4Product(jsonItem);

                        iapV4ProductList.Add(iapV4Product);
                    }
                }

                int balance = 0;
                resJsonObject.GetField (ref balance, "balance");

                onIAPV4ProductInfo listener = (onIAPV4ProductInfo)handler;
                listener (new ResultAPI (resJsonObject.GetField ("resultAPI")), iapV4ProductList, balance);
            }
            else if ("purchase".Equals (methodName) || "purchaseSubscriptionUpdate".Equals (methodName) || "purchaseSubscription".Equals (methodName)) {

                JSONObject iapV4ReceiptJson = resJsonObject.GetField("iapV4Receipt");                    
                IAPV4Receipt iapV4Receipt = IAPV4ReceiptJsonTo4Receipt(iapV4ReceiptJson);

                onIAPV4Purchase listener = (onIAPV4Purchase)handler;
                listener (new ResultAPI (resJsonObject.GetField ("resultAPI")), iapV4Receipt);
            }
            else if ("restore".Equals (methodName) || "restoreSubscription".Equals (methodName)) {

                List<IAPV4Receipt> iapv4ReceiptList = new List<IAPV4Receipt> ();

                JSONObject jsonArray = resJsonObject.GetField ("iapv4ReceiptList");
                if (jsonArray != null && jsonArray.count > 0) {
                    List<JSONObject> iapV4ReceiptJsonList = jsonArray.list;
                    foreach(JSONObject iapV4ReceiptJson in iapV4ReceiptJsonList) {

                        IAPV4Receipt iapV4Receipt = IAPV4ReceiptJsonTo4Receipt(iapV4ReceiptJson);

                        iapv4ReceiptList.Add(iapV4Receipt);
                    }
                }

                onIAPV4Restore listener = (onIAPV4Restore)handler;
                listener (new ResultAPI (resJsonObject.GetField ("resultAPI")), iapv4ReceiptList);
            }
            else if ("transactionFinish".Equals (methodName)) {

                String marketPid = "";
                resJsonObject.GetField (ref marketPid, "marketPid");

                onIAPV4TransactionFinish listener = (onIAPV4TransactionFinish)handler;
                listener (new ResultAPI (resJsonObject.GetField ("resultAPI")), marketPid);
            }
            else if ("transactionMultiFinish".Equals (methodName)) {

                List<ResultAPI> resultList = new List<ResultAPI> ();

                JSONObject jsonArray = resJsonObject.GetField ("resultList");
                if (jsonArray != null && jsonArray.count > 0) {
                    List<JSONObject> jsonList = jsonArray.list;
                    foreach(JSONObject jsonItem in jsonList) {
                        ResultAPI result = new ResultAPI(jsonItem);

                        resultList.Add(result);
                    }
                }

                List<String> marketPidList = new List<String> ();

                JSONObject jsonArray2 = resJsonObject.GetField ("marketPidList");
                if (jsonArray2 != null && jsonArray2.count > 0) {
                    List<JSONObject> jsonList = jsonArray2.list;
                    foreach(JSONObject jsonItem in jsonList) {

                        String marketPid = jsonItem.stringValue;
                        marketPidList.Add(marketPid);
                    }
                }

                onIAPV4TransactionMultiFinish listener = (onIAPV4TransactionMultiFinish)handler;
                listener (resultList, marketPidList);
            }
#if !UNITY_EDITOR && UNITY_ANDROID
            else if ("getBalanceInfo".Equals (methodName)
                || "showCharge".Equals (methodName)) {
                    
                int balance = 0;
                resJsonObject.GetField (ref balance, "balance");

                onIAPV4Balance listener = (onIAPV4Balance)handler;
                listener (new ResultAPI (resJsonObject.GetField ("resultAPI")), balance);
            }
#endif
            else if ("checkPromotePurchase".Equals (methodName)) {
                    
                String marketPid = "";
                resJsonObject.GetField (ref marketPid, "marketPid");

                onIAPV4CheckPromotePurchase listener = (onIAPV4CheckPromotePurchase)handler;
                listener (new ResultAPI (resJsonObject.GetField ("resultAPI")), marketPid);
            }

            else if ("showInAppMessages".Equals (methodName)) {

                int responseCode = -1;
                resJsonObject.GetField(ref responseCode, "responseCode");

                onIAPV4ShowInAppMessages listener = (onIAPV4ShowInAppMessages)handler;
                listener(responseCode);
            }
        }

        public enum IAPV4Type {

            NOT_SELECTED = 0
            , APPLE_APPSTORE = 1                ///< Apple App Store
            , GOOGLE_PLAYSTORE = 2              ///< Google Play Store
            , HIVE_LEBI = 3                     ///< HIVE Lebi
            , ONESTORE = 4                      ///< OneStore
            , AMAZON_APPSTORE = 5               ///< Amazon App Store
            , SAMSUNG_GALAXYSTORE = 6           ///< Samsung Galaxy Store
            , HUAWEI_APPGALLERY = 7             ///< Huawei App Gallery
            , FUNTAP = 8                        ///< Funtap
            , OPPO_APPMARKET = 9                ///< OPPO App Market
            , VIVO_APPSTORE = 10                ///< VIVO App Store
            , TENCENT_MYAPP = 11                ///< TENCENT Myapp
            , XIAOMI_APPSTORE = 12              ///< Xiaomi App Store
            , HUAWEI_APPGALLERY_CHINA = 13      ///< Huawei App Gallery China
            , FACEBOOK_CLOUD_GAME = 14          ///< Facebook Cloud Game
            , HIVESTORE = 15                    ///< HiveStore
            , STEAM = 16                        ///< Steam
            , NOWGG = 17                        ///< Nowgg
        }

        public class IAPV4Product {

            public String productType;          ///< \~korean consumable, subscription      \~english consumable, subscription
            public String marketPid;            ///< \~korean 마켓에 등록된 고유 상품 ID          \~english Unique product ID registered in Market
            public String currency;             ///< \~korean 통화 코드 (ex, KRW / USD)        \~english Currency code (ex, KRW / USD)    
            public double price;                ///< \~korean 상품 가격                        \~english Product price
            public String displayPrice;         ///< \~korean 상품 가격 문자열 (ex. $100.00)     \~english Product price string (ex. $100.00)
            public String title;                ///< \~korean 상품 타이틀                       \~english Item Title
            public String productDescription;   ///< \~korean 상품 설명                        \~english Product Description
            
            public String originalJson;         ///< \~korean HiveIAP 서버에서 받은 원문         \~english Original Json String received from HiveIAP server.
            public String originalMarketJson;   ///< \~korean 마켓에서 받은 원문                 \~english Original Json String received from Market.

            //Price before Promotion Sale
            public String displayOriginalPrice; /// 세일전 가격 문자열 Google Only 
            public double originalPrice;        /// 세일전 가격 Google Only 
            public String iconURL;              /// 상품 아이콘 (512x512) Google Only 

            // for Amazon
            public int coinsReward;             /// 구매시 추가 지급 코인량

            /**
            * 32-bit PNG (with alpha)
            *
            * Dimensions: 512px by 512px
            *
            * Maximum file size: 1024KB
            */

            public IAPV4Product() {
            }

            public IAPV4Product(String productType
                            , String marketPid, String currency
                            , double price, String displayPrice
                            , String title, String productDescription
                            , String originalJson) {

                this.productType = productType;
                this.marketPid = marketPid;
                this.currency = currency;
                this.price = price;
                this.displayPrice = displayPrice;
                this.title = title;
                this.productDescription = productDescription;
                this.originalJson = originalJson;
            }

            public IAPV4Product(JSONObject resJsonParam) {

                if(resJsonParam == null)
                    return;

                resJsonParam.GetField (ref this.productType, "productType");
                resJsonParam.GetField (ref this.marketPid, "marketPid");
                resJsonParam.GetField (ref this.currency, "currency");
                resJsonParam.GetField (ref this.price, "price");
                resJsonParam.GetField (ref this.displayPrice, "displayPrice");
                resJsonParam.GetField (ref this.title, "title");
                resJsonParam.GetField (ref this.productDescription, "productDescription");
                
                resJsonParam.GetField (ref this.originalJson, "originalJson");
                resJsonParam.GetField (ref this.originalMarketJson, "originalMarketJson");

                resJsonParam.GetField (ref this.displayOriginalPrice, "displayOriginalPrice");
                resJsonParam.GetField (ref this.originalPrice, "originalPrice");
                resJsonParam.GetField (ref this.iconURL, "iconURL");

                resJsonParam.GetField (ref this.coinsReward, "coinsReward");
            }

            public String toString() {

                StringBuilder sb = new StringBuilder();

                sb.Append("IAPV4Product { marketPid = ");
                sb.Append(marketPid);
                sb.Append(", currency = ");
                sb.Append(currency);
                sb.Append(", price = ");
                sb.Append(price);
                sb.Append(", displayPrice = ");
                sb.Append(displayPrice);
                sb.Append(", title = ");
                sb.Append(title);
                sb.Append(", productDescription = ");
                sb.Append(productDescription);
                sb.Append(", originalJson = ");
                sb.Append(originalJson);
                sb.Append(", originalMarketJson = ");
                sb.Append(originalMarketJson);
                sb.Append (" }\n");

                return sb.ToString();
            }
        }

        public class IAPV4Receipt {

            public IAPV4Type type;              ///< \~korean 영수증 마켓 정보        \~english  Receipt Market Information
            public IAPV4Product product;        ///< \~korean 구매 성공한 상품 정보    \~english  Purchased product information.
            public int quantity;                ///< \~korean 구매 수량             \~english The quantity of the purchased product.
            public String iapPayload;           ///< \~korean 구매 요청시 추가로 입력받은 iapPayload. Restore 영수증은 서버 검증 응답으로 확인해야 한다.
			                                    ///  \~english iapPayload entered in purchase request. The restore receipt must be confirmed with the server validation response.
            public String hiveiapReceipt;       ///< \~korean bypassInfo가 IAP서버 영수증 검증 요청시 보낼 데이터    \~english  Receipt Data to send when requesting validation to IAP server
            public String bypassInfo;           ///< \~korean 게임 쪽에서 영수증 검증 서버로 바로 전달(bypass)하는 데이터    \~english  data bypassed from game to receipt verification server

            public JSONObject originalJsonObj;

            public IAPV4Receipt() {}

            public IAPV4Receipt(JSONObject resJsonParam) {

                if(resJsonParam == null)
                    return;

                originalJsonObj = resJsonParam;

                String iapV4TypeName = "";
                resJsonParam.GetField(ref iapV4TypeName, "type");
                this.type = getIAPV4Type(iapV4TypeName);

                JSONObject productJson = resJsonParam.GetField("product");
                this.product = new IAPV4Product(productJson);

                resJsonParam.GetField (ref this.quantity, "quantity");
                resJsonParam.GetField (ref this.iapPayload, "iapPayload");
                resJsonParam.GetField (ref this.hiveiapReceipt, "hiveiapReceipt");
                resJsonParam.GetField (ref this.bypassInfo, "bypassInfo");
            }

            public virtual String toString() {

                StringBuilder sb = new StringBuilder();

                sb.Append("IAPV4Receipt { type = ");
                sb.Append(type.ToString());
                sb.Append(", product = ");
                sb.Append(product.toString());
                sb.Append(", quantity = ");
                sb.Append(quantity);
                sb.Append(", iapPayload = ");
                sb.Append(iapPayload);
                sb.Append(", hiveiapReceipt = ");
                sb.Append(hiveiapReceipt);
                sb.Append(", bypassInfo = ");
                sb.Append(bypassInfo);
                sb.Append (" }\n");

                return sb.ToString();
            }
        }


        public class IAPV4PurchaseParam
        {
            public readonly String marketPid;
            public readonly String oldMarketPid;
            public readonly String iapPayload;
            public readonly String offerToken;

            private IAPV4PurchaseParam(
                String marketPid,
                String oldMarketPid,
                String iapPayload,
                String offerToken
            ) {
                this.marketPid = marketPid;
                this.oldMarketPid = oldMarketPid;
                this.iapPayload = iapPayload;
                this.offerToken = offerToken;
            }

            public String toJson() {
                JSONObject jsonObj = new JSONObject();

                jsonObj.AddField("marketPid", marketPid);
                jsonObj.AddField("oldMarketPid", oldMarketPid);
                jsonObj.AddField("iapPayload", iapPayload);
                jsonObj.AddField("offerToken", offerToken);

                return jsonObj.ToString();
            }


            public class Builder {

                private String marketPid = "";
                private String oldMarketPid = "";
                private String iapPayload = "";
                private String offerToken = "";

                public Builder() {}

                public Builder setMarketPid(String p) { marketPid = p; return this; }
                public Builder setOldMarketPid(String p) { oldMarketPid = p; return this; }
                public Builder setIapPayload(String p) { iapPayload = p; return this; }
                public Builder setOfferToken(String p) { offerToken = p; return this; }

                public Builder setFromJson(String source) {
                    JSONObject jsonObj = new JSONObject(source);

                    jsonObj.GetField(ref this.marketPid, "marketPid");
                    jsonObj.GetField(ref this.oldMarketPid, "oldMarketPid");
                    jsonObj.GetField(ref this.iapPayload, "iapPayload");
                    jsonObj.GetField(ref this.offerToken, "offerToken");

                    return this;
                }

                public IAPV4PurchaseParam build() {
                    return new IAPV4PurchaseParam(marketPid, oldMarketPid, iapPayload, offerToken);
                }
            }

        }


    }
}


/** @} */



