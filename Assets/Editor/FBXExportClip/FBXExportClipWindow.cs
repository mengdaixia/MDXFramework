using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using Code.Fight.Animancer;

public class FBXExportClipWindow : EditorBaseWindow
{
	[MenuItem("Assets/导出动画片段")]
	private static void ExportClip()
	{
		var objs = Selection.objects;
		List<Object> animationClips = new List<Object>();
		foreach (var obj in objs)
		{
			for (int i = 0; i <= objs.Length - 1; i++)
			{
				// AnimationUtility.GetAnimationClips()方法可以检索与游戏对象或组件关联的动画剪辑数组。但是这里不适用
				// 使用AssetDatabase.LoadAllAssetsAtPath函数提取fbx中的AnimationClip，该函数接收一个路径参数，即fbx文件所在路径，然后返回一个Object类型的数组，数组中存放的是fbx文件中的所有资源

				var path = AssetDatabase.GetAssetPath(objs[i]);
				if (path.Contains(".fbx"))
				{
					var assets = AssetDatabase.LoadAllAssetsAtPath(path);
					// 取出其中的AnimationClip
					foreach (var asset in assets)
					{
						//UnityEngine.PreviewAnimationClip是在编辑器中查看动画的临时剪辑，比如在动画曲线编辑器中（名字格式如：__preview__Take 001），你可以看到一些动画的预览剪辑。
						//UnityEngine.AnimationClip是最终实际播放的动画剪辑，该剪辑可以保存在项目中，然后由Animator或Animation组件加载并播放。
						if (asset is AnimationClip)//脚本中没有UnityEngine.PreviewAnimationClip类型, 所以这里用string.Contains判断
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