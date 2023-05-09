using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
	[UIID(EPanelId.UITest)]
	public class UITest : UIBasePanel
	{
		[UIWidget("Image")]
		private Image img_test;

		protected override void OnInit()
		{
			base.OnInit();
			img_test.color = Color.yellow;
		}
		protected override void OnDestroy()
		{
			base.OnDestroy();
		}
	}
}