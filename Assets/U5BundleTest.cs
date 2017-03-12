using UnityEngine;
using System.Collections;
using System.IO;

public class U5BundleTest : MonoBehaviour {

	public string bundlePrefix = "Assetbundles";

	public string assetName = ""; 

	public string filePathRoot = "";

	// Use this for initialization
	void Start () {
		#if UNITY_ANDROID && !UNITY_EDITOR
		filePathRoot = "/sdcard/";
		#else
		filePathRoot = Application.persistentDataPath;
		#endif
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void LoadBundle(){
		
	}

	IEnumerator LoadFromFile(string name, string path, System.Action<Object> cb){
		var req = AssetBundle.LoadFromFileAsync (path);
		yield return req;
		if (req.assetBundle == null) {
			Debug.LogError ("Missing bundle from file in :" + path);
			yield break;
		}
		Debug.Log ("Load bundle from file async, success:" + path);
		if (!req.assetBundle.Contains (name)) {
			Debug.LogError ("Missing Asset:" + name + " in bundle:" + path);
			cb (null);
		} else {
			var req2 = req.assetBundle.LoadAssetAsync (name);
			yield return req2;
			cb (req2.asset);
		}
		req.assetBundle.Unload (true);
	}

	IEnumerator LoadFromWWW(string name, string path, System.Action<Object> cb){
		var req = new WWW (path);
		yield return req;
		if (req.assetBundle == null) {
			Debug.LogError ("Miss bundle from www in :" + path);
			yield break;
		}
		Debug.Log ("Load bundle from www, success:" + path);
		if (!req.assetBundle.Contains (name)) {
			Debug.LogError ("Missing Asset:" + name + " in bundle:" + path);
			cb (null);
		} else {
			var req2 = req.assetBundle.LoadAssetAsync (name);
			yield return req2;
			cb (req2.asset);
		}
		req.assetBundle.Unload (true);
	}

	void OnLoadedAsset(Object asset){
		Debug.Log ("Asset loaded named: " + (asset == null ? "null" : asset.name));
	}

	void OnGUI(){
		if(GUILayout.Button("File From Streaming", GUILayout.Width(400), GUILayout.Height(160))){
			#if UNITY_ANDROID && !UNITY_EDITOR
			StartCoroutine(LoadFromFile(assetName, Application.dataPath + "!assets/" + Path.Combine(bundlePrefix, assetName.ToLower() + ".u3d"), OnLoadedAsset));
			#else
			StartCoroutine(LoadFromFile(assetName, Path.Combine(Application.streamingAssetsPath, Path.Combine(bundlePrefix, assetName.ToLower() + ".u3d")), OnLoadedAsset));
			#endif
		}

		if(GUILayout.Button("File From Local", GUILayout.Width(400), GUILayout.Height(160))){
			StartCoroutine(LoadFromFile(assetName, Path.Combine(filePathRoot, Path.Combine(bundlePrefix, assetName.ToLower() + ".u3d")), OnLoadedAsset));
		}

		if(GUILayout.Button("WWW From Streaming", GUILayout.Width(400), GUILayout.Height(160))){
			#if UNITY_ANDROID && !UNITY_EDITOR
			StartCoroutine(LoadFromWWW(assetName, Path.Combine(Application.streamingAssetsPath, Path.Combine(bundlePrefix, assetName.ToLower() + ".u3d")), OnLoadedAsset));
			#else
			StartCoroutine(LoadFromWWW(assetName, "file://" + Path.Combine(Application.streamingAssetsPath, Path.Combine(bundlePrefix, assetName.ToLower() + ".u3d")), OnLoadedAsset));
			#endif
		}

		if(GUILayout.Button("WWW From Local", GUILayout.Width(400), GUILayout.Height(160))){
			StartCoroutine(LoadFromWWW(assetName, "file://" + Path.Combine(filePathRoot, Path.Combine(bundlePrefix, assetName.ToLower() + ".u3d")), OnLoadedAsset));
		}

		filePathRoot = GUILayout.TextField(filePathRoot, GUILayout.Width(800), GUILayout.Height(320));
	}
}
