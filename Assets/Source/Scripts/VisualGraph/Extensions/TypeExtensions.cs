using System;
using UnityEngine;

namespace Tools.Extensions
{
	public static class TypeExtensions
	{
		public static bool Extends(this Type type, Type @base)
		{
			if (type == null) return false;
			
			Type baseType = type.BaseType;
			while (baseType != null)
			{
				if (baseType == @base) return true;
				baseType = baseType.BaseType;
			}

			Debug.Log("false");
			return false;
		}
	}
}