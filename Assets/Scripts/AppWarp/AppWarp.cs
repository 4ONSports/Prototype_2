using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using com.shephertz.app42.gaming.multiplayer.client;
using com.shephertz.app42.gaming.multiplayer.client.events;
using com.shephertz.app42.gaming.multiplayer.client.listener;
using com.shephertz.app42.gaming.multiplayer.client.command;
using com.shephertz.app42.gaming.multiplayer.client.message;
using com.shephertz.app42.gaming.multiplayer.client.transformer;

using AssemblyCSharp;

public class AppWarp : MonoBehaviour {
	
	//please update with values you get after signing up
	public static string apiKey = "a548e1b567249923bcba5c96a94341fdeed1ecbb11f115f8b52c41e68a2b4cd8";//"<api key>";
	public static string secretKey = "d75b2d992ab2820ae3616f35e6a16e25506cc039667d4f8d1d906d52a2b43187";//"<secret key>";
	public static string roomid = "1994991362";//"581581528";//"<room id>";
	public static string localusername;
	public static bool isFirstPlayer = false;
	
	public static WarpClient warpClient;
	public static ConnectionListener conn_listen;
	public static NotificationListener notify_listen;
	public static RoomListener room_listen;

	public static float interval = 0.1f;
	//	float timer = 0;
	public static GameObject obj;
	public static bool isConnected = false;
	public static GameObject gameplayObj;
	public static string gameplaySceneStr = "";
	public static string name_player1 = "";
	public static string name_player2 = "";

	void Awake() {
		DontDestroyOnLoad (this);
	}

	void Start () {
		warpClient = WarpClient.GetInstance (); // WarpClient.GetInstance() returns null until it is initialized

		conn_listen = new ConnectionListener();
		notify_listen = new NotificationListener();
		room_listen = new RoomListener();
	}
	
	public static void InitializeConnection(string _name, string _gameplayScene) {
		WarpClient.initialize(apiKey,secretKey);
		DontDestroyOnLoad (WarpClient.GetInstance().gameObject);
		warpClient = WarpClient.GetInstance ();
		WarpClient.GetInstance().AddConnectionRequestListener(conn_listen);
		WarpClient.GetInstance().AddRoomRequestListener(room_listen);

		localusername = _name;
		gameplaySceneStr = _gameplayScene;
		WarpClient.GetInstance().Connect(localusername);
	}

	public static void InitializeNotification() {
		WarpClient.GetInstance().AddNotificationListener(notify_listen);
	}
	
	public static void DeInitializeConnection() {
		if( warpClient != null ) {
			WarpClient.GetInstance().UnsubscribeRoom(AppWarp.roomid);
			WarpClient.GetInstance().LeaveRoom(AppWarp.roomid);
			WarpClient.GetInstance().RemoveNotificationListener(notify_listen);
			WarpClient.GetInstance().RemoveRoomRequestListener(room_listen);
			WarpClient.GetInstance().RemoveConnectionRequestListener(conn_listen);
			WarpClient.GetInstance().Disconnect();
		}
	}
	
	void Update () {
//		timer -= Time.deltaTime;
//		if(timer < 0)
//		{
//			string json = "{\"x\":\""+transform.position.x+"\",\"y\":\""+transform.position.y+"\",\"z\":\""+transform.position.z+"\"}";
//			
//			listen.sendMsg(json);
//			
//			timer = interval;
//		}
//		
//		if (Input.GetKeyDown(KeyCode.Escape)) {
//			Application.Quit();
//		}
//		WarpClient.GetInstance().Update();
	}
	
	void OnGUI()
	{
		GUI.contentColor = Color.black;
		GUI.Label(new Rect(10,100,100,100), conn_listen.getDebug());
		GUI.Label(new Rect(10,160,100,100), room_listen.getDebug());
		GUI.Label(new Rect(10,220,100,100), notify_listen.getDebug());
	}
	
	void OnApplicationQuit()
	{
		if( warpClient != null ) {
			Debug.Log ("Disconnecting");
			DeInitializeConnection();
		}
	}
	
}
