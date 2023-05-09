﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public interface ITreeViewItem
{
	List<ITreeViewItem> ChildItemLst { get; }
	void Draw();
}