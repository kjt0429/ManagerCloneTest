using System.Globalization;
using System.Linq;
using UnityEngine;

namespace hive.manager
{
    public static class HIVEGlobalizationUtility {
        public static CultureInfo GetCurrentCultureInfo()
        {
            SystemLanguage currentLanguage = Application.systemLanguage;
            CultureInfo correspondingCultureInfo = CultureInfo.GetCultures(CultureTypes.AllCultures).FirstOrDefault(x => x.EnglishName.Equals(currentLanguage.ToString()));
            return CultureInfo.CreateSpecificCulture(correspondingCultureInfo.TwoLetterISOLanguageName);
        }
        
        public static string getOSLanguageCodeLower() {
            CultureInfo ci = GetCurrentCultureInfo();//CultureInfo.InstalledUICulture ;
            string lancode = ci.TwoLetterISOLanguageName.ToLower();
            return lancode;
        }

        public static string IfOSLanguageCodeMatchStringOrAltString(this string matchString, string languageCode, string altString) {
            if (getOSLanguageCodeLower().Equals(languageCode.ToLower())) {
                return matchString;
            } 
            return altString;
        }
    }   
}
