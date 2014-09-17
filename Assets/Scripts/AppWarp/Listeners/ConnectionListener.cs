using UnityEngine;

using com.shephertz.app42.gaming.multiplayer.client;
using com.shephertz.app42.gaming.multiplayer.client.events;
using com.shephertz.app42.gaming.multiplayer.client.listener;
using com.shephertz.app42.gaming.multiplayer.client.command;
using com.shephertz.app42.gaming.multiplayer.client.message;
using com.shephertz.app42.gaming.multiplayer.client.transformer;

using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class ConnectionListener : ConnectionRequestListener {

		string debug = "";
		
		private void Log(string msg)
		{
			debug = msg + "\n" + debug;
		}
		
		public string getDebug()
		{
			return debug;
		}

		public void onConnectDone(ConnectEvent eventObj)
		{
			if(eventObj.getResult() == WarpResponseResultCode.SUCCESS)
			{
				WarpClient.GetInstance().JoinRoom(AppWarp.roomid);
				Debug.Log("Connection Successful");
			}
			else {
				Debug.Log("Connection Failed");
			}
			Debug.Log ("onConnectDone : " + eventObj.getResult());
			Log ("onConnectDone : " + eventObj.getResult());
		}
		
		public void onInitUDPDone(byte res)
		{
		}
		
		public void onLog(String message){
			Log (message);
		}
		
		public void onDisconnectDone(ConnectEvent eventObj)
		{
			if(eventObj.getResult() == WarpResponseResultCode.SUCCESS)
			{
				Debug.Log("Disconnect Successful");
			}
			else {
				Debug.Log("Disconnect Failed");
			}
			Log("onDisconnectDone : " + eventObj.getResult());
		}

	}
}
