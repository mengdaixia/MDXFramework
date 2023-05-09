using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 技能 --- 动画
/// </summary>
[CSV("Skill", "Anim")]
public partial class CSVAnim
{
	private static SqliteConnection sql;
	private static Dictionary<int, CSVAnim> mainDic = new Dictionary<int, CSVAnim>(3);
	private static List<CSVAnim> mainLst = new List<CSVAnim>(3);
	private static readonly string SQLITE_NAME = "Skill.bytes";
	private static readonly string Tab_NAME = "Anim";
	private static bool isInitAllData = false;

	private static CSVReader reader = new CSVReader();
	private static int totolCount = 3;

	private byte[] unSerializedBytes;
	private bool isDeserialized = false;
	static CSVAnim()
	{
		if (sql == null)
		{
			sql = SqlUtils.GetSqlCn(SQLITE_NAME);
		}
	}

	private CSVAnim() { }
	
	public static List<CSVAnim> GetAllLst()
	{	
		InitAllSqlDatas();
		return mainLst;
	}

	public static CSVAnim Get(int id)
	{
		if (isInitAllData)
		{
			return null;
		}
		CSVAnim result = null;
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

	private static CSVAnim GetSqlData(int id)
	{
		CSVAnim result = null;
		try
		{
			using (var sqlCmd = sql.CreateCommand())
			{
				sqlCmd.CommandText = "select * from Anim where AnimId = '" + id + "'";
				var reader = sqlCmd.ExecuteReader();
				if (reader.Read())
				{
					var bytes = (byte[])reader[1];
					result = new CSVAnim();
					result.unSerializedBytes = bytes;
					result.m_AnimId = id;
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
			Debug.LogError("CSVAnim解析错误->>>" + id);
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
			sqlCmd.CommandText = "select * from Anim";
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
						var data = new CSVAnim();
						data.unSerializedBytes = bytes;
						data.m_AnimId = firstValue;
						//data.DeserializeSelect(reader);
						mainDic[data.m_AnimId] = data;
						mainLst.Add(data);
						data.OnPostDeserialized();
					}
					catch (System.Exception)
					{
						Debug.LogError("CSVAnim解析错误->>>" + first);
						throw;
					}	
				}
			}
			reader.Close();
			Dispose();
			OnPostAllDeserialized();
		}
	}

	private int m_AnimId;
    private string m_AnimName;
    private bool m_IsFromStart;
    private List<int> m_AnimParamEvt;
    

	public int iAnimId { get { return m_AnimId; } }
    public string sAnimName { get { Deserialized(); return m_AnimName; } }
    public bool bIsFromStart { get { Deserialized(); return m_IsFromStart; } }
    public List<int> iListAnimParamEvt { get { Deserialized(); return m_AnimParamEvt; } }
    

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
			m_AnimName = reader.ReadString();
            m_IsFromStart = reader.ReadBool();
            m_AnimParamEvt = reader.ReadIntList();
            
			unSerializedBytes = null;
			isDeserialized = true;
			reader.Dispose();
		}
		catch (System.Exception)
		{
			isDeserialized = true;
			Debug.LogError("CSVAnim解析错误->>>" + m_AnimId);
			throw;
		}
	}

	private static void Dispose()
	{
	
	}
}