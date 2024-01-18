using UnityEditor;
using UnityEngine;

namespace hive.manager{
    public partial class HIVEManager {
        static float maximumStep = 1.0f;
        static string stepName = "";

        /// <summary>
        /// 프로세스의 최대치를 설정합니다.
        /// updateProcessWorking의 step항목에 영향을 주게 되며 updateProcessWorking/setUpdateProcessStepMaximum 만큼으로 스탭의 진행정도를 표시합니다.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="maximum"></param>
        static void beginProcessWorking(string name="", float maximum = 1.0f) {
            EditorApplication.LockReloadAssemblies();
            setProcessWorkingName(name);
            maximumStep = maximum;
        }

        /// <summary>
        /// 현제 프로세스의 이름을 설정합니다.
        /// </summary>
        /// <param name="name"></param>
        static void setProcessWorkingName(string name) {
            stepName = name; 
        }

        /// <summary>
        /// 프로세스의 진행상태를 업데이트 합니다.
        /// </summary>
        /// <param name="process"></param>
        /// <param name="step"></param>
        static void updateProcessWorking(string process,float step) {
            EditorUtility.DisplayProgressBar(stepName, process, step/maximumStep);
        }

        /// <summary>
        /// 진행중인 작업이 끝나면 호출합니다.
        /// 스탭진행도 0으로 최대치를 기본값(1)으로 되돌립니다.
        /// </summary>
        /// <returns></returns>
        static void endProcessWorking() {
            EditorApplication.UnlockReloadAssemblies();
            EditorUtility.ClearProgressBar();
        }
    }
}