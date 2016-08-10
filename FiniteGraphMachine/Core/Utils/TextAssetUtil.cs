using System;
using System.IO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DTFiniteGraphMachine {
  public static class TextAssetUtil {
		private const string RESOURCES_PATH = "Assets/GameSpecific/Resources";
		private const string TEXT_ASSETS_FOLDER = "TextAssets";
		private const string FILE_EXTENSION = ".txt";

    public static void WriteToTextAsset(string serializedString, TextAsset source) {
      if (source == null) {
        Debug.LogWarning("WriteToTextAsset: failed to write because source is null!");
        return;
      }

#if UNITY_EDITOR
      string assetPath = AssetDatabase.GetAssetPath(source);
      File.WriteAllText(Application.dataPath + assetPath.Replace("Assets", ""), serializedString);
      AssetDatabase.SaveAssets();
      AssetDatabase.Refresh();
#endif
    }

		public static TextAsset GetOrCreateTextAsset(string filename) {
			TextAsset textAsset = Resources.Load(TEXT_ASSETS_FOLDER + "/" + filename) as TextAsset;
#if UNITY_EDITOR
      string textAssetFullPath = RESOURCES_PATH + "/" + TEXT_ASSETS_FOLDER + "/" + filename + FILE_EXTENSION;

			if (textAsset == null) {
				if (!AssetDatabase.IsValidFolder(RESOURCES_PATH + "/" + TEXT_ASSETS_FOLDER)) {
					AssetDatabase.CreateFolder(RESOURCES_PATH, TEXT_ASSETS_FOLDER);
				}
				File.WriteAllText(textAssetFullPath, "");
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();

  			textAsset = Resources.Load(TEXT_ASSETS_FOLDER + "/" + filename) as TextAsset;
			}
#endif

      if (textAsset == null) {
        Debug.LogError("Failed to find or create text asset for filename: " + filename);
        return new TextAsset();
      }

      return textAsset;
		}

    public static void WriteToTextAssetFilename(string serializedString, string filename) {
#if UNITY_EDITOR
      string textAssetFullPath = RESOURCES_PATH + "/" + TEXT_ASSETS_FOLDER + "/" + filename + FILE_EXTENSION;
			File.WriteAllText(textAssetFullPath, serializedString);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
#endif
    }
  }
}