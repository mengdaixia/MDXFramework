using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using Code.Fight.Animancer;

public class FBXExportClipWindow : EditorBaseWindow
{
	[MenuItem("Assets/��������Ƭ��")]
	private static void ExportClip()
	{
		var objs = Selection.objects;
		List<Object> animationClips = new List<Object>();
		foreach (var obj in objs)
		{
			for (int i = 0; i <= objs.Length - 1; i++)
			{
				// AnimationUtility.GetAnimationClips()�������Լ�������Ϸ�������������Ķ����������顣�������ﲻ����
				// ʹ��AssetDatabase.LoadAllAssetsAtPath������ȡfbx�е�AnimationClip���ú�������һ��·����������fbx�ļ�����·����Ȼ�󷵻�һ��Object���͵����飬�����д�ŵ���fbx�ļ��е�������Դ

				var path = AssetDatabase.GetAssetPath(objs[i]);
				if (path.Contains(".fbx"))
				{
					var assets = AssetDatabase.LoadAllAssetsAtPath(path);
					// ȡ�����е�AnimationClip
					foreach (var asset in assets)
					{
						//UnityEngine.PreviewAnimationClip���ڱ༭���в鿴��������ʱ�����������ڶ������߱༭���У����ָ�ʽ�磺__preview__Take 001��������Կ���һЩ������Ԥ��������
						//UnityEngine.AnimationClip������ʵ�ʲ��ŵĶ����������ü������Ա�������Ŀ�У�Ȼ����Animator��Animation������ز����š�
						if (asset is AnimationClip)//�ű���û��UnityEngine.PreviewAnimationClip����, ����������string.Contains�ж�
						{
							if (!asset.name.Contains("__preview__"))
							{
								animationClips.Add(asset);
							}
						}
					}
					var ePath = path.Substring(0, path.LastIndexOf("/") + 1);
					foreach (AnimationClip Clip in animationClips)
					{
						Object newClip = new AnimationClip();
						EditorUtility.CopySerialized(Clip, newClip);
						newClip.name = Clip.name;
						AssetDatabase.CreateAsset(newClip, ePath + newClip.name + ".anim");
					}
					animationClips.Clear();
				}
			}
		}
		AssetDatabase.Refresh();
	}
}