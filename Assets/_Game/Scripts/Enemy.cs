using System;
using _Game.Scripts;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;


public class Enemy : NetworkBehaviour
{
	[SerializeField] private TextMesh _enemyNameText;
	[SerializeField] private GameObject _floatingInfo;

	[SerializeField] private Renderer _mesh;
	
	

	
	[SyncVar(hook = nameof(OnNameChanged))]
	private string _enemyName;


	[SyncVar(hook = nameof(OnTitleChanged))]
	private string _fullTitleName;

	private Damagable _damageable;

	

	public override void OnStartServer()
	{
		StartSpawn();
	}

	private void OnNameChanged(string _Old, string _New)
	{
		_enemyNameText.text = _enemyName;
	}
	private void OnTitleChanged(string _Old, string _New)
	{
		_enemyNameText.text = _New;
	}
	
	[ServerCallback]
	public void StartSpawn()
	{
		_damageable = GetComponent<Damagable>();
		transform.position = FindObjectOfType<PrefabPool>().GetSpawnPosition();
		string name = "Enemy" + Random.Range(100, 999);
		//player info sent to server, then server updates sync vars which handles it on all clients
		_enemyName = name;
		SetupLocal();		
	}

	
	private void SetupLocal()
	{
		_damageable.OnHit += UpdateNameText;
	}


	private void UpdateNameText(int currentHealth)
	{
		_fullTitleName = $"{_enemyName}\nHealth {currentHealth}/10";
	}
	
	private void Update()
	{
		if (!isLocalPlayer)
		{
			// make non-local players run this
			_floatingInfo.transform.LookAt(Camera.main.transform);
		}
		
		transform.Translate(Vector3.forward * Time.deltaTime);
	}
}
