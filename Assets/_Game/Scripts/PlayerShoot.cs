using Mirror;
using UnityEngine;

namespace _Game.Scripts
{
	public class PlayerShoot : NetworkBehaviour
	{
		private void Update()
		{
			if(!isLocalPlayer) return;

			Debug.DrawRay(transform.position, transform.forward, Color.red);
			if (Input.GetButtonDown("Fire1"))
			{
				var ray = new Ray(transform.position, transform.forward);
				if(Physics.Raycast(ray, out var hit, Mathf.Infinity))
				{
					var damageable = hit.transform.GetComponent<Damagable>();
					if (damageable != null)
					{
						Shoot(damageable);
					}

				}
			}
		}

		[Command]
		private void Shoot(Damagable damageable)
		{
			damageable.TakeHit(1);
		}
	}
}