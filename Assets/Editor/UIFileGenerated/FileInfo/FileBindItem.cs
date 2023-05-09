using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class FileBindItem : EditorBaseView
{
	protected string findPath;
	protected string fieldName;
	protected Component fieldComp;
	protected static StringBuilder tempSb = new StringBuilder();

	public Action<FileBindItem> OnDelete;
	protected override void OnDraw()
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label(fieldComp.GetType().Name, EUtility.GUI.WHOptions(120));
		GUILayout.Label(fieldName);
		//GUILayout.Label(findPath);
		if (GUILayout.Button("删除", EUtility.GUI.ExpandWidthFalse))
		{
			OnDelete?.Invoke(this);
		}
		GUILayout.EndHorizontal();
	}
	public void Set(GameObject root, Component comp)
	{
		fieldComp = comp;
		findPath = EUtility.Go.GetRelativePath(root, comp.gameObject);
		fieldName = EUtility.Go.GetName(comp.gameObject);
	}
	public virtual void Generated(int id)
	{
		var compType = fieldComp.GetType();
		var pFieldName = FileGenerated.GetFieldName(fieldName);
		var pPropertyName = FileGenerated.GetPropertyName(fieldName, compType);

		tempSb.Append("private ").Append(compType.FullName).Append(" ").Append(pFieldName).Append(";");
		FileGenerated.WirteCache(id, EFileWriteType.Field, tempSb.ToString());
		tempSb.Clear();

		tempSb.Append("protected ").Append(compType.FullName).Append(" ").Append(pPropertyName).Append("{ get { ");
		tempSb.Append("if(").Append(pFieldName).Append(" == null){ ").Append(pFieldName).Append(" = uiObj");
		if (!string.IsNullOrEmpty(findPath))
		{
			tempSb.Append(".transform.Find(\"").Append(findPath).Append("\")");
		}
		tempSb.Append(".GetComponent<").Append(compType.FullName).Append(">(); } return ");
		tempSb.Append(pFieldName).Append(";} }");
		FileGenerated.WirteCache(id, EFileWriteType.Property, tempSb.ToString());
		tempSb.Clear();
	}
}