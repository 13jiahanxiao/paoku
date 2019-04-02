using System.Collections.Generic;
using System.Reflection;
using System;
using System.Collections;
using System.ComponentModel; 
	
/// <summary>
/// Serialization utils, meant to be generic and work with any object, meant to work with Json
/// </summary>
public class SerializationUtils
{
	protected static Notify notify;
	
	static SerializationUtils()
	{
		notify = new Notify("SerializationUtils");	
	}
	
	/// <summary>
	/// Displays some information about a given type
	/// </summary>
    private static void DisplayTypeInfo(Type t)
    {
       	notify.Debug("\r\n{0}", t);

        notify.Debug("\tIs this a generic type definition? {0}", 
            t.IsGenericTypeDefinition);

        notify.Debug("\tIs it a generic type? {0}", 
            t.IsGenericType);

        Type[] typeArguments = t.GetGenericArguments();
        notify.Debug("\tList type arguments ({0}):", typeArguments.Length);
        foreach (Type tParam in typeArguments)
        {
            notify.Debug("\t\t{0}", tParam);
        }
    }
	
	/// <summary>
	/// Returns a List<typeX>
	/// </summary>
	private static Object CreateGenericList(Type typeX)
	{
	    Type listType = typeof(List<>);
	    Type[] typeArgs = {typeX};   
	    Type genericType = listType.MakeGenericType(typeArgs);   
	    object o = Activator.CreateInstance(genericType);
	    return o;
	}
	
	/// <summary>
	/// Returns a Dictionary<typeX,typeY>
	/// </summary>
	private static Object CreateGenericDictionary(Type typeX, Type typeY)
	{
	    Type dictType = typeof(Dictionary<,>);
	    Type[] typeArgs = {typeX, typeY};   
	    Type genericType = dictType.MakeGenericType(typeArgs);   
	    object o = Activator.CreateInstance(genericType);
	    return o;
	}	
	
	/// <summary>
	/// Sets the fields of destObject with data from a dictionary. the dictionary is probably coming from a json object
	/// </summary>
	/// <param name='destObject'>
	/// Destination object, we will set the fields here from the dictionary
	/// </param>
	/// <param name='data'>
	/// Dictionary data, probably coming from a json object
	/// </param>
	/// <param name='bindingAttr'>
	/// defaults to just considering instance public fields
	/// </param>
	public static void SetDataFromDictionary(Object destObject, 
		Dictionary<string, object> data, 
		BindingFlags bindingAttr = (BindingFlags.Public | BindingFlags.Instance))
	{
		if (data == null)
		{
			notify.Warning("SerializationUtils.SetDataFromDictionary dictionary is null");
			return;
		}
		
		// I'm so tempted to use reflection to do this, let's try it
		System.Type myType = destObject.GetType();
		FieldInfo[] myFieldInfo;
        myFieldInfo = myType.GetFields(BindingFlags.Public | BindingFlags.Instance);
		
		foreach ( FieldInfo fieldInfo in myFieldInfo)
        {
			System.Type fieldType = fieldInfo.FieldType;
			string memberName = fieldInfo.Name;
			
			if (data.ContainsKey(memberName))
			{
				if (fieldType == data[memberName].GetType())
				{
					fieldInfo.SetValue(destObject, data[memberName]);
					notify.Debug ("assigning " + data[memberName] + " to " + memberName);
				}
				else if ( fieldType == typeof(System.Int32) && data[memberName].GetType() == typeof(System.Int64))
				{
					// seems we save ints as int64
					int temp = System.Convert.ToInt32(data[memberName]);
					// if we overflow the load or save exception handlers should catch us
					fieldInfo.SetValue(destObject, temp);
				}
				else if ( fieldType == typeof(float) && data[memberName].GetType() == typeof(System.Int64))
				{
					// seems we save floats  as int64 if they are round
					float temp = (float) System.Convert.ToDouble(data[memberName]);
					// if we overflow the load or save exception handlers should catch us
					fieldInfo.SetValue(destObject, temp);
				}
				else if (typeof(IList).IsAssignableFrom(fieldType))
				{
					//notify.Debug("we have a List******");
					Type listType;
					Type[] typeArguments = fieldType.GetGenericArguments();
					if (typeArguments.Length > 0)
					{
						// create a new list based on the string entries, then set it
						listType = typeArguments[0];
						var newList = CreateGenericList(listType);
						MethodInfo mListAdd = newList.GetType().GetMethod("Add");
						IList list = data[memberName] as IList;
			            foreach (var strItem in list)
			            {
							var newListItem = Activator.CreateInstance(listType);
							FromJson(newListItem, (string) strItem);
							mListAdd.Invoke(newList, new object[] { newListItem });
			            }
						fieldInfo.SetValue(destObject, newList);
					}
					else
					{
						notify.Warning("don't know how to handle non generic list");	
					}
				}
				else if (typeof(IDictionary).IsAssignableFrom(fieldType))
				{
					//notify.Debug("we have a Dicitonary******");
					Type dictType1, dictType2;
					Type[] typeArguments = fieldType.GetGenericArguments();
					if (typeArguments.Length > 0)
					{
						// create a new dictionary based on the string entries, then set it
						dictType1 = typeArguments[0];
						//notify.Debug("dictType1.GetType = " + dictType1.Name);
						dictType2 = typeArguments[1];
						//notify.Debug("dictType2.GetType = " + dictType2.Name);
						var newDict = CreateGenericDictionary(dictType1, dictType2);
						IDictionary dict = data[memberName] as IDictionary;
						
						Type rawType = typeof(Dictionary<,>);
						Type genericType = rawType.MakeGenericType(dictType1, dictType2);
						MethodInfo myMethod = genericType.GetMethod("Add");

						MethodInfo myMethod1 = dictType1.GetMethod("Parse", new [] {typeof(string)});
						
			            foreach (DictionaryEntry dictItem in dict)
			            {
							//notify.Debug("dictItem.Key = " + dictItem.Key.ToString());
							//notify.Debug("dictItem.Key GetType() = " + dictItem.Key.GetType().ToString());
							//notify.Debug("dictItem.Value = " + dictItem.Value.ToString());
							//notify.Debug("dictItem.Value GetType() = " + dictItem.Value.GetType().ToString());
							
							// parse 1st value, since it comes in as a string
							var newItem1 = Activator.CreateInstance(dictType1);
							var parsedVal = myMethod1.Invoke(newItem1, new object[] { dictItem.Key });
							//notify.Debug("newItem1 GetType() = " + newItem1.GetType().ToString());
							//notify.Debug("newItem1 = " + newItem1.ToString());
							//notify.Debug("parsedVal GetType() = " + parsedVal.GetType().ToString());
							//notify.Debug("parsedVal = " + parsedVal.ToString());
								
							//var newDictItem1 = Activator.CreateInstance(dictType1);
							
							myMethod.Invoke(newDict, new object[] { parsedVal, dictItem.Value });
			            }	
						fieldInfo.SetValue(destObject, newDict);
					}
					else
					{
						notify.Warning("don't know how to handle non generic dictionary");	
					}
				}				
				else
				{
					notify.Warning("member {0} of type {1} does not match dictionary value of type {2}",
						memberName, fieldType.GetType(), data[memberName].GetType());
				}
			}
			else 
			{
				notify.Warning("dictionary does not contain key " + memberName);
			}
        }
	}
	
	/// <summary>
	/// returns a dictionary representation of the source object
	/// </summary>
	/// <returns>
	/// The dict.
	/// </returns>
	/// <param name='sourceObject'>
	/// Source object from which we create the dictionary
	/// </param>
	/// <param name='bindingAttr'>
	/// defaults to just considering instance public fields
	/// </param>
	public static Dictionary<string, object> ToDict(Object sourceObject, BindingFlags bindingAttr = (BindingFlags.Public | BindingFlags.Instance))
	{
		System.Type myType = sourceObject.GetType();
		FieldInfo[] myFieldInfo;
        myFieldInfo = myType.GetFields(bindingAttr);	
		Dictionary<string, object> d = new Dictionary<string, object>();
		
		for(int i = 0; i < myFieldInfo.Length; i++)
        {
			string memberName = myFieldInfo[i].Name;
			d.Add(memberName, myFieldInfo[i].GetValue(sourceObject));
		}
		return d;
	}
	
	/// <summary>
	/// Returns a json text representation of the source object
	/// </summary>
	/// <returns>
	/// Returns a json text representation of the source object
	/// </returns>
	/// <param name='sourceObject'>
	/// Source object.
	/// </param>
	/// <param name='bindingAttr'>
	/// Binding attr, defaults instance public fields
	/// </param>
	public static string ToJson ( Object sourceObject, BindingFlags bindingAttr = (BindingFlags.Public | BindingFlags.Instance))
	{
		Dictionary<string, object> dict = ToDict(sourceObject, bindingAttr);
		string result = MiniJSON.Json.Serialize(dict);		
		return result;
	}
	
	/// <summary>
	/// Assign fields of destObject from a json text string
	/// </summary>
	/// <returns>
	/// True if the json text string was decoded
	/// </returns>
	/// <param name='destObject'>
	/// the object we assing stuff to
	/// </param>
	/// <param name='jsonText'>
	/// the json text representation 
	/// </param>
	/// <param name='bindingAttr'>
	/// defaults to instance or public fields
	/// </param>
	public static bool FromJson  ( Object destObject, string jsonText, BindingFlags bindingAttr = (BindingFlags.Public | BindingFlags.Instance))
	{
		bool result = false;
		Dictionary<string, object> loadedData = MiniJSON.Json.Deserialize(jsonText) as Dictionary<string, object>;
		if (loadedData != null)
		{
			SetDataFromDictionary( destObject, loadedData, bindingAttr);
			result = true;
		}
		return result;
	}

//	public static bool HasMethod(this object objectToCheck, string methodName)
//	{
//	    var type = objectToCheck.GetType();
//	    return type.GetMethod(methodName) != null;
//	}
}




						
						//Dictionary<string, string> dictionary = new Dictionary<string, string>();
						
						//Type rawType1 = typeof(Dictionary<,>);
						//Type genericType1 = dictType1.MakeGenericType(dictType1);
						//MethodInfo myMethod1 = dictType1.GetMethod("TryParse");

							
							//if (dictType1.HasMethod("TryParse"))
							//	var newDictItem1a = 
							
							//Type type = typeof(double);
    						//string text = "123.45";
//   							object key = TypeDescriptor.GetConverter(dictType1).ConvertFromInvariantString(dictItem.Key.ToString());
							
							//FromJson(newDictItem1, (string) dictItem.Key);
//							newDictItem1 = dictItem.Key;
//							notify.Debug("newDictItem1 = " + newDictItem1.ToString());
//							notify.Debug("newDictItem1 GetType = " + newDictItem1.GetType().ToString());
//							newDictItem2 = dictItem.Value;
//							notify.Debug("newDictItem2 = " + newDictItem2.ToString());
//							notify.Debug("newDictItem2 GetType = " + newDictItem2.GetType().ToString());
							
							//notify.Debug("newDictItem1 = " + newDictItem1.ToString());
							//FromJson(newDictItem2, dictItem.Value.ToString());
							//notify.Debug("newDictItem2 = " + newDictItem2.ToString());
							
							//mDictAdd.Invoke(newDict, new object[] { newDictItem1, newDictItem2 });
//							myMethod.Invoke(newDict, new object[] { key, newDictItem2 });
							
						