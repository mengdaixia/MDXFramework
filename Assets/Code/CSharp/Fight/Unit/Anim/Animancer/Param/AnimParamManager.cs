using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Fight
{
	public class AnimParamManager : SubManager
	{
		private Action<int> OnSkillEndFunc;
		private List<AnimRuntimeParam> paramLst = new List<AnimRuntimeParam>();
		private Dictionary<int, AnimRuntimeParam> skill2ParamDic = new Dictionary<int, AnimRuntimeParam>();
		protected override void OnInit()
		{
			OnSkillEndFunc = OnSkillEndFunc == null ? OnSkillEndHandler : OnSkillEndFunc;
			var skill = owner.SubMgrList.Get<SkillManager>();
			if (skill != null)
			{
				skill.OnSkillEnd += OnSkillEndFunc;
			}
		}
		protected override void OnDestroy(bool is_dead)
		{
			var skill = owner.SubMgrList.Get<SkillManager>();
			if (skill != null)
			{
				skill.OnSkillEnd -= OnSkillEndFunc;
			}
		}
		public void Start(int id, int skill_id)
		{
			var conf = CSVAnimParam.Get(id);
			if (conf != null)
			{
				var param = new AnimRuntimeParam();
				param.Set(owner, conf);
				paramLst.Add(param);
				skill2ParamDic[skill_id] = param;
			}
		}
		protected override void OnUpdate()
		{
			for (int i = paramLst.Count - 1; i >= 0; i--)
			{
				var param = paramLst[i];
				param.Update();
				if (param.IsEnd)
				{
					paramLst.RemoveAt(i);
				}
			}
		}
		public void OnSkillEndHandler(int id)
		{
			if (skill2ParamDic.TryGetValue(id, out AnimRuntimeParam param))
			{
				skill2ParamDic.Remove(id);
				paramLst.Remove(param);
			}
		}
	}
}