using UnityEngine;
using FixMath;

namespace FixMath.Extensions
{
	public static class F32Vec2Extensions
	{
		public static Vector2 ToVector2(this F32Vec2 v)
		{
			return new Vector2(v.X.Float, v.Y.Float);
		}
	}

	public static class F32Vec3Extensions
	{
		public static Vector3 ToVector3(this F32Vec3 v)
		{
			return new Vector3(v.X.Float, v.Y.Float, v.Z.Float);
		}
	}

	public static class F32Vec4Extensions
	{
		public static Vector3 ToVector4(this F32Vec4 v)
		{
			return new Vector4(v.X.Float, v.Y.Float, v.Z.Float, v.W.Float);
		}
	}

	public static class F64Vec2Extensions
	{
		public static Vector2 ToVector2(this F64Vec2 v)
		{
			return new Vector2(v.X.Float, v.Y.Float);
		}
	}

	public static class F64Vec3Extensions
	{
		public static Vector3 ToVector3(this F64Vec3 v)
		{
			return new Vector3(v.X.Float, v.Y.Float, v.Z.Float);
		}
	}

	public static class F64Vec4Extensions
	{
		public static Vector4 ToVector4(this F64Vec4 v)
		{
			return new Vector4(v.X.Float, v.Y.Float, v.Z.Float, v.W.Float);
		}
	}

	public static class F64QuatExtensions
	{
		public static Quaternion ToQuaternion(this F64Quat v)
		{
			return new Quaternion(v.X.Float, v.Y.Float, v.Z.Float, v.W.Float);
		}
	}
}
