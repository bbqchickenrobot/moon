/*
 * GlobalInfo.cs.
 *
 * Contact:
 *   Moonlight List (moonlight-list@lists.ximian.com)
 *
 * Copyright 2008 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 * 
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

class GlobalInfo : MemberInfo {
	private List<FieldInfo> dependency_properties;
	private List<MethodInfo> cppmethods_to_bind;
	private List<TypeInfo> dependency_objects;
	
	/// <value>
	/// A list of all the types that inherits from DependencyObject
	/// </value>
	public List<TypeInfo> GetDependencyObjects (GlobalInfo all) {
		if (dependency_objects == null) {
			dependency_objects = new List<TypeInfo> ();
			
			foreach (MemberInfo member in Children.Values) {
				TypeInfo type = member as TypeInfo;
				TypeInfo current, parent;
				bool is_do = false;
				int limit = 20;
				
				if (type == null)
					continue;
				
				if (type.IsEnum || type.IsStruct)
					continue;
				
				current = type;
				
				while (limit-- > 0) {
					if (current.Base == null || string.IsNullOrEmpty (current.Base.Value))
						break;
					
					if (!all.Children.ContainsKey (current.Base.Value))
						continue;
					
					parent = all.Children [current.Base.Value] as TypeInfo;
					
					if (parent == null)
						break;
					
					if (parent.Name == "DependencyObject") {
						is_do = true;
						break;
					}
				
					current = parent;
				}
				
			//	if (limit <= 0)
			//		throw new Exception (string.Format ("Infinite loop while checking if '{0}' inherits from DependencyObject.", type.FullName));
				
				if (is_do)
					dependency_objects.Add (type);
			}
			
			dependency_objects.Sort (new Members.MembersSortedByManagedFullName <TypeInfo> ());
		}
		return dependency_objects;
	}
	
	public List<FieldInfo> DependencyProperties {
		get {
			if (dependency_properties == null) {
				// Check annotations against a list of known properties
				// to catch typos (DefaulValue, etc).
				Dictionary<string, string> known_annotations = new Dictionary <string, string> ();
				
				known_annotations.Add ("ReadOnly", null);
				known_annotations.Add ("AlwaysChange", null);
				known_annotations.Add ("Version", null);
				known_annotations.Add ("PropertyType", null);
				known_annotations.Add ("AutoCreateValue", null);
				known_annotations.Add ("DefaultValue", null);
				known_annotations.Add ("Access", null);
				known_annotations.Add ("ManagedAccess", null);
				known_annotations.Add ("Nullable", null);
				known_annotations.Add ("Attached", null);
				known_annotations.Add ("ManagedDeclaringType", null);
				known_annotations.Add ("ManagedPropertyType", null);
				known_annotations.Add ("ManagedFieldAccess", null);
				known_annotations.Add ("ManagedAccessorAccess", null);
				known_annotations.Add ("ManagedGetterAccess", null);
				known_annotations.Add ("ManagedSetterAccess", null);
				known_annotations.Add ("GenerateGetter", null);
				known_annotations.Add ("GenerateSetter", null);
				known_annotations.Add ("GenerateAccessors", null);
				known_annotations.Add ("GenerateManagedDP", null);
				known_annotations.Add ("Validator", null);
				
				dependency_properties = new List<FieldInfo>  ();
				foreach (MemberInfo member in Children.Values) {
					TypeInfo type = member as TypeInfo;
					
					if (type == null)
						continue;
					
					foreach (MemberInfo member2 in member.Children.Values) {
						FieldInfo field = member2 as FieldInfo;
						
						if (field == null)
							continue;
						
						if (field.FieldType == null || field.FieldType.Value != "int")
							continue;
						
						if (!field.IsStatic)
							continue;
						
						if (!field.Name.EndsWith ("Property")) {
							Console.WriteLine ("GenerateDPs: Found the static field {0} which returns an int, but the property name doesn't end with 'Property' (ignore this warning if the field isn't supposed to be a DP).", field.FullName);
							continue;
						}
						
						dependency_properties.Add (field);
						
						foreach (Annotation p in field.Annotations.Values) {
							if (!known_annotations.ContainsKey (p.Name))
								Console.WriteLine ("The field {0} in {3} has an unknown property: '{1}' = '{2}'", field.FullName, p.Name, p.Value, Path.GetFileName (field.Header));
						}
					}
				}
				dependency_properties.Sort (new Members.MembersSortedByFullName <FieldInfo> ());
			}
			return dependency_properties;
		}
	}
	
	
	public List<MethodInfo> CPPMethodsToBind {
		get {
			if (cppmethods_to_bind == null) {
				cppmethods_to_bind = new List<MethodInfo> ();
				foreach (MemberInfo member1 in Children.Values) {
					TypeInfo type = member1 as TypeInfo;
					if (type == null)
						continue;
					
					foreach (MemberInfo member2 in type.Children.Values) {
						MethodInfo method = member2 as MethodInfo;
						if (method == null)
							continue;
						if (method.Parent == null) {
							Console.WriteLine ("The method {0} in type {1} does not have its parent set.", method.Name, type.Name);
							continue;
						}
						if (!method.Annotations.ContainsKey ("GenerateCBinding"))
							continue;
						cppmethods_to_bind.Add (method);
					}
				}
				cppmethods_to_bind.Sort (new Members.MembersSortedByFullName <MethodInfo> ());
			}
			return cppmethods_to_bind;
		}
	}
}