using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor.Skill
{
	public class SkillView
	{
		private string skillIdStr;
		public int SkillId { get; private set; }
		public string AnimName { get; private set; }
		public void Draw()
		{
			GUILayout.BeginHorizontal();
			skillIdStr = GUILayout.TextField(skillIdStr, EUtility.GUI.WHOptions(100, 20));
			if (GUILayout.Button("检索", EUtility.GUI.WHOptions(40, 20)))
			{

			}
			GUILayout.Space(100);
			if (GUILayout.Button("添加新技能", EUtility.GUI.WHOptions(100, 20)))
			{

			}
			GUILayout.EndHorizontal();
		}
	}
}