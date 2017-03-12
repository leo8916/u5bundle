using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class BundleBuild{

	[MenuItem("Build/SelectionBundle")]
	public static void BuildSeletionsBundle(){
		var sels = Selection.GetFiltered (typeof(Object), SelectionMode.DeepAssets);
		BuildWithAssets (System.IO.Path.Combine(Application.dataPath, "../Assetbundles"), sels);
	}


	public static void BuildWithAssets(string path, Object[] assets){
		if (assets.Length == 0) {
			return;
		}
		#if UNITY_IOS
		BuildTarget target = BuildTarget.iOS;
		#elif UNITY_ANDROID
		BuildTarget target = BuildTarget.Android;
		#elif UNITY_EDITOR_OSX
		BuildTarget target = BuildTarget.StandaloneOSXIntel64;
		#else
		BuildTarget target = BuildTarget.StandaloneWindows;
		#endif
		path = System.IO.Path.Combine(path, target.ToString());
		if (!System.IO.Directory.Exists (path)) {
			System.IO.Directory.CreateDirectory (path);
		}
		List<AssetBundleBuild> bs = new List<AssetBundleBuild>();
		foreach (var a in assets) {
			AssetBundleBuild abb = new AssetBundleBuild ();
			var an = AssetDatabase.GetAssetPath (a);
			if (string.IsNullOrEmpty (an)) {
				continue;
			}
			abb.assetBundleName = an + ".u3d";
			abb.assetNames = new string[]{an};
			bs.Add (abb);
		}
		if (bs.Count == 0) {
			return;
		}


		AssetBundleManifest abm = BuildPipeline.BuildAssetBundles (path, bs.ToArray (), BuildAssetBundleOptions.None, target);
		if (abm == null) {
			Debug.LogError ("Build Failed!");
		}
	}
}
