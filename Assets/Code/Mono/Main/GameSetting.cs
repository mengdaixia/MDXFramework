using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public enum EGameType
{
	Editor,
	Normal,
	Update,
}
public class GameSetting : MonoBehaviour
{
	public EGameType GameType;

	public static GameSetting Instance { get; private set; }
	private void Awake()
	{
		Instance = this;
	}
}