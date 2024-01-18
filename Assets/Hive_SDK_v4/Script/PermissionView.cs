/**
 * @file    PermssionView.cs
 * 
 *  @date		2016-2022
 *  @copyright	Copyright © Com2uS Platform Corporation. All Right Reserved.
 *  @author		cocororo
 *  @since		4.11.2 
 */
using System.Collections.Generic;

namespace hive
{
    public enum PermissionViewPermissionCategory {
    Camera = 1,
    Contacts = 2,
    Location = 3,
    Microphone = 4,
    Phone = 5,
    Photo = 6,
    Push = 7,
    Storage = 8,
    Common = 9,
    IDFA = 10,
    Other = 99999
    }

    public class PermissionViewUnitData {
        public PermissionViewUnitData(string title,string contents,string permissionName, PermissionViewPermissionCategory category) {
            this.title = title;
            this.contents = contents;
            this.nativePermissionName = permissionName;
            this.permissionCategory = category;
        }
        public PermissionViewUnitData(JSONObject resJsonParam) { 
            if (resJsonParam == null || resJsonParam.count <= 0)
				return;
            
            string _contents = "";
            string _permissionName = "";
            string _title = "";
            int _permissionCategory = (int)PermissionViewPermissionCategory.Other;
            resJsonParam.GetField(ref _contents, "contents");
            resJsonParam.GetField(ref _permissionName, "nativePermissionName");
            resJsonParam.GetField(ref _permissionCategory, "permissionCategory");
            resJsonParam.GetField(ref _title, "title");

            this.title = _title;
            this.contents = _contents;
            this.nativePermissionName = _permissionName; 
            this.permissionCategory = (PermissionViewPermissionCategory)_permissionCategory;
        }
        public string title {get;private set;} ///< UI에 노출되어야할 권한의 이름입니다.
        public string contents{get;private set;} ///< UI에 노출되어야할 권한의 내용입니다. 어떤 식으로 권한이 이용되는지 설명이 되어있습니다.
        public string nativePermissionName{get;private set;} ///< UI에는 노출하지 않는 권한의 이름입니다. 해당 이름으로 어떠한 권한을 이용하는지 판단 할 수 있습니다.
        public PermissionViewPermissionCategory permissionCategory {get;private set;}
    }

    public class PermissionViewData {
        public PermissionViewData(string contents, List<PermissionViewUnitData> permssions, List<PermissionViewUnitData> commons) {
            this.contents = contents;
            this.permissions = permissions;
            this.commons = commons;
        }
        public PermissionViewData(JSONObject resJsonParam) { 
            if (resJsonParam == null || resJsonParam.count <= 0)
				return;
			
            string _contents ="";
			resJsonParam.GetField (ref _contents, "contents");
            this.contents = _contents;

            List<PermissionViewUnitData> _permissions = new List<PermissionViewUnitData>();

            JSONObject jsonArray = resJsonParam.GetField("permissions");
            if (jsonArray != null && jsonArray.count > 0) {
                List<JSONObject> jsonList = jsonArray.list;
                foreach (JSONObject jsonItem in jsonList) {
                    _permissions.Add(new PermissionViewUnitData(jsonItem));
                }
            }
            this.permissions = _permissions;


            List<PermissionViewUnitData> _commons = new List<PermissionViewUnitData>();

            JSONObject jsonArrayCommons = resJsonParam.GetField("commons");
            if (jsonArrayCommons != null && jsonArrayCommons.count > 0)
            {
                List<JSONObject> jsonList = jsonArrayCommons.list;
                foreach (JSONObject jsonItem in jsonList)
                {
                    _commons.Add(new PermissionViewUnitData(jsonItem));
                }
            }
            this.commons = _commons;
        }

        public string contents {get;private set;} ///< 권한 고지 팝업에 대한 설명입니다. 권한고지 팝업에 대한 설명이 들어있으므로 UI에 노출하여 주세요.
        public List<PermissionViewUnitData> permissions{get;private set;} ///< 각 권한들에 대한 설명과 이름들이 들어있는 선형리스트 혹은 배열입니다. 데이터에 대해서는 PermissionViewUnitData를 참고하여 주세요. 리스트에 들어있는 데이터의 개수는 OS의 업데이트나 법령에 따라 추가적인 권한정보가 더 올수 있으니 UI 디자인은 여러개의 값이 들어 갈 수 있도록 변화에 능동적인 디자인이 되어야 합니다.
        public List<PermissionViewUnitData> commons { get; private set; }
    }
}