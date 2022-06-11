using UnityEngine;
using Mirror;
using Random = UnityEngine.Random;

public class Player : NetworkBehaviour
{
	public TextMesh playerNameText;
	public GameObject floatingInfo;

	private Material playerMaterialClone;
	
	[SyncVar(hook = nameof(OnNameChanged))]
	public string _playerName;

	[SyncVar(hook = nameof(OnColorChanged))]
	public Color _playerColor = Color.white;
	
	private SceneScript _sceneScript;
	
	void Awake()
	{
		//allow all players to run this
		_sceneScript = FindObjectOfType<SceneScript>();
	}

	
	void OnNameChanged(string _Old, string _New)
	{
		playerNameText.text = _playerName;
	}

	void OnColorChanged(Color _Old, Color _New)
	{
		playerNameText.color = _playerColor;
		playerMaterialClone = new Material(GetComponent<Renderer>().material);
		playerMaterialClone.color = _playerColor;
		GetComponent<Renderer>().material = playerMaterialClone;
	}
	
		
	public override void OnStartLocalPlayer()
	{
		_sceneScript.playerScript = this;

		Camera.main.transform.SetParent(transform);
		Camera.main.transform.localPosition = new Vector3(0, 0.7f, 0.3f);

		floatingInfo.transform.localPosition = new Vector3(0, -0.3f, 0.6f);
		floatingInfo.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

		string name = "Player" + Random.Range(100, 999);
		Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
		CmdSetupPlayer(name, color);
	}

	private void Update()
	{
		if (!isLocalPlayer)
		{
			// make non-local players run this
			floatingInfo.transform.LookAt(Camera.main.transform);
		}
	}

	[Command]
	public void CmdSetupPlayer(string _name, Color _col)
	{
		//player info sent to server, then server updates sync vars which handles it on all clients
		_playerName = _name;
		_playerColor = _col;
		_sceneScript.statusText = $"{_playerName} joined.";
	}

	[Command]
	public void CmdSendPlayerMessage()
	{
		if (_sceneScript) 
			_sceneScript.statusText = $"{_playerName} says hello {Random.Range(10, 99)}";
	}
}