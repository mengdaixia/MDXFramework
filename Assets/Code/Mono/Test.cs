/************************************************* 
  *Author: 作者 
  *Date: 日期 
  *Description: 説明
**************************************************/
using Code.Msg;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
	public Transform Trans;
	public Animator animator;

	public static Socket ClientSocket;  //声明负责通信的socket
	public static int SFlag = 0;    //连接服务器成功标志
	public Thread th1;     //声明一个线程

	private unsafe void Start()
	{
		//var root = GameObject.Find("Main").transform;
		//var field = typeof(Test).GetField("Trans", BindingFlags.Public | BindingFlags.Instance);
		//byte* ptr = UnsafeTool.unsafeTool.ObjectToBytePtr(this);
		//ptr += sizeof(IntPtr*);
		////取得字段的偏移量	
		//int offset = UnsafeUtility.GetFieldOffset(field);
		//ptr += offset;
		////取得字段的值
		//*(IntPtr*)ptr = (IntPtr)UnsafeTool.unsafeTool.ObjectToIntPtr(root);

		//var b = new TestClassB();
		//var a = new TestClassA();
		//var field = typeof(TestClassA).GetField("b", BindingFlags.Public | BindingFlags.Instance);
		//var ptr = UnsafeTool.unsafeTool.ObjectToIntPtr(a);
		////取得字段的偏移量	
		//int offset = UnsafeUtility.GetFieldOffset(field);
		//ptr += offset;
		//UnsafeUtility.CopyObjectAddressToPtr(root, ptr);

		//Trans = a.b;
		//animator.enabled = false;
		//animator.CrossFade("1", 0);

		//ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);     //声明负责通信的套接字

		//IPAddress IP = IPAddress.Parse("127.0.0.1");      //获取设置的IP地址
		//IPEndPoint iPEndPoint = new IPEndPoint(IP, 8080);    //指定的端口号和服务器的ip建立一个IPEndPoint对象

		//ClientSocket.Connect(iPEndPoint);       //用socket对象的Connect()方法以上面建立的IPEndPoint对象做为参数，向服务器发出连接请求

		////开启一个线程接收数据
		//th1 = new Thread(Receive);
		//th1.IsBackground = true;
		//th1.Start();

	}
	void Receive()
	{
		Socket socketRec = ClientSocket;

		while (true)
		{
			//5.接收数据
			byte[] receive = new byte[1024];
			ClientSocket.Receive(receive);  //调用Receive()接收字节数据

			//6.打印接收数据
			if (receive.Length > 0)
			{
				Debug.LogError(Encoding.ASCII.GetString(receive));
			}
		}
	}
	private Transform transhelp;

	private void Update()
	{
		//animator.Update(Time.deltaTime);
		//if (Input.GetKeyDown(KeyCode.F))
		//{
		//	ClientSocket.Send(Encoding.ASCII.GetBytes("Hello"));
		//}
	}

	[SerializeField]
	private Transform a;
	[SerializeField]
	private Transform b;
	[SerializeField]
	private Transform c;
	private void OnDrawGizmos()
	{
		if (a == null)
		{
			return;
		}

		Debug.DrawLine(a.position, b.position, Color.white);
		Debug.DrawLine(a.position, c.position, Color.yellow);
		var v1 = b.position - a.position;
		var v2 = c.position - a.position;
		var v3 = Vector3.Project(v1, v2);
		Debug.DrawLine(a.position, a.position + v3, Color.red);
	}
	private void LateUpdate()
	{
		//var cube = GameObject.Find("YYYYY");
		//var rect = GameObject.Find("XXXXX");
		//var uiCamera = GameObject.Find("Camera").GetComponent<Camera>();
		//var screenPos = RectTransformUtility.WorldToScreenPoint(uiCamera, rect.transform.position);
		//var ray = RectTransformUtility.ScreenPointToRay(Camera.main, screenPos);
		//var rayDir = ray.direction;
		////if (transhelp == null)
		////{
		////	transhelp = new GameObject("help").transform;
		////	transhelp.position = Camera.main.transform.position;
		////	transhelp.transform.forward = rayDir;
		////	Camera.main.transform.parent = transhelp;
		////}
		////transhelp.LookAt(cube.transform.position);

		//var newDir = -(Camera.main.transform.position - cube.transform.position).normalized;
		//var forward = Camera.main.transform.forward;
		//var inverse = Quaternion.Inverse(Quaternion.LookRotation(rayDir));
		//var localOff = inverse * Quaternion.LookRotation(forward);
		//Camera.main.transform.rotation = Quaternion.LookRotation(newDir) * localOff;

		//var localORay = inverse * rayDir;
		//var rotOff = Quaternion.FromToRotation(newDir, localORay);

		//Camera.main.transform.rotation = Quaternion.Inverse(rotOff);

		//Camera.main.transform.forward = Quaternion.FromToRotation(ray.direction, forward) * newDir;
		//Camera.main.transform.LookAt(cube.transform);
		//Camera.main.transform.forward = newDir;
	}
	private void OnDestroy()
	{
		//ClientSocket?.Close();
		//ClientSocket?.Dispose();
		//th1?.Abort();
	}

	public class TestClassA
	{
		public Transform b;
	}
	public class TestClassB
	{
		public int a;
	}
}
[StructLayout(LayoutKind.Explicit)]
public unsafe class UnsafeTool
{
	public static UnsafeTool unsafeTool = new UnsafeTool();

	public delegate void* ObjectToVoidPtrDelegate(object obj);
	public delegate IntPtr* ObjectToIntPtrDelegate(object obj);
	public delegate byte* ObjectToBytePtrDelegate(object obj);
	public delegate void CopyObjectDelegate(void* ptr, object obj);


	[FieldOffset(0)]
	public ObjectToVoidPtrDelegate ObjectToVoidPtr;
	[FieldOffset(0)]
	public ObjectToIntPtrDelegate ObjectToIntPtr;
	[FieldOffset(0)]
	public ObjectToBytePtrDelegate ObjectToBytePtr;
	[FieldOffset(0)]
	Func<object, object> func;

	public delegate object VoidPtrToObjectDelegate(void* ptr);
	public delegate object IntPtrToObjectDelegate(IntPtr* ptr);
	public delegate object BytePtrToObjectDelegate(byte* ptr);

	[FieldOffset(8)]
	public VoidPtrToObjectDelegate VoidPtrToObject;
	[FieldOffset(8)]
	public IntPtrToObjectDelegate IntPtrToObject;
	[FieldOffset(8)]
	public BytePtrToObjectDelegate BytePtrToObject;
	[FieldOffset(8)]
	Func<object, object> func2;


	[FieldOffset(16)]
	public CopyObjectDelegate SetObject;
	[FieldOffset(16)]
	CopyObjectDelegate_ func3;
	delegate void CopyObjectDelegate_(void** ptr, void* obj);

	public UnsafeTool()
	{
		func = Out;
		func2 = Out;
		func3 = _CopyObject;
	}
	object Out(object o) { return o; }
	void _CopyObject(void** ptr, void* obj)
	{
		*ptr = obj;
	}
}