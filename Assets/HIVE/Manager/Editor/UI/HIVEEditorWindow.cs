using UnityEngine;
using UnityEditor;

namespace hive.manager.editor
{
    /// <summary>
    /// EditorWindow를 상속한 클래스
    /// 팝업에서 주로 사용되는 각종 유틸성 함수를 모아놓음.
    /// </summary>
    public class HIVEEditorWindow<T> : EditorWindow where T : EditorWindow {

        protected static Vector2 DefaultPopupButtonSize = new Vector2(100,40);
        
        ~HIVEEditorWindow() {
            instance = null;
        }

        private static T instance;
        protected static T ShowSingletonPopup() {
            if (instance != null) {
                instance.Focus();
                return instance;
            }
            instance = CreateInstance<T>();
            instance.ShowUtility();
            return instance;
        }

        public void FixWindowSize(float width, float height) {
            this.minSize = new Vector2(width,height);
            this.maxSize = new Vector2(width,height);
        }

        protected Rect centerHorizontalRect(float x, float y, float width, float height) {
            return new Rect(x+position.width/2-width/2,y,width,height);
        }

        static protected HIVEEditorStrings getEditorStrings() {
            return HIVEEditorStrings.Instance;
        }
    }
}