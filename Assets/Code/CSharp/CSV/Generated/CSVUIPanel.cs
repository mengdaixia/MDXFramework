using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// UI面板 --- 面板
/// </summary>
[CSV("UIPanel", "UIPanel")]
public partial class CSVUIPanel
{
	private static SqliteConnection sql;
	private static Dictionary<int, CSVUIPanel> mainDic = new Dictionary<int, CSVUIPanel>(9);
	private static List<CSVUIPanel> mainLst = new List<CSVUIPanel>(9);
	private static readonly string SQLITE_NAME = "UIPanel.bytes";
	private static readonly string Tab_NAME = "UIPanel";
	private static bool isInitAllData = false;

	private static CSVReader reader = new CSVReader();
	private static int totolCount = 9;

	private byte[] unSerializedBytes;
	private bool isDeserialized = false;
	static CSVUIPanel()
	{
		if (sql == null)
		{
			sql = SqlUtils.GetSqlCn(SQLITE_NAME);
		}
	}

	private CSVUIPanel() { }
	
	public static List<CSVUIPanel> GetAllLst()
	{	
		InitAllSqlDatas();
		return mainLst;
	}

	public static CSVUIPanel Get(int id)
	{
		if (isInitAllData)
		{
			return null;
		}
		CSVUIPanel result = null;
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

	private static CSVUIPanel GetSqlData(int id)
	{
		CSVUIPanel result = null;
		try
		{
			using (var sqlCmd = sql.CreateCommand())
			{
				sqlCmd.CommandText = "select * from UIPanel where PanelId = '" + id + "'";
				var reader = sqlCmd.ExecuteReader();
				if (reader.Read())
				{
					var bytes = (byte[])reader[1];
					result = new CSVUIPanel();
					result.unSerializedBytes = bytes;
					result.m_PanelId = id;
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
			Debug.LogError("CSVUIPanel解析错误->>>" + id);
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
			sqlCmd.CommandText = "select * from UIPanel";
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
						var data = new CSVUIPanel();
						data.unSerializedBytes = bytes;
						data.m_PanelId = firstValue;
						//data.DeserializeSelect(reader);
						mainDic[data.m_PanelId] = data;
						mainLst.Add(data);
						data.OnPostDeserialized();
					}
					catch (System.Exception)
					{
						Debug.LogError("CSVUIPanel解析错误->>>" + first);
						throw;
					}	
				}
			}
			reader.Close();
			Dispose();
			OnPostAllDeserialized();
		}
	}

	private int m_PanelId;
    private int m_Layer;
    private string m_Path;
    

	public int iPanelId { get { return m_PanelId; } }
    public int iLayer { get { Deserialized(); return m_Layer; } }
    public string sPath { get { Deserialized(); return m_Path; } }
    

	public EPanelId PanelId => (EPanelId)iPanelId;
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
			m_Layer = reader.ReadInt();
            m_Path = reader.ReadString();
            
			unSerializedBytes = null;
			isDeserialized = true;
			reader.Dispose();
		}
		catch (System.Exception)
		{
			isDeserialized = true;
			Debug.LogError("CSVUIPanel解析错误->>>" + m_PanelId);
			throw;
		}
	}

	private static void Dispose()
	{
	
	}
}