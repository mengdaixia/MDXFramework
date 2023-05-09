using Code.Loader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class LoadQueue
{
	private List<IAssetVO> waitJobLst = new List<IAssetVO>();

	private List<IAssetVO> bundleWorkLst = new List<IAssetVO>();
	private List<IAssetVO> assetWorkLst = new List<IAssetVO>();

	public void Update()
	{
		UpdateWaitJob();
		UpdateWorkJob();
	}
	private void UpdateWaitJob()
	{
		for (int i = 0; i < waitJobLst.Count; i++)
		{
			var job = waitJobLst[i];
			if (job.IsDependenceLoaded())
			{
				waitJobLst.RemoveAt(i);
				if (job is BundleVO)
				{
					bundleWorkLst.Add(job);
				}
				else
				{
					assetWorkLst.Add(job);
				}
				i--;
			}
		}
	}
	private void UpdateWorkJob()
	{
		for (int i = 0; i < bundleWorkLst.Count; i++)
		{
			var job = bundleWorkLst[i];
			switch (job.State)
			{
				case EAssetLoadState.Start:
					job.Start();
					break;
				case EAssetLoadState.Loading:
					job.Update();
					break;
				case EAssetLoadState.Finish:
					job.Finish();
					bundleWorkLst.RemoveAt(i);
					i--;
					break;
			}
		}
		int workCount = 0;
		for (int i = 0; i < assetWorkLst.Count; i++)
		{
			var job = assetWorkLst[i];
			switch (job.State)
			{
				case EAssetLoadState.Start:
					job.Start();
					workCount++;
					break;
				case EAssetLoadState.Loading:
					job.Update();
					workCount++;
					break;
				case EAssetLoadState.Finish:
					job.Finish();
					assetWorkLst.RemoveAt(i);
					i--;
					break;
			}
			if (workCount >= 4)
			{
				break;
			}
		} 
	}
	public void EnqueueJob(IAssetVO vo)
	{
		waitJobLst.Add(vo);
	}
}