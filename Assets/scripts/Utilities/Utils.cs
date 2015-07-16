using UnityEngine;
using System.Collections;
using Newtonsoft.Json;
using System.Collections.Generic;

public class Utils 
{
	private static Utils _instance = null;
	public static Utils instance{get{return _instance ?? new Utils();}}


	Utils(){}//private constructor

	public void LogList<T>(List<T> list){Debug.Log (ListString<T>(list));}
	public string ListString<T>(List<T> list)
	{
		string output = "";
		foreach (T item in list) {
			output += item.ToString()+"\n";
		}
		return output;
	}
	
	public void LogDict<A, B>(Dictionary<A, B> list)
	{
		string output = "";
		foreach (A key in list.Keys) {
			output += key.ToString()+": "+list[key].ToString()+"\n";
		}
		Debug.Log (output);
	}

	public string Serialize (object data) 
	{
		return JsonConvert.SerializeObject (data);
	}
	
	public T Deserialize<T> (string data) 
	{
		return JsonConvert.DeserializeObject<T> (data);
	}

	/// <summary>Function to save byte array to a file</summary>
	/// <param name="_FileName">File name to save byte array</param>
	/// <param name="_ByteArray">Byte array to save to external file</param>
	/// <returns>Return true if byte array save successfully, if not return false</returns>
	public bool ByteArrayToFile(byte[] byteArray, string fileName, string dir = "Assets/Resources")
	{
		try{
			if(!System.IO.Directory.Exists(dir)){
				System.IO.Directory.CreateDirectory(dir);
			}
			System.IO.FileStream fs = new System.IO.FileStream(dir+"/"+fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);// Open file for reading
			fs.Write(byteArray, 0, byteArray.Length);// Writes a block of bytes to this stream using data from a byte array.
			fs.Close();// close file stream
			return true;
		}catch (System.Exception e){
			Debug.Log("Exception caught in process: " + e.Message);
		}		
		return false;// error occured, return false
	}
}