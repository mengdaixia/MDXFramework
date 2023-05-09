/************************************************* 
  *Author: 作者 
  *Date: 日期 
  *Description: 説明
**************************************************/
using Code;
using Code.Fight;
using Code.Mgr;
using Code.UI;
using System.Collections.Generic;
using UnityEngine;

public class GameRunning : MonoBehaviour
{
	private RunningMgrList mgrLst = new RunningMgrList();

	private void Awake()
	{
		SingletonEvent.OnSingletonInit += OnSingletonInitHandler;
		Utility.Go.DontDestroy(gameObject);
		Utility.Go.DontDestroy(Utility.CameraX.main.gameObject);
	}
	private void Start()
	{
		UIManager.Instance.Show<UITest>();
		var player = Utility.Unit.CreatePlayerUnit(1, Vector3.zero, Quaternion.identity);
		animancer = player.SubMgrList.Get<AnimancerManager>();
		skill = player.SubMgrList.Get<SkillManager>();
		skill.Play(1);

		CameraManager.Instance.SetTarget(player.Root);
	}
	private void OnEnable()
	{

	}
	private void FixedUpdate()
	{
		Utility.Time.SetDeltaTime(Time.fixedDeltaTime);
		UpdateEvent.OnFixedUpdate?.Invoke();
		mgrLst.FixedUpdate();
	}
	private AnimancerManager animancer;
	private SkillManager skill;
	private void Update()
	{
		//if (Input.GetKeyDown(KeyCode.A))
		//{
		//	animancer.SetParam("Speed", 0f);
		//}
		//if (Input.GetKeyDown(KeyCode.S))
		//{
		//	animancer.SetParam("Speed", 0.3f);
		//}
		//if (Input.GetKeyDown(KeyCode.D))
		//{
		//	animancer.SetParam("Speed", 0.6f);
		//}
		//if (Input.GetKeyDown(KeyCode.F))
		//{
		//	animancer.SetParam("Speed", 1f);
		//}
		Utility.Time.SetDeltaTime(Time.deltaTime);
		UpdateEvent.OnUpdate?.Invoke();
		mgrLst.Update();
	}
	private void LateUpdate()
	{
		UpdateEvent.OnLateUpdate?.Invoke();
		mgrLst.LateUpdate();
	}
	private void OnDisable()
	{

	}
	private void OnDestroy()
	{
		mgrLst.Destroy();
		SingletonEvent.OnSingletonInit -= OnSingletonInitHandler;
	}
	private void OnSingletonInitHandler(object singleton)
	{
		if (singleton is IRunningMgr mgr)
		{
			mgrLst.AddMgr(mgr);
		}
	}
}