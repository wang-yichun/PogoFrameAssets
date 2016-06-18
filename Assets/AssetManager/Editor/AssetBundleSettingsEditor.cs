namespace pogorock
{
	using UnityEngine;
	using UnityEditor;
	using UnityEditorInternal;
	using AssetBundles;
	using System.IO;

	[CustomEditor (typeof(AssetBundleSettings))]
	public class AssetBundleSettingsEditor : Editor
	{
		private ReorderableList reorderableTargetPaths;

		void OnEnable ()
		{
			AssetBundleSettings settings = target as AssetBundleSettings;

			reorderableTargetPaths = new ReorderableList (serializedObject, serializedObject.FindProperty ("targetPaths"));
			reorderableTargetPaths.drawHeaderCallback = (Rect rect) => {
				EditorGUI.LabelField (rect, "配置输出的目标位置");
			};
			reorderableTargetPaths.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {

				rect.y += 2;
				AssetBundleTargetPath targetPath = settings.targetPaths [index];

				rect.width = 10;
				targetPath.enable = EditorGUI.Toggle (rect, targetPath.enable);

				rect.x += 20;
				rect.width = 300;
				rect.height = EditorGUIUtility.singleLineHeight;
				targetPath.targetPath = EditorGUI.DelayedTextField (rect, targetPath.targetPath);

				if ((Event.current.type == EventType.DragExited) && rect.Contains (Event.current.mousePosition)) {  
					//改变鼠标的外表  
					DragAndDrop.visualMode = DragAndDropVisualMode.Generic;  
					if (DragAndDrop.paths != null && DragAndDrop.paths.Length > 0) {  
						targetPath.targetPath = DragAndDrop.paths [0];  
					}  
				}  
			};
		}

		public override void OnInspectorGUI ()
		{
			serializedObject.Update ();

			EditorGUILayout.BeginVertical ();
			reorderableTargetPaths.DoLayoutList ();
			EditorGUILayout.EndVertical ();

			EditorGUILayout.Space ();
			if (GUILayout.Button ("开始输出")) {
				BuildAllAssetBundles ();
			}
			if (GUILayout.Button ("获取所有的 AssetBundle 名")) {
				GetNames ();
			}

			if (GUI.changed) {
				serializedObject.ApplyModifiedProperties ();
				EditorUtility.SetDirty (target);
			}
		}

		void BuildAllAssetBundles ()
		{
			AssetBundleSettings settings = target as AssetBundleSettings;

			foreach (var targetPath in settings.targetPaths) {
				if (targetPath.enable) {
//					BuildPipeline.BuildAssetBundles (targetPath.targetPath);

					// Choose the output path according to the build target.
					string outputPath = Path.Combine (targetPath.targetPath, Utility.GetPlatformName ());
					if (!Directory.Exists (outputPath))
						Directory.CreateDirectory (outputPath);

					//@TODO: use append hash... (Make sure pipeline works correctly with it.)
					BuildPipeline.BuildAssetBundles (outputPath, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);

				}
			}
		}

		void GetNames ()
		{
			var names = AssetDatabase.GetAllAssetBundleNames ();
			foreach (var name in names)
				Debug.Log ("AssetBundle: " + name);
		}
	}
}