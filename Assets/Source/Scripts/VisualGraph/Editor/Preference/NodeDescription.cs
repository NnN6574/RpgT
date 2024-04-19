using System;
using LevelsConstructor.New.Editor.Elements;
using Source.Scripts.VisualGraph.Editor.Elements;
using Tools.Extensions;
using UnityEditor;
using UnityEngine;

namespace LevelsConstructor.New.Editor.Preference
{
	[Serializable]
	public class NodeDescription
	{
		[SerializeField] private string _name = "Node";
		[SerializeField] private string _contextMenuPath = "Add node";
		[SerializeField] private string _searchMenuPath = "Node";
		[SerializeField] private string _typeName;
		[SerializeField] private MonoScript _nodeScript;

		public string Name => _name;
		public string ContextMenuPath => _contextMenuPath;
		public string SearchMenuPath => _searchMenuPath;

		public string TypeName => _typeName;
		public MonoScript NodeScript
		{
			get => _nodeScript;
			set
			{
				_nodeScript = value;
				if (_nodeScript == null) _typeName = "";
				else _typeName = GenerateTypeName();
			}
		}

		public void Validate()
		{
			if (_nodeScript == null) return;
			string typeName = GenerateTypeName();
			Type type = Type.GetType(typeName);
			if (type != null && type.Extends(typeof(LCNode)))
			{
				_typeName = typeName;
				if (string.IsNullOrWhiteSpace(_name)) _name = type.Name;
				if (string.IsNullOrWhiteSpace(_searchMenuPath)) _searchMenuPath = "Nodes/" + _name;
			}
			else
			{
				_typeName = "";
				_nodeScript = null;
			}
		}

		private string GenerateTypeName()
		{
			if (_nodeScript == null) return null;
			string text = _nodeScript.text;
			string[] words = text.Split(' ', '\n', '\t', '\r', '{');
			string @namespace = GetNextWord(words, "namespace");
			string @class = GetNextWord(words, "class");
			string result;
			if (!string.IsNullOrWhiteSpace(@namespace)) result = @namespace + '.' + @class;
			else result = @class;
			
			return result;
		}

		private static string GetNextWord(string[] words, string @from)
		{
			for (int i = 0; i < words.Length; i++)
			{
				string word = words[i];
				if (word.Equals(from)) return words[i + 1];
			}

			return "";
		}
	}
}