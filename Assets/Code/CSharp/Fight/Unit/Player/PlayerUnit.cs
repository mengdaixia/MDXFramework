using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Fight
{
	public class PlayerUnit : ISceneUnit, IModelConf
	{
		private int modelId;
		private UnitAttr unitAttr = new UnitAttr();
		private SubManagerList subMgrList = new SubManagerList();

		public long UnitId { get; set; }
		public Transform Root { get; set; }
		public ISubManagerList SubMgrList => subMgrList;
		public UnitAttr Attr => unitAttr;
		public bool IsAlive => Attr[EAttrType.Hp] > 0;
		public EUnitCampType UnitCamp => EUnitCampType.Character;
		public ESceneUnitType UnitType => ESceneUnitType.Character;
		public GameObject ModelGo { get; private set; }
		public int ModelId => modelId;

		public void Set(int model_id)
		{
			modelId = model_id;
		}
		public void Init()
		{
			subMgrList.Init(this);
			var defDic = new Dictionary<int, float>()
			{
				{ (int)EAttrType.Hp, 100 },
				{ (int)EAttrType.TimeRunningSpeed, 1 },
			};
			unitAttr.Init(defDic);
			var modelConf = CSVModel.Get(modelId);
			ModelGo = GameResLoader.Instance.GetInstance(modelConf.sPath);
			Utility.Trans.SetParent(ModelGo.transform, Root);
			Utility.Trans.SetParent(Root, SceneStaticSetting.CharacterRoot);
		}
		public void DestroySelf(bool is_dead)
		{
			subMgrList.Destroy(is_dead);
			GameResLoader.Instance.Recycle(ModelGo);
		}
	}
}