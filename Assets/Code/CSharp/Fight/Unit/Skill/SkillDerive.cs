using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Fight.Skill
{
	public class SkillDerive
	{
		public float StartTime { get; private set; }
		public float EndTime { get; private set; }
		public int BtnDeriveId { get; private set; }
		public int NextSkillId { get; private set; }
		public unsafe void Set(string str)
		{
			var dataSpan = str.AsSpan();
			var index = str.IndexOf(':');
			int count = 0;
			while (index > 0)
			{
				var data = dataSpan.Slice(0, index);
				switch (count)
				{
					case 0:
						StartTime = float.Parse(data);
						break;
					case 1:
						EndTime = float.Parse(data);
						EndTime = EndTime <= 0 ? float.MaxValue : EndTime;
						break;
					case 2:
						BtnDeriveId = int.Parse(data);
						break;
					case 3:
						NextSkillId = int.Parse(data);
						break;
				}
				count++;
				if (index + 1 < dataSpan.Length)
				{
					dataSpan = dataSpan.Slice(index + 1);
					index = str.IndexOf(':');
				}
				else
				{
					index = -1;
				}
			}
		}

		public bool CheckBtnState(Dictionary<EBtnType, EBtnStateType> dic)
		{
			var btnDeriveConf = CSVBtnDerive.Get(BtnDeriveId);
			if (btnDeriveConf != null)
			{
				var ids = btnDeriveConf.iListBtnId;
				var states = btnDeriveConf.iListBtnType;
				for (int i = 0; i < ids.Count; i++)
				{
					var id = ids[i];
					var state = states[i];
					if (!dic.TryGetValue((EBtnType)id, out EBtnStateType type) || (int)type != state)
					{
						return false;
					}
				}
			}
			else
			{
				return false;
			}
			return true;
		}
	}
}