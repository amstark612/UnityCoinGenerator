using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

// https://answers.unity.com/questions/926220/in-editor-object-placement-by-script-in-editor.html

[CustomEditor(typeof(CoinGenerator))]
public class CoinGeneratorExtension : Editor {
    #if UNITY_EDITOR
        override public void OnInspectorGUI() {
            CoinGenerator coinGenerator = (CoinGenerator)target;
            if (GUILayout.Button("Generate coins")) {
                coinGenerator.GenerateCoins();
            }

            if (GUI.changed) {
                EditorUtility.SetDirty(target);
            }

            DrawDefaultInspector();
        }
    #endif
}
