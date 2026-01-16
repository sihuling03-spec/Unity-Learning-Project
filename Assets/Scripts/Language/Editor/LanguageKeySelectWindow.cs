using UnityEditor;
using UnityEngine;

namespace Language
{
    using GUI = UnityEngine.GUI;
    public class LanguageKeySelectWindow : EditorWindow
    {
        static float width = 300;
        static float height = Screen.height - 200;
        static LanguageKeySelectWindow Win; 
        public static void OpenKeySelectWindow(string[] words, LanguageText text)
        {
            WordSort(ref words);
            Rect _rect = new Rect(Screen.width / 2, 100, width, height);
            Win = (LanguageKeySelectWindow)EditorWindow.GetWindowWithRect(typeof(LanguageKeySelectWindow), _rect, false, "KeySelect");
            Win.words = words;
            Win.text = text;
            Win.ViewRect = new Rect(0, 0, Win.position.width - 20, words.Length * 22);
            Win.Show();
        }
        public string[] words;
        public LanguageText text;
        public Vector2 scrollPosition;
        Rect ViewPos = new Rect(0, 0, width, height);
        public Rect ViewRect;
        private void OnGUI()
        {
            scrollPosition = GUI.BeginScrollView(ViewPos, scrollPosition, ViewRect, false, true);

            for (int i = 0; i < words.Length; i++)
            {
                if (GUI.Button(new Rect(0, i * 22, Screen.width, 20), words[i], "MiniToolbarButtonLeft"))
                {
                    GUI.contentColor = Color.black;
                    text.Key = words[i];
                    text.Value = LanguageService.Instance.GetStringByKey(text.label, text.Key);
                    EditorUtility.SetDirty(text);
                    EditorUtility.SetDirty(text.gameObject);
                }
            }

            GUI.EndScrollView();
        }

        public static void WordSort(ref string[] vstr)
        {
            int n = vstr.Length - 1;
            string str = null;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n - i; j++)
                {
                    if (string.CompareOrdinal(vstr[j], vstr[j + 1]) > 0)
                    {
                        str = vstr[j];
                        vstr[j] = vstr[j + 1];
                        vstr[j + 1] = str;
                    }
                }
            }
        }
    }
}
