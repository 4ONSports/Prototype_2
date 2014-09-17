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
	public class NotificationListener : NotifyListener {

		string debug = "";

		private void Log(string msg)
		{
			debug = msg + "\n" + debug;
		}
		
		public string getDebug()
		{
			return debug;
		}
		
		public void onRoomCreated (RoomData eventObj)
		{
			Log ("onRoomCreated");
		}

		public void onPrivateUpdateReceived (string sender, byte[] update, bool fromUdp)
		{
			Log ("onPrivateUpdate");
		}

		public void onRoomDestroyed (RoomData eventObj)
		{
			Log ("onRoomDestroyed");
		}
		
		public void onUserLeftRoom (RoomData eventObj, string username)
		{
			Log ("onUserLeftRoom : " + username);
		}
		
		public void onUserJoinedRoom (RoomData eventObj, string username)
		{
			Log ("onUserJoinedRoom : " + username);

			if (AppWarp.isFirstPlayer) {
				AppWarp.name_player2 = username;
			}
//			else {
//				AppWarp.name_player1 = AppWarp.localusername;
//			}
		}
		
		public void onUserLeftLobby (LobbyData eventObj, string username)
		{
			Log ("onUserLeftLobby : " + username);
		}
		
		public void onUserJoinedLobby (LobbyData eventObj, string username)
		{
			Log ("onUserJoinedLobby : " + username);
		}
		
		public void onUserChangeRoomProperty(RoomData roomData, string sender, Dictionary<string, object> properties, Dictionary<string, string> lockedPropertiesTable)
		{
			Log ("onUserChangeRoomProperty : " + sender);
		}
		
		public void onPrivateChatReceived(string sender, string message)
		{
			Log ("onPrivateChatReceived : " + sender);
		}
		
		public void onMoveCompleted(MoveEvent move)
		{
			Log ("onMoveCompleted by : " + move.getSender());
		}
		
		public void onChatReceived (ChatEvent eventObj)
		{
			Log(eventObj.getSender() + " sended " + eventObj.getMessage());
//			//			SimpleJSON.JSONNode msg =  SimpleJSON.JSON.Parse("aa");//eventObj.getMessage());
//			//			SimpleJSON.JSONNode msg =  SimpleJSON.JSONNode.Parse(eventObj.getMessage());
//			SimpleJSON.JObject msg =  SimpleJSON.JSON.Parse(eventObj.getMessage());
//			//msg[0] 
//			if(eventObj.getSender() != AppWarp.localusername)
//			{
////				AppWarp.movePlayer(msg["x"].AsFloat,msg["y"].AsFloat,msg["z"].AsFloat);
//				//Log(msg["x"].ToString()+" "+msg["y"].ToString()+" "+msg["z"].ToString());
//			}
		}
		
		public void onUpdatePeersReceived (UpdateEvent eventObj)
		{
			Log ("onUpdatePeersReceived");
			
			OnlineMessage msg = OnlineMessage.DecodeMessage(eventObj.getUpdate());

			if (msg.type == "new_match")
			{
				Application.LoadLevel (AppWarp.gameplaySceneStr);
				if(AppWarp.isFirstPlayer) {
					AppWarp.name_player2 = msg.sender;
//					// Tell second player to start match
//					AppWarp.warpClient.SendUpdatePeers(OnlineMessage.BuildBytes_NewMatch());
				}
				else {
					AppWarp.name_player1 = msg.sender;
				}
				
				OnlineGameEvent ge = new OnlineGameEvent(EOnlineGameEvent.ONL_EVT_START_MATCH);
				AppWarp.warpClient.SendUpdatePeers(OnlineMessage.BuildBytes_OnlineEvent(ge));
			}
			else if (msg.type == "oppName")
			{
				if(!AppWarp.isFirstPlayer) {
					AppWarp.name_player1 = msg.sender;
				}
			}
			else if (msg.type == "move")
			{
				if((AppWarp.isFirstPlayer && msg.sender != AppWarp.name_player1) ||
				   (!AppWarp.isFirstPlayer && msg.sender != AppWarp.name_player2)) {
//					AppWarp.gameplayObj.UpdateSceneFromMove(msg);
				}
			}
			else if (msg.type == "online_event")
			{
				if( msg.sender != AppWarp.localusername) {
					OnlineGameEventsHandler.BroadcastEvent(msg.onlineEvent);
				}
			}
			else {
				Debug.Log ("Invalid Message Type Received");
			}
		}
		
		public void onUserChangeRoomProperty(RoomData roomData, string sender, Dictionary<String, System.Object> properties)
		{
			Log("Notification for User Changed Room Propert received");
			Log(roomData.getId());
			Log(sender);
			foreach (KeyValuePair<String, System.Object> entry in properties)
			{
				Log("KEY:" + entry.Key);
				Log("VALUE:" + entry.Value.ToString());
			}
		}
		
		public void onUserPaused(string a, bool b, string c)
		{
		}
		
		public void onUserResumed(string a, bool b, string c)
		{
		}
		
		public void onGameStarted(string a, string b, string c)
		{
		}
		
		public void onGameStopped(string a, string b)
		{
		}
	}
}
