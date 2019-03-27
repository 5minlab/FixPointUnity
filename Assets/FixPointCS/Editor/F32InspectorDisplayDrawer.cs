using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UniRx;
using FixMath;

namespace FixMath.UniRx
{
	// InspectorDisplayDrawer 내용 갈아치워서 렌더링 규격을 F32에 맞추고 싶다
	[UnityEditor.CustomPropertyDrawer(typeof(F32ReactiveProperty))]
	public class ExtendInspectorDisplayDrawer : UnityEditor.PropertyDrawer
	{
		public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label)
		{
			string fieldName;
			bool notifyPropertyChanged;
			{
				var attr = this.attribute as InspectorDisplayAttribute;
				fieldName = (attr == null) ? "value" : attr.FieldName;
				notifyPropertyChanged = (attr == null) ? true : attr.NotifyPropertyChanged;
			}

			if (notifyPropertyChanged)
			{
				EditorGUI.BeginChangeCheck();
			}
			var targetSerializedProperty = property.FindPropertyRelative(fieldName);
			if (targetSerializedProperty == null)
			{
				UnityEditor.EditorGUI.LabelField(position, label, new GUIContent() { text = "InspectorDisplay can't find target:" + fieldName });
				if (notifyPropertyChanged)
				{
					EditorGUI.EndChangeCheck();
				}
				return;
			}
			else
			{
				EmitPropertyField(position, targetSerializedProperty, label);
			}

			if (notifyPropertyChanged)
			{
				if (EditorGUI.EndChangeCheck())
				{
					property.serializedObject.ApplyModifiedProperties(); // deserialize to field

					var paths = property.propertyPath.Split('.'); // X.Y.Z...
					var attachedComponent = property.serializedObject.targetObject;

					var targetProp = (paths.Length == 1)
						? fieldInfo.GetValue(attachedComponent)
						: GetValueRecursive(attachedComponent, 0, paths);
					if (targetProp == null) return;
					var propInfo = targetProp.GetType().GetProperty(fieldName, BindingFlags.IgnoreCase | BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					var modifiedValue = propInfo.GetValue(targetProp, null); // retrieve new value

					var methodInfo = targetProp.GetType().GetMethod("SetValueAndForceNotify", BindingFlags.IgnoreCase | BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if (methodInfo != null)
					{
						methodInfo.Invoke(targetProp, new object[] { modifiedValue });
					}
				}
				else
				{
					property.serializedObject.ApplyModifiedProperties();
				}
			}
		}

		object GetValueRecursive(object obj, int index, string[] paths)
		{
			var path = paths[index];
			var fieldInfo = obj.GetType().GetField(path, BindingFlags.IgnoreCase | BindingFlags.GetField | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

			// If array, path = Array.data[index]
			if (fieldInfo == null && path == "Array")
			{
				try
				{
					path = paths[++index];
					var m = Regex.Match(path, @"(.+)\[([0-9]+)*\]");
					var arrayIndex = int.Parse(m.Groups[2].Value);
					var arrayValue = (obj as System.Collections.IList)[arrayIndex];
					if (index < paths.Length - 1)
					{
						return GetValueRecursive(arrayValue, ++index, paths);
					}
					else
					{
						return arrayValue;
					}
				}
				catch
				{
					Debug.Log("InspectorDisplayDrawer Exception, objType:" + obj.GetType().Name + " path:" + string.Join(", ", paths));
					throw;
				}
			}
			else if (fieldInfo == null)
			{
				throw new Exception("Can't decode path, please report to UniRx's GitHub issues:" + string.Join(", ", paths));
			}

			var v = fieldInfo.GetValue(obj);
			if (index < paths.Length - 1)
			{
				return GetValueRecursive(v, ++index, paths);
			}

			return v;
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var height = base.GetPropertyHeight(property, label);
			return height;
		}

		protected virtual void EmitPropertyField(Rect position, SerializedProperty targetSerializedProperty, GUIContent label)
		{
			var tokens = targetSerializedProperty.propertyPath.Split('.').ToList();
			System.Object obj = targetSerializedProperty.serializedObject.targetObject;
			foreach (var fieldName in tokens)
			{
				var flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
				var field = obj.GetType().GetField(fieldName, flags);
				var inner = field.GetValue(obj);

				if (inner == null) { return; }
				else { obj = inner; }
				if (inner.GetType() == typeof(F32ReactiveProperty)) { break; }
			}

			var fval = obj as F32ReactiveProperty;
			var prev = fval.Value.Float;

			EditorGUI.BeginProperty(position, label, targetSerializedProperty);
			float next = EditorGUI.FloatField(position, label.text, prev);
			fval.Value = new F32(next);
			EditorGUI.EndProperty();
		}
	}
}

