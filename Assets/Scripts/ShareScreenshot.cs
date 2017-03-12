using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ShareScreenshot : MonoBehaviour
{

	public void CaptureScreenshotPlugin ()
	{
		Texture2D tex = new Texture2D (Screen.width, Screen.height);
		tex.ReadPixels (new Rect (0, 0, Screen.width, Screen.height), 0, 0);

		byte[] dataToSave = tex.EncodeToJPG();
		string destination = Path.Combine(Application.persistentDataPath,System.DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".jpg");
		print (destination);
		File.WriteAllBytes(destination, dataToSave);
		NativeShareScreenshot (destination);

	}

	public void CaptureScreenshotNoPlugin ()
	{
		Texture2D tex = new Texture2D (Screen.width, Screen.height);
		tex.ReadPixels (new Rect (0, 0, Screen.width, Screen.height), 0, 0);

		byte[] dataToSave = tex.EncodeToJPG();
		string destination = Path.Combine(Application.persistentDataPath,System.DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".jpg");
		print (destination);
		File.WriteAllBytes(destination, dataToSave);

		if(!Application.isEditor)
		{
			// block to open the file and share it ------------START
			AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
			AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
			intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
			AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
			AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse","file://" + destination);
			intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);

			intentObject.Call<AndroidJavaObject> ("setType", "text/plain");
			intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), "Jogue Flappy Wing, é muito legal!");
			intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), "Flappy Wing");

			intentObject.Call<AndroidJavaObject>("setType", "image/jpeg");
			AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

			currentActivity.Call("startActivity", intentObject);
		}


	}

	public void NativeShareScreenshot (string destination)
	{
		AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

		using (AndroidJavaClass javaClass = new AndroidJavaClass("br.edu.posgames.flappybird.NativeScreenshot"))
		{
			using (AndroidJavaObject activity = javaClass.GetStatic<AndroidJavaObject>("mContext"))
			{
				activity.Call("ShareScreenshot", destination);
			}
		}





	}

	public void CallShareScreenshot(string destination){
	
		//instance of Koi plugin
		AndroidJavaObject androidObject;

		//Reference to Unity Player
		AndroidJavaObject unityObject;


		AndroidJavaClass unityClass =  new AndroidJavaClass("com.unity3d.player.UnityPlayer");

		unityObject = unityClass.GetStatic<AndroidJavaObject>("currentActivity");


		AndroidJavaClass androidClass = new AndroidJavaClass("com.grepgame.android.plugin.grplugin.Koi");

		androidObject = androidClass.GetStatic<AndroidJavaObject>("instance");

		androidObject.Call("launchGallery", unityObject);

	}






}
