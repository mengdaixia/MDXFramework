using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Object = UnityEngine.Object;

public enum EAssetLoadState
{
	None,
	Start,
	Loading,
	Finish,
}
public interface IAssetVO
{
	int RefCount { get; set; }
	EAssetLoadState State { get; }
	bool IsDependenceLoaded();
	void Start();
	void Update();
	void Finish();
	ETTask<Object> LoadAsync(string asset_name);
	Object Load(string asset_name);
	void UnLoad();
}