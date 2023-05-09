using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 技能 --- 按键派生
/// </summary>
[CSV("Skill", "BtnDerive")]
public partial class CSVBtnDerive
{
	private static SqliteConnection sql;
	private static Dictionary<int, CSVBtnDerive> mainDic = new Dictionary<int, CSVBtnDerive>(5);
	private static List<CSVBtnDerive> mainLst = new List<CSVBtnDerive>(5);
	private static readonly string SQLITE_NAME = "Skill.bytes";
	private static readonly string Tab_NAME = "BtnDerive";
	private static bool isInitAllData = false;

	private static CSVReader reader = new CSVReader();
	private static int totolCount = 5;

	private byte[] unSerializedBytes;
	private bool isDeserialized = false;
	static CSVBtnDerive()
	{
		if (sql == null)
		{
			sql = SqlUtils.GetSqlCn(SQLITE_NAME);
		}
	}

	private CSVBtnDerive() { }
	
	public static List<CSVBtnDerive> GetAllLst()
	{	
		InitAllSqlDatas();
		return mainLst;
	}

	public static CSVBtnDerive Get(int id)
	{
		if (isInitAllData)
		{
			return null;
		}
		CSVBtnDerive result = null;
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

	private static CSVBtnDerive GetSqlData(int id)
	{
		CSVBtnDerive result = null;
		try
		{
			using (var sqlCmd = sql.CreateCommand())
			{
				sqlCmd.CommandText = "select * from BtnDerive where BtnDeriveId = '" + id + "'";
				var reader = sqlCmd.ExecuteReader();
				if (reader.Read())
				{
					var bytes = (byte[])reader[1];
					result = new CSVBtnDerive();
					result.unSerializedBytes = bytes;
					result.m_BtnDeriveId = id;
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
			Debug.LogError("CSVBtnDerive解析错误->>>" + id);
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
			sqlCmd.CommandText = "select * from BtnDerive";
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
						var data = new CSVBtnDerive();
						data.unSerializedBytes = bytes;
						data.m_BtnDeriveId = firstValue;
						//data.DeserializeSelect(reader);
						mainDic[data.m_BtnDeriveId] = data;
						mainLst.Add(data);
						data.OnPostDeserialized();
					}
					catch (System.Exception)
					{
						Debug.LogError("CSVBtnDerive解析错误->>>" + first);
						throw;
					}	
				}
			}
			reader.Close();
			Dispose();
			OnPostAllDeserialized();
		}
	}

	private int m_BtnDeriveId;
    private List<int> m_BtnId;
    private List<int> m_BtnType;
    

	public int iBtnDeriveId { get { return m_BtnDeriveId; } }
    public List<int> iListBtnId { get { Deserialized(); return m_BtnId; } }
    public List<int> iListBtnType { get { Deserialized(); return m_BtnType; } }
    

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
			m_BtnId = reader.ReadIntList();
            m_BtnType = reader.ReadIntList();
            
			unSerializedBytes = null;
			isDeserialized = true;
			reader.Dispose();
		}
		catch (System.Exception)
		{
			isDeserialized = true;
			Debug.LogError("CSVBtnDerive解析错误->>>" + m_BtnDeriveId);
			throw;
		}
	}

	private static void Dispose()
	{
	
	}
}