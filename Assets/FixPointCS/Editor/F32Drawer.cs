using System;
using UnityEditor;
using UnityEngine;
using FixMath;
using System.Reflection;

namespace FixMath.Utils
{
	public abstract class AbstractF32Drawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
		{
			var obj = prop.serializedObject.targetObject;
			var fieldName = prop.name;
			var field = obj.GetType().GetField(fieldName);
			if (field == null) { return; }

			var val = field.GetValue(obj);
			var fval = (F32)Convert.ChangeType(val, typeof(F32));

			EditorGUI.BeginProperty(position, label, prop);
			DrawField(position, label, obj, field, fval);
			EditorGUI.EndProperty();
		}

		protected abstract void DrawField(Rect position, GUIContent label, UnityEngine.Object targetObject, FieldInfo field, F32 fval);
	}

	[CustomPropertyDrawer(typeof(F32))]
	public class F32Drawer : AbstractF32Drawer
	{
		protected override void DrawField(Rect position, GUIContent label, UnityEngine.Object targetObject, FieldInfo field, F32 fval)
		{
			float next = EditorGUI.FloatField(position, label.text, fval.Float);
			field.SetValue(targetObject, new F32(next));
		}
	}

	[CustomPropertyDrawer(typeof(F32RawAttribute))]
	public class F32RawDrawer : AbstractF32Drawer
	{
		protected override void DrawField(Rect position, GUIContent label, UnityEngine.Object targetObject, FieldInfo field, F32 fval)
		{
			int next = EditorGUI.IntField(position, label.text, fval.Raw);
			field.SetValue(targetObject, F32.FromRaw(next));
		}
	}
}
