using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 技能 --- 动画参数
/// </summary>
[CSV("Skill", "AnimParam")]
public partial class CSVAnimParam
{
	private static SqliteConnection sql;
	private static Dictionary<int, CSVAnimParam> mainDic = new Dictionary<int, CSVAnimParam>(3);
	private static List<CSVAnimParam> mainLst = new List<CSVAnimParam>(3);
	private static readonly string SQLITE_NAME = "Skill.bytes";
	private static readonly string Tab_NAME = "AnimParam";
	private static bool isInitAllData = false;

	private static CSVReader reader = new CSVReader();
	private static int totolCount = 3;

	private byte[] unSerializedBytes;
	private bool isDeserialized = false;
	static CSVAnimParam()
	{
		if (sql == null)
		{
			sql = SqlUtils.GetSqlCn(SQLITE_NAME);
		}
	}

	private CSVAnimParam() { }
	
	public static List<CSVAnimParam> GetAllLst()
	{	
		InitAllSqlDatas();
		return mainLst;
	}

	public static CSVAnimParam Get(int id)
	{
		if (isInitAllData)
		{
			return null;
		}
		CSVAnimParam result = null;
		if (!mainDic.TryGetValue(id, out result))
		{
			result = GetSqlData(id);
			if (result != null)
			{
				mainDic[id] = result;
			}
		}
		return result;
	}

	private static CSVAnimParam GetSqlData(int id)
	{
		CSVAnimParam result = null;
		try
		{
			using (var sqlCmd = sql.CreateCommand())
			{
				sqlCmd.CommandText = "select * from AnimParam where ParamId = '" + id + "'";
				var reader = sqlCmd.ExecuteReader();
				if (reader.Read())
				{
					var bytes = (byte[])reader[1];
					result = new CSVAnimParam();
					result.unSerializedBytes = bytes;
					result.m_ParamId = id;
					//result.DeserializeSelect(reader);
					mainDic[id] = result;
					mainLst.Add(result);
					result.OnPostDeserialized();
				}
				reader.Close();
			}
			return result;
		}
		catch (System.Exception)
		{
			Debug.LogError("CSVAnimParam解析错误->>>" + id);
			throw;
		}
	}

	private static void InitAllSqlDatas()
	{
		if (isInitAllData)
		{
			return;
		}
		isInitAllData = true;
		using (var sqlCmd = sql.CreateCommand())
		{
			sqlCmd.CommandText = "select * from AnimParam";
			var reader = sqlCmd.ExecuteReader();
			while (reader.Read())
			{
				var first = reader.GetString(0);
				var firstValue = int.Parse(first);
				if (!mainDic.ContainsKey(firstValue))
				{
					try
					{
						var bytes = (byte[])reader[1];
						var data = new CSVAnimParam();
						data.unSerializedBytes = bytes;
						data.m_ParamId = firstValue;
						//data.DeserializeSelect(reader);
						mainDic[data.m_ParamId] = data;
						mainLst.Add(data);
						data.OnPostDeserialized();
					}
					catch (System.Exception)
					{
						Debug.LogError("CSVAnimParam解析错误->>>" + first);
						throw;
					}	
				}
			}
			reader.Close();
			Dispose();
			OnPostAllDeserialized();
		}
	}

	private int m_ParamId;
    private float m_StartTime;
    private float m_LifeTime;
    private List<string> m_Param;
    private List<float> m_Value;
    

	public int iParamId { get { return m_ParamId; } }
    public float fStartTime { get { Deserialized(); return m_StartTime; } }
    public float fLifeTime { get { Deserialized(); return m_LifeTime; } }
    public List<string> sListParam { get { Deserialized(); return m_Param; } }
    public List<float> fListValue { get { Deserialized(); return m_Value; } }
    

	/*Other*/
	
	private void DeserializeSelect(SqliteDataReader reader)
	{
		
	}

	private void Deserialized()
	{
		if (isDeserialized)
		{
			return;
		}
		try
		{	
			reader.Write(unSerializedBytes);
			m_StartTime = reader.ReadFloat();
            m_LifeTime = reader.ReadFloat();
            m_Param = reader.ReadStringList();
            m_Value = reader.ReadFloatList();
            
			unSerializedBytes = null;
			isDeserialized = true;
			reader.Dispose();
		}
		catch (System.Exception)
		{
			isDeserialized = true;
			Debug.LogError("CSVAnimParam解析错误->>>" + m_ParamId);
			throw;
		}
	}

	private static void Dispose()
	{
	
	}
}