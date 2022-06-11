using System;
using Mirror;

namespace _Game.Scripts
{
	public class Damagable : NetworkBehaviour
	{

		[SyncVar(hook = nameof(OnHealthChanged))]
		public int _health = 10;

		public event Action<int> OnHit;

		public void OnHealthChanged(int _old, int _new)
		{
			OnHit?.Invoke(_health);
		}

		public void TakeHit(int damage)
		{
			_health -= damage;
		}
	}
}