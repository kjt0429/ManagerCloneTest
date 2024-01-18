/**
 * @file    DataStore.cs
 * 
 *  @date		2022
 *  @copyright	Copyright © Com2uS Platform Corporation. All Right Reserved.
 *  @author		sjyong
 *  @since		4.15.8 
 */
using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;


namespace hive
{
    /**
     *  \~korean
     * Hive 데이터 스토어는 NoSQL 클라우드 데이터베이스를 사용하여 게임 구성 요소나 설정 정보 등 <br/>
     * 클라이언트에 사용되는 데이터를 저장하고 동기화할 수 있습니다. 배포 및 유지관리할 서버가 필요없는 클라이언트 기반의 게임이나 <br/>
     * 클라이언트 간 실시간 업데이트가 필요한 경우 좋은 솔루션이 될 수 있습니다.<br/>
     *
     * Hive 데이터 스토어의 특징은 다음과 같습니다:<br/>
     * - 데이터가 각 게임의 개별 저장소에 저장됩니다.<br/>
     * - 모든 데이터는 암호화되어 안전하게 전송됩니다.<br/>
     * - SQL 데이터베이스와 달리 테이블이나 행이 없으며 컬렉션으로 구성된 문서에 데이터를 저장합니다.<br/>
     * - 데이터는 키-값 쌍으로 문서화해 저장할 수 있습니다.<br/>
     * - 다수의 작은 문서가 모인 컬렉션을 저장하는 데에 최적화되어 있습니다.<br/>
     *
     *  \~english
     * Hive Data Store uses NoSQL Cloud Database to synchronize and store the data for use on the client, <br/>
     * such as the components of the game and the settings information. Using this service is a good <br/>
     * solution for the games based on the client or if it needs real-time updates between clients.<br/>
     *
     * Hive Data Store has the following features:<br/>
     * - The data is stored in the storage separately of each game.<br/>
     * - All data is encrypted and delivered safely.<br/>
     * - Unlike SQL database, NoSQL Cloud Database has no tables and rows, and the data is stored in documents consisting of collections.<br/>
     * - Stores and documented data as key-value pairs.<br/>
     * - Optimizes for storing the collection that consists of multiple documents.<br/>
     *  \~
     *
     * @since   4.15.8
     * @ingroup DataStore
     */
	public class DataStore {


		public delegate	void onSet(ResultAPI result);

		public delegate void onGet(ResultAPI result, string data);

        public delegate void onMyData(ResultAPI result, Dictionary<string, string> myData);

        public delegate void onUsersData(ResultAPI result, string key, Dictionary<long, string> usersData);

 		/**
         *  \~korean
         * 데이터 추가<br/>
         *
         * 데이터는 하나의 키-값 쌍으로 추가하거나 한꺼번에 여러 쌍을 Map 형태로 추가할 수 있습니다.<br/>
         * 요청 후 성공 또는 실패 결과를 받을 수 있습니다.<br/>
         * - 중요 : key에 해당하는 value는 단순한 문자열(String) 혹은 백슬래시(\)가 포함된 1차원의 JSON 문자열로 설정해야 합니다.<br/>
         *
         *  \~english
         * Add Data<br/>
         *
         * You can add data to the Data Store in one pair of a key-value or multiple pairs of map format at once.<br/>
         * After a request, true or false is returned.<br/>
         * - IMPORTANT : The key-value needs to be set up in a one-dimensional JSON string that includes a simple string or backslash(\).<br/>
         *  \~
         *
         * @since   4.15.8
         * @ingroup DataStore
         */
        public static void set(string key, string value,  onSet listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("DataStore", "set", listener);
            jsonParam.AddField ("key", key);
            jsonParam.AddField ("value", value);

			HIVEUnityPlugin.callNative (jsonParam);
		}

        /**
         *  \~korean
         * 데이터 추가<br/>
         *
         * 데이터는 하나의 키-값 쌍으로 추가하거나 한꺼번에 여러 쌍을 Map 형태로 추가할 수 있습니다.<br/>
         * 요청 후 성공 또는 실패 결과를 받을 수 있습니다.<br/>
         * - 중요 : key에 해당하는 value는 단순한 문자열(String) 혹은 백슬래시(\)가 포함된 1차원의 JSON 문자열로 설정해야 합니다.<br/>
         *
         *  \~english
         * Add Data<br/>
         *
         * You can add data to the Data Store in one pair of a key-value or multiple pairs of map format at once.<br/>
         * After a request, true or false is returned.<br/>
         * - IMPORTANT : The key-value needs to be set up in a one-dimensional JSON string that includes a simple string or backslash(\).<br/>
         *  \~
         *
         * @since   4.15.8
         * @ingroup DataStore
         */
        public static void set(Dictionary<string, string> data, onSet listener) {
            
            JSONObject jsonParam = HIVEUnityPlugin.createParam("DataStore", "set", listener);
			string jsonString = new JSONObject(data).ToString();
            
            jsonParam.AddField("data", jsonString);

            HIVEUnityPlugin.callNative (jsonParam);
        }

        /**
         *  \~korean
         * 내 데이터 하나 가져오기<br/>
         *
         *  \~english
         * Get one of my data<br/>
         *  \~
         *
         * @since   4.15.8
         * @ingroup DataStore
         */
        public static void get(string key, onGet listener) {
            
            JSONObject jsonParam = HIVEUnityPlugin.createParam("DataStore", "get", listener);
            jsonParam.AddField("key", key);

            HIVEUnityPlugin.callNative (jsonParam);
        }

        /**
         *  \~korean
         * 내 데이터 모두 가져오기<br/>
         *
         *  \~english
         * Get all my data<br/>
         *  \~
         *
         * @since   4.15.8
         * @ingroup DataStore
         */
        public static void getMyData(onMyData listener) {
            JSONObject jsonParam = HIVEUnityPlugin.createParam("DataStore", "getMyData", listener);
            
            HIVEUnityPlugin.callNative (jsonParam);
        }

        /**
         *  \~korean
         * 키로 전체 데이터 가져오기<br/>
         *
         *  \~english
         * Get all data using key<br/>
         *  \~
         *
         * @since   4.15.8
         * @ingroup DataStore
         */
        public static void getUsersData(string key, onUsersData listener) {
            JSONObject jsonParam = HIVEUnityPlugin.createParam("DataStore", "getUsersData", listener);
            jsonParam.AddField("key", key);

            HIVEUnityPlugin.callNative(jsonParam);
        }


		// \internal
		// \~korean Native 영역에서 호출된 요청을 처리하기 위한 플러그인 내부 코드
		// \~english Plug-in internal code to handle requests invoked from the native code.
		public static void executeEngine(JSONObject resJsonObject) {

			String methodName = null;
			resJsonObject.GetField (ref methodName, "method");

			int handlerId = -1;
			resJsonObject.GetField (ref handlerId, "handler");
			object handler = (object)HIVEUnityPlugin.popHandler (handlerId);

			if (handler == null) return;

            if ("set".Equals (methodName)) {
                onSet listener = (onSet)handler;
                listener (new ResultAPI(resJsonObject.GetField("resultAPI")));
            }
            else if ("get".Equals (methodName)) {
                onGet listener = (onGet)handler;
                string dataValue = "";
                resJsonObject.GetField(ref dataValue, "data");
                listener (new ResultAPI(resJsonObject.GetField("resultAPI")), dataValue);
            }
            else if ("getMyData".Equals (methodName)) {
                onMyData listener = (onMyData)handler;

                Dictionary<string, string> myDataDictinoary = new Dictionary<string, string>();

                String myDataString = null;
                resJsonObject.GetField (ref myDataString, "myData");

                try
                {
                    JSONObject myDataObject = new JSONObject(myDataString);
                    myDataDictinoary = myDataObject.ToDictionary();
                }
                catch (System.Exception ignore) {}
                
                listener (new ResultAPI(resJsonObject.GetField("resultAPI")), myDataDictinoary);
            }
            else if ("getUsersData".Equals (methodName)) {
                onUsersData listener = (onUsersData)handler;

                String keyString = null;
                resJsonObject.GetField (ref keyString, "key");

                Dictionary<string, string> usersDataDictionary = new Dictionary<string, string>();

                String usersDataString = null;
                resJsonObject.GetField(ref usersDataString, "usersData");

                try
                {
                    JSONObject usersDataObject = new JSONObject(usersDataString);
                    usersDataDictionary = usersDataObject.ToDictionary();
                }
                catch (System.Exception ignore) {}

                Dictionary<long, string> parsedUsersDataDictionary = new Dictionary<long, string>();
                foreach (string key in usersDataDictionary.Keys) {
                    long keyLong = long.Parse(key);
                    parsedUsersDataDictionary[keyLong] = usersDataDictionary[key];
                }

                listener (new ResultAPI(resJsonObject.GetField("resultAPI")), keyString, parsedUsersDataDictionary);
            }
		}

	}
}


/** @} */



