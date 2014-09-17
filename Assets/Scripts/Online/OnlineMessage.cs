using UnityEngine;
using System.Collections;

public class OnlineMessage {
	
	public string sender;
	public int gridBoxIndex;
	public string piece;
	public string type;
	public OnlineGameEvent onlineEvent;
	
	public static OnlineMessage DecodeMessage(byte[] update) {
		SimpleJSON.JSONClass jsonObj = (SimpleJSON.JSONClass)SimpleJSON.JSONClass.Parse(System.Text.Encoding.UTF8.GetString(update, 0, update.Length));

		OnlineMessage msg = new OnlineMessage();  
		msg.sender = jsonObj["sender"].Value;  
		msg.type = jsonObj["type"].Value;
		if(msg.type == "move")  
		{  
			msg.gridBoxIndex = jsonObj["gridBoxIndex"].AsInt;  
			msg.piece = jsonObj["piece"].Value;  
		}
		if(msg.type == "online_event")  
		{
			int i = jsonObj["gE"].AsInt;
			EOnlineGameEvent e = (EOnlineGameEvent)jsonObj["gE"].AsInt;
			EOnlineGameEvent eb = EOnlineGameEvent.ONL_EVT_START_MATCH+i;
			bool hasEP = jsonObj["hasEP"].AsBool;
			object obj = hasEP? (object)jsonObj["gEP"].AsObject: null;
			msg.onlineEvent = new OnlineGameEvent(e, obj);
		}
		return msg;  
	}
	
	public static byte[] BuildMessageBytes_Move( string piece, int gbi ) {
		SimpleJSON.JSONClass moveObj = new SimpleJSON.JSONClass();  
		moveObj.Add("gridBoxIndex", gbi);  
		moveObj.Add("sender", AppWarp.localusername);  
		moveObj.Add("piece", piece);  
		moveObj.Add("type", "move");  
		return System.Text.Encoding.UTF8.GetBytes(moveObj.ToString());  
	}
	
	public static byte[] BuildBytes_NewMatch() {
		SimpleJSON.JSONClass obj = new SimpleJSON.JSONClass();  
		obj.Add("sender", AppWarp.localusername);  
		obj.Add("type", "new_match");  
		
		byte[] retVal = System.Text.Encoding.UTF8.GetBytes(obj.ToString());
		return retVal;  
	}
	
	public static byte[] BuildMessageBytes_OppName() {
		SimpleJSON.JSONClass obj = new SimpleJSON.JSONClass();  
		obj.Add("sender", AppWarp.localusername);  
		obj.Add("type", "oppName");
		
		byte[] retVal = System.Text.Encoding.UTF8.GetBytes(obj.ToString());
		return retVal;  
	}
	
	public static byte[] BuildBytes_OnlineEvent( OnlineGameEvent _ge ) {
		SimpleJSON.JSONClass obj = new SimpleJSON.JSONClass();  
		obj.Add("sender", AppWarp.localusername);
		obj.Add("type", "online_event");
		obj.Add("gE", (int)_ge.gameEvent);
		if( _ge.gameEventProperty != null ) {
			obj.Add("hasEP", true);
			obj.Add("gEP", _ge.gameEventProperty);
		}
		else {
			obj.Add("hasEP", false);
			obj.Add("gEP", "");
		}
		
		byte[] retVal = System.Text.Encoding.UTF8.GetBytes(obj.ToString());
		return retVal;  
	}
}
