using Code.Fight.Operate;
using Code.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Fight
{
	public enum ESkillState
	{
		None,
		Start,
		Playing,
		End,
	}
	public class SkillRuntime : IPoolObj
	{
		private SkillManager mgr;
		private ISceneUnit owner;
		private CSVSkill skillConf;
		private ESkillState skillState;
		private float currTime;
		private EventsRunningCtr eventRunning = new EventsRunningCtr();
		public CSVSkill Conf => skillConf;
		public ESkillState SkillState => skillState;
		public void Set(SkillManager skill_mgr, ISceneUnit unit, CSVSkill conf)
		{
			mgr = skill_mgr;
			owner = unit;
			skillConf = conf;
		}
		public void SetState(ESkillState state)
		{
			skillState = state;
		}
		public void Start()
		{
			skillState = ESkillState.Playing;
			var animancer = owner.SubMgrList.Get<AnimancerManager>();
			if (animancer != null)
			{
				var animConf = CSVAnim.Get(skillConf.iAnimId);
				if (animConf != null)
				{
					var animName = animConf.sAnimName;
					var layer = Conf.iSkillLayer;
					animancer.CrossFade(animName, layer, 0.2f, animConf.bIsFromStart);
				}
			}
			AddEvts();
			currTime = 0;
			Utility.DebugX.LogError("技能" + Conf.iSkillId + "释放");
		}
		public void Update()
		{
			currTime += owner.DeltaTime();
			eventRunning.Update(currTime);
			var deriveLst = Conf.SkillDeriveLst;
			if (deriveLst.Count > 0)
			{
				for (int i = 0; i < deriveLst.Count; i++)
				{
					var derive = deriveLst[i];
					if ((currTime - derive.StartTime) * (derive.EndTime - currTime) >= 0)
					{
						var op = owner.SubMgrList.Get<OperateManager>();
						if (derive.CheckBtnState(op.CurrStateDic))
						{
							var netxSkillId = derive.NextSkillId;
							if (!mgr.IsPlaying(netxSkillId))
							{
								mgr.Play(netxSkillId, true);
							}
							return;
						}
					}
				}
			}
		}
		private void AddEvts()
		{
			eventRunning.ClearEvt();
			var animConf = CSVAnim.Get(skillConf.iAnimId);
			if (animConf != null)
			{
				var paramEvtLst = animConf.iListAnimParamEvt;
				for (int i = 0; i < paramEvtLst.Count; i++)
				{
					var conf = CSVAnimParam.Get(paramEvtLst[i]);
					var frameEvt = new FrameEvent();
					frameEvt.Time = conf.fStartTime;
					frameEvt.Event = () =>
					{
						var mgr = owner.SubMgrList.Get<AnimParamManager>();
						mgr.Start(conf.iParamId, skillConf.iSkillId);
					};
					eventRunning.AddEvent(frameEvt);
				}
			}
		}
		public void End()
		{
			var animancer = owner.SubMgrList.Get<AnimancerManager>();
			animancer.Stop(Conf.iSkillLayer);
			if (Conf != null)
			{
				//Utility.DebugX.LogError("技能" + Conf.iSkillId + "结束");
			}
			Clear();
		}
		public void Clear()
		{
			owner = null;
			mgr = null;
			skillConf = null;
			skillState = ESkillState.None;
		}
	}
}