using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public static partial class Utility
{
	public unsafe static class Unsafe
	{
		private delegate void* ObjectToVoidPtrDelegate(object obj);
		private delegate IntPtr* ObjectToIntPtrDelegate(object obj);
		private delegate byte* ObjectToBytePtrDelegate(object obj);
		private delegate void CopyObjectDelegate(void* ptr, object obj);
		private delegate object VoidPtrToObjectDelegate(void* ptr);
		private delegate object IntPtrToObjectDelegate(IntPtr* ptr);
		private delegate object BytePtrToObjectDelegate(byte* ptr);

		private static UnsafeTool unsafeTool = new UnsafeTool();

		public static void SetValue(object target, FieldInfo field, object value)
		{
			//field.SetValue(target, value);
			var ptr = unsafeTool.ObjectToBytePtr(target);
			var offset = UnsafeUtility.GetFieldOffset(field);
			ptr += offset;
			UnsafeUtility.CopyObjectAddressToPtr(value, ptr);
		}

		[StructLayout(LayoutKind.Explicit)]
		private unsafe class UnsafeTool
		{

			[FieldOffset(0)]
			public ObjectToVoidPtrDelegate ObjectToVoidPtr;
			[FieldOffset(0)]
			public ObjectToIntPtrDelegate ObjectToIntPtr;
			[FieldOffset(0)]
			public ObjectToBytePtrDelegate ObjectToBytePtr;
			[FieldOffset(0)]
			Func<object, object> func;
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
	}
}