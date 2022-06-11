using _Game.Scripts;
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

	[SyncVar(hook = nameof(OnTitleChanged))]
	private string _fullTitleName;
	
	[SyncVar(hook = nameof(OnColorChanged))]
	public Color _playerColor = Color.white;
	private Damagable _damageable;
	[SerializeField] private Renderer _mesh;
	void Awake()
	{
		_damageable = GetComponent<Damagable>();
	}

	void OnTitleChanged(string _Old, string _New)
	{
		Debug.Log($"Syncing Title from {_playerName}");
		playerNameText.text = _New;
	}
	void OnNameChanged(string _Old, string _New)
	{
		playerNameText.text = _playerName;
	}

	void OnColorChanged(Color _Old, Color _New)
	{
		playerNameText.color = _playerColor;
		playerMaterialClone = new Material(_mesh.material);
		playerMaterialClone.color = _playerColor;
		_mesh.material = playerMaterialClone;
	}
	
		
	public override void OnStartLocalPlayer()
	{

		Camera.main.transform.SetParent(transform);
		Camera.main.transform.localPosition = new Vector3(0, 0.7f, 0.3f);

		floatingInfo.transform.localPosition = new Vector3(0, -0.3f, 0.6f);
		floatingInfo.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

		string name = "Player" + Random.Range(100, 999);
		Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
		CmdSetupPlayer(name, color);
		
		
		SetupEventHooks();
	}

	private void SetupEventHooks()
	{
		_damageable.OnHit += UpdateNameText;
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

	}

	[Command]
	private void UpdateNameText(int currentHealth)
	{
		//Debug.Log($"Owner is {name}");
		_fullTitleName = $"{_playerName}\nHealth {currentHealth}/10";
	}
	
}