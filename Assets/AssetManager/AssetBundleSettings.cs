namespace pogorock
{

	using UnityEngine;
	using System;
	using System.Collections;
	using System.Collections.Generic;

	#if UNITY_EDITOR
	using UnityEditor;

	[InitializeOnLoad]
	#endif

	public class AssetBundleSettings : ScriptableObject
	{
		public static readonly string assetName = "AssetBundleSettings";
		public static readonly string fullPath = "Assets/AssetManager/Resources/AssetBundleSettings.asset";

		private static AssetBundleSettings instance = null;

		public static AssetBundleSettings Instance {
			get {
				if (instance == null) {
					instance = Resources.Load<AssetBundleSettings> (assetName);
					if (instance == null) {
						instance = CreateInstance<AssetBundleSettings> ();

						#if UNITY_EDITOR
						AssetDatabase.CreateAsset (instance, fullPath);
						#endif
					}
				}
				return instance;
			}
		}
		#if UNITY_EDITOR
		[MenuItem ("PogoTools/AssetBundle Settings #&a")]
		public static void Execute ()
		{
			Selection.activeObject = AssetBundleSettings.Instance;
		}
		#endif

		// 定义数据载体
		public List<AssetBundleTargetPath> targetPaths;

		public int actionCount;
	}

	[System.Serializable]
	public class AssetBundleTargetPath
	{
		public string targetPath;
		public bool enable;
	}
}