using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Fight
{
	public class SkillManager : SubManager
	{
		private Dictionary<int, SkillRuntime> layer2SkillDic = new();
		private Dictionary<int, SkillRuntime> id2SkillDic = new();
		private List<SkillRuntime> skillLst = new List<SkillRuntime>();
		private List<int> playSkillLst = new List<int>();

		public Action<int> OnSkillEnd;
		public void Play(int id, bool is_next_frame = false)
		{
			if (is_next_frame)
			{
				playSkillLst.Add(id);
				return;
			}
			var conf = CSVSkill.Get(id);
			if (conf != null)
			{
				var skill = ObjectPool.Get<SkillRuntime>();
				skill.Set(this, owner, conf);
				skill.SetState(ESkillState.Start);
				OnPlaySkill(skill);
			}
		}
		public void Stop(int id)
		{
			if (id2SkillDic.TryGetValue(id, out SkillRuntime skill))
			{
				skill.SetState(ESkillState.End);
			}
		}
		public bool IsPlaying(int id)
		{
			if (id2SkillDic.TryGetValue(id, out SkillRuntime skill))
			{
				var state = skill.SkillState;
				return state == ESkillState.Start || state == ESkillState.Playing;
			}
			return false;
		}
		protected override void OnUpdate()
		{
			for (int i = 0; i < skillLst.Count; i++)
			{
				var skill = skillLst[i];
				switch (skill.SkillState)
				{
					case ESkillState.Start:
						skill.Start();
						break;
					case ESkillState.Playing:
						skill.Update();
						break;
					case ESkillState.End:
						OnStopSkill(skill);
						i--;
						break;
				}
			}
			if (playSkillLst.Count > 0)
			{
				foreach (var id in playSkillLst)
				{
					Play(id);
				}
				playSkillLst.Clear();
			}
		}
		private void OnPlaySkill(SkillRuntime skill)
		{
			var conf = skill.Conf;
			skillLst.Add(skill);
			id2SkillDic[conf.iSkillId] = skill;
			if (layer2SkillDic.TryGetValue(conf.iSkillLayer, out SkillRuntime lastSkill))
			{
				lastSkill.SetState(ESkillState.End);
			}
			layer2SkillDic[conf.iSkillLayer] = skill;
		}
		private void OnStopSkill(SkillRuntime skill)
		{
			var conf = skill.Conf;
			if (layer2SkillDic.TryGetValue(conf.iSkillLayer, out SkillRuntime layerSkill) && skill == layerSkill)
			{
				layer2SkillDic[conf.iSkillLayer] = null;
			}
			id2SkillDic.Remove(conf.iSkillId);
			skillLst.Remove(skill);
			skill.End();
			ObjectPool.Release(skill);
			OnSkillEnd?.Invoke(conf.iSkillId);
		}
	}
}