using System;

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Framework
{
	namespace Utils
	{
		[Serializable]
		public struct AssetRef<T> : ISerializationCallbackReceiver where T : UnityEngine.Object
		{
			#region Public Data
			public string _filePath;
			public string _fileGUID;
			#endregion

			#region Private Data
			private T _asset;
#if UNITY_EDITOR
			[NonSerialized]
			public T _editorAsset;
			[NonSerialized]
			public bool _editorFoldout;
#endif
			#endregion

			#region Public Interface
			public static implicit operator T(AssetRef<T> property)
			{
				return property.LoadAsset();
			}

#if UNITY_EDITOR
			public static implicit operator AssetRef<T>(T asset)
			{
				return new AssetRef<T>(asset);
			}
#endif

			public static implicit operator string(AssetRef<T> property)
			{
				if (property.IsValid())
					return System.IO.Path.GetFileNameWithoutExtension(property._filePath);

				return "<" + typeof(T).Name + ">";
			}

			public bool IsValid()
			{
				return !string.IsNullOrEmpty(_filePath);
			}

			public T LoadAsset()
			{
				if (_asset == null)
				{
					string resourcePath = GetResouceFilePath();
					_asset = Resources.Load<T>(resourcePath) as T;
				}
				
				return _asset;
			}

			public void UnloadAsset()
			{
				if (_asset != null)
				{
					Resources.UnloadAsset(_asset);
					_asset = null;
				}
			}

			public string GetResouceFilePath()
			{
				return StringUtils.GetResourcePath(_filePath);
			}

			public string GetStreamingAssetFilePath()
			{
				return StringUtils.GetStreamingAssetPath(_filePath);
			}

			public string GetAssetFilePath()
			{
				return StringUtils.GetAssetPath(_filePath);
			}

#if UNITY_EDITOR
			public AssetRef(T asset)
			{
				_asset = null;
				_editorAsset = asset;
				_editorFoldout = true;

				if (asset != null)
				{
					_filePath = AssetDatabase.GetAssetPath(asset);
					_fileGUID = AssetDatabase.AssetPathToGUID(_filePath);
				}
				else
				{
					_filePath = null;
					_fileGUID = null;
				}
			}

			public AssetRef(string assetPath)
			{
				_asset = null;
				_editorAsset = null;
				_editorFoldout = true;

				_filePath = assetPath;
				_fileGUID = AssetDatabase.AssetPathToGUID(assetPath);
			}

			public void ClearAsset()
			{
				_filePath = null;
				_fileGUID = null;
			}
#endif
			#endregion

			#region ISerializationCallbackReceiver
			public void OnBeforeSerialize()
			{

			}

			public void OnAfterDeserialize()
			{
#if UNITY_EDITOR
				_editorAsset = FindEditorAsset();
#endif
			}
			#endregion

			#region ICustomEditable
#if UNITY_EDITOR
			public bool RenderObjectProperties(GUIContent label)
			{
				bool dataChanged = false;

				

				return dataChanged;
			}
#endif
			#endregion

#if UNITY_EDITOR
			private T FindEditorAsset()
			{
				T asset = null;

				if (!string.IsNullOrEmpty(_fileGUID))
				{
					string filepath = AssetDatabase.GUIDToAssetPath(_fileGUID);
					asset = AssetDatabase.LoadAssetAtPath(filepath, typeof(T)) as T;
				}

				if (asset == null && !string.IsNullOrEmpty(_filePath))
				{
					asset = AssetDatabase.LoadAssetAtPath(_filePath, typeof(T)) as T;
				}

				return asset;
			}
#endif
		}
	}
}