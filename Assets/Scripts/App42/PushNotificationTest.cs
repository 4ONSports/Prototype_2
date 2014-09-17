using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.pushNotification;
using Online;

public class PushNotificationTest : MonoBehaviour
{
	//=====================================================================================================
	Constant cons = new Constant ();                                    // Making cons Object For Using Constant.
	PushNotificationResponse callBack = new PushNotificationResponse();// Making callBack Object for PushNotificationResponse.
	ServiceAPI sp = null;                                             // Initializing Service API.
	PushNotificationService pushNotificationService = null;          // Initializing PushNotification Service.
	//=====================================================================================================
	
	//=======================================================================================
	public string password = "password";
	public int max = 2;
	public int offSet = 1;
	public string success, box;
	//=======================================================================================
	
	#if UNITY_EDITOR
	public static bool Validator (object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
	{return true;}
	#endif
	void Start ()
	{
		#if UNITY_EDITOR
		ServicePointManager.ServerCertificateValidationCallback = Validator;
		#endif
		sp = new ServiceAPI (cons.apiKey,cons.secretKey);
	}
	
	// Update is called once per frame
	void Update ()
	{	
	}
	
	void OnGUI ()
	{
		
		if (Time.time % 2 < 1) {
			success = callBack.getResult ();
		}
		
		// For Setting Up ResponseBox.
		GUI.TextArea (new Rect (10, 5, 500, 175), success);
		
		//=========================================================================	
		if (GUI.Button (new Rect (50, 200, 200, 30), "Create Channel ForApp")) {
			pushNotificationService = sp.BuildPushNotificationService (); // Initializing PushNotification Service.
			pushNotificationService.CreateChannelForApp (cons.channelName, cons.description, callBack);
		}
		
		//=========================================================================	
		if (GUI.Button (new Rect (250, 200, 200, 30), "Subscribe To Channel ")) {
			pushNotificationService = sp.BuildPushNotificationService (); // Initializing PushNotification Service.
			pushNotificationService.SubscribeToChannel (cons.channelName, cons.userName, callBack);
		}
		
		//=========================================================================	
		if (GUI.Button (new Rect (50, 250, 200, 30), "Send Push Message ToChannel ")) {
			pushNotificationService = sp.BuildPushNotificationService (); // Initializing PushNotification Service.
			pushNotificationService.SendPushMessageToChannel (cons.channelName, cons.message, callBack);
		}
		
		//===================================###################=========================================	
		if (GUI.Button (new Rect (250, 250, 200, 30), "Unsubscribe From Channel")) {
			pushNotificationService = sp.BuildPushNotificationService (); // Initializing PushNotification Service.
			pushNotificationService.UnsubscribeFromChannel (cons.channelName, cons.userName, callBack);
		}
	}
}