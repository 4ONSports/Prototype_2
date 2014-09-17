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
	public class RoomListener : RoomRequestListener {

		string debug = "";
		
		private void Log(string msg)
		{
			debug = msg + "\n" + debug;
		}
		
		public string getDebug()
		{
			return debug;
		}
		
		public void onSubscribeRoomDone (RoomEvent eventObj)
		{
			if(eventObj.getResult() == WarpResponseResultCode.SUCCESS)
			{
				/*string json = "{\"start\":\""+id+"\"}";
				WarpClient.GetInstance().SendChat(json);
				state = 1;*/
//				WarpClient.GetInstance().JoinRoom(AppWarp.roomid);
				AppWarp.isConnected = true;
				WarpClient.GetInstance().GetLiveRoomInfo(AppWarp.roomid);  
			}
			
			Log ("onSubscribeRoomDone : " + eventObj.getResult());
		}
		
		public void onUnSubscribeRoomDone (RoomEvent eventObj)
		{
			AppWarp.isConnected = false;
			Log ("onUnSubscribeRoomDone : " + eventObj.getResult());
		}
		
		public void onJoinRoomDone (RoomEvent eventObj)
		{
			if(eventObj.getResult() == WarpResponseResultCode.SUCCESS)
			{
				WarpClient.GetInstance().SubscribeRoom(AppWarp.roomid);
			}
			else {
				Debug.Log("There are alredy 2 user wait for some time");
			}
			Log ("onJoinRoomDone : " + eventObj.getResult());
			
		}
		
		public void onLockPropertiesDone(byte result)
		{
			Log ("onLockPropertiesDone : " + result);
		}
		
		public void onUnlockPropertiesDone(byte result)
		{
			Log ("onUnlockPropertiesDone : " + result);
		}
		
		public void onLeaveRoomDone (RoomEvent eventObj)
		{
			Log ("onLeaveRoomDone : " + eventObj.getResult());
		}
		
		public void onGetLiveRoomInfoDone (LiveRoomInfoEvent eventObj)
		{
			Log ("onGetLiveRoomInfoDone : " + eventObj.getResult());
			
			if (eventObj.getResult() == WarpResponseResultCode.SUCCESS && (eventObj.getJoinedUsers() != null))  
			{  
				AppWarp.InitializeNotification ();

				if (eventObj.getJoinedUsers().Length == 1)  
				{  
					AppWarp.isFirstPlayer = true;
					// Decide what string is for first player and what is for second player
				}  
				else  
				{  
					AppWarp.isFirstPlayer = false;
					
					// Tell both players in room to load gameplay scene
					AppWarp.warpClient.SendUpdatePeers(OnlineMessage.BuildBytes_NewMatch());
				}

				AppWarp.interval = 8.0f;
			}
		}
		
		public void onSetCustomRoomDataDone (LiveRoomInfoEvent eventObj)
		{
			Log ("onSetCustomRoomDataDone : " + eventObj.getResult());
		}
		
		public void onUpdatePropertyDone(LiveRoomInfoEvent eventObj)
		{
			if (WarpResponseResultCode.SUCCESS == eventObj.getResult())
			{
				Log ("UpdateProperty event received with success status");
			}
			else
			{
				Log ("Update Propert event received with fail status. Status is :" + eventObj.getResult().ToString());
			}
		}
	}
}
