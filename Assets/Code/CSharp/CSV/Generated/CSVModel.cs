using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 模型 --- 模型
/// </summary>
[CSV("Model", "Model")]
public partial class CSVModel
{
	private static SqliteConnection sql;
	private static Dictionary<int, CSVModel> mainDic = new Dictionary<int, CSVModel>(9);
	private static List<CSVModel> mainLst = new List<CSVModel>(9);
	private static readonly string SQLITE_NAME = "Model.bytes";
	private static readonly string Tab_NAME = "Model";
	private static bool isInitAllData = false;

	private static CSVReader reader = new CSVReader();
	private static int totolCount = 9;

	private byte[] unSerializedBytes;
	private bool isDeserialized = false;
	static CSVModel()
	{
		if (sql == null)
		{
			sql = SqlUtils.GetSqlCn(SQLITE_NAME);
		}
	}

	private CSVModel() { }
	
	public static List<CSVModel> GetAllLst()
	{	
		InitAllSqlDatas();
		return mainLst;
	}

	public static CSVModel Get(int id)
	{
		if (isInitAllData)
		{
			return null;
		}
		CSVModel result = null;
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

	private static CSVModel GetSqlData(int id)
	{
		CSVModel result = null;
		try
		{
			using (var sqlCmd = sql.CreateCommand())
			{
				sqlCmd.CommandText = "select * from Model where ModelId = '" + id + "'";
				var reader = sqlCmd.ExecuteReader();
				if (reader.Read())
				{
					var bytes = (byte[])reader[1];
					result = new CSVModel();
					result.unSerializedBytes = bytes;
					result.m_ModelId = id;
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
			Debug.LogError("CSVModel解析错误->>>" + id);
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
			sqlCmd.CommandText = "select * from Model";
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
						var data = new CSVModel();
						data.unSerializedBytes = bytes;
						data.m_ModelId = firstValue;
						//data.DeserializeSelect(reader);
						mainDic[data.m_ModelId] = data;
						mainLst.Add(data);
						data.OnPostDeserialized();
					}
					catch (System.Exception)
					{
						Debug.LogError("CSVModel解析错误->>>" + first);
						throw;
					}	
				}
			}
			reader.Close();
			Dispose();
			OnPostAllDeserialized();
		}
	}

	private int m_ModelId;
    private string m_Path;
    private string m_AnimatorName;
    

	public int iModelId { get { return m_ModelId; } }
    public string sPath { get { Deserialized(); return m_Path; } }
    public string sAnimatorName { get { Deserialized(); return m_AnimatorName; } }
    

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
			m_Path = reader.ReadString();
            m_AnimatorName = reader.ReadString();
            
			unSerializedBytes = null;
			isDeserialized = true;
			reader.Dispose();
		}
		catch (System.Exception)
		{
			isDeserialized = true;
			Debug.LogError("CSVModel解析错误->>>" + m_ModelId);
			throw;
		}
	}

	private static void Dispose()
	{
	
	}
}