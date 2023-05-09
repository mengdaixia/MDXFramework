using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 技能 --- 技能
/// </summary>
[CSV("Skill", "Skill")]
public partial class CSVSkill
{
	private static SqliteConnection sql;
	private static Dictionary<int, CSVSkill> mainDic = new Dictionary<int, CSVSkill>(9);
	private static List<CSVSkill> mainLst = new List<CSVSkill>(9);
	private static readonly string SQLITE_NAME = "Skill.bytes";
	private static readonly string Tab_NAME = "Skill";
	private static bool isInitAllData = false;

	private static CSVReader reader = new CSVReader();
	private static int totolCount = 9;

	private byte[] unSerializedBytes;
	private bool isDeserialized = false;
	static CSVSkill()
	{
		if (sql == null)
		{
			sql = SqlUtils.GetSqlCn(SQLITE_NAME);
		}
	}

	private CSVSkill() { }
	
	public static List<CSVSkill> GetAllLst()
	{	
		InitAllSqlDatas();
		return mainLst;
	}

	public static CSVSkill Get(int id)
	{
		if (isInitAllData)
		{
			return null;
		}
		CSVSkill result = null;
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

	private static CSVSkill GetSqlData(int id)
	{
		CSVSkill result = null;
		try
		{
			using (var sqlCmd = sql.CreateCommand())
			{
				sqlCmd.CommandText = "select * from Skill where SkillId = '" + id + "'";
				var reader = sqlCmd.ExecuteReader();
				if (reader.Read())
				{
					var bytes = (byte[])reader[1];
					result = new CSVSkill();
					result.unSerializedBytes = bytes;
					result.m_SkillId = id;
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
			Debug.LogError("CSVSkill解析错误->>>" + id);
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
			sqlCmd.CommandText = "select * from Skill";
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
						var data = new CSVSkill();
						data.unSerializedBytes = bytes;
						data.m_SkillId = firstValue;
						//data.DeserializeSelect(reader);
						mainDic[data.m_SkillId] = data;
						mainLst.Add(data);
						data.OnPostDeserialized();
					}
					catch (System.Exception)
					{
						Debug.LogError("CSVSkill解析错误->>>" + first);
						throw;
					}	
				}
			}
			reader.Close();
			Dispose();
			OnPostAllDeserialized();
		}
	}

	private int m_SkillId;
    private int m_AnimId;
    private int m_SkillLayer;
    private List<string> m_SkillBtnDerive;
    

	public int iSkillId { get { return m_SkillId; } }
    public int iAnimId { get { Deserialized(); return m_AnimId; } }
    public int iSkillLayer { get { Deserialized(); return m_SkillLayer; } }
    public List<string> sListSkillBtnDerive { get { Deserialized(); return m_SkillBtnDerive; } }
    

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
			m_AnimId = reader.ReadInt();
            m_SkillLayer = reader.ReadInt();
            m_SkillBtnDerive = reader.ReadStringList();
            
			unSerializedBytes = null;
			isDeserialized = true;
			reader.Dispose();
		}
		catch (System.Exception)
		{
			isDeserialized = true;
			Debug.LogError("CSVSkill解析错误->>>" + m_SkillId);
			throw;
		}
	}

	private static void Dispose()
	{
	
	}
}