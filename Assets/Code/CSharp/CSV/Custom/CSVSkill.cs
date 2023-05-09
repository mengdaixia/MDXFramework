using Code.Fight.Skill;
using System.Collections.Generic;
using UnityEngine;

public partial class CSVSkill
{
	public List<SkillDerive> SkillDeriveLst { get; private set; } = new List<SkillDerive>();

	private void OnPostDeserialized()
	{
		foreach (var str in sListSkillBtnDerive)
		{
			var sd = new SkillDerive();
			sd.Set(str);
			SkillDeriveLst.Add(sd);
		}
	}
	private static void OnPostAllDeserialized()
	{

	}
}