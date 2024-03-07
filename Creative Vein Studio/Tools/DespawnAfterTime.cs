using UnityEngine;

namespace CVStudio
{
	namespace HeistEscape
	{
		public class DespawnAfterTime : MonoBehaviour
		{

			[SerializeField] private float TimeToDespawn = 1.2f;
			private float originalTimeToDespawn;

			void OnEnable()
			{
				originalTimeToDespawn = TimeToDespawn;
			}

			// Update is called once per frame
			void Update()
			{
				// Check to see if despawn is less than or equal to 0 and prevent it from continuing
				if (TimeToDespawn <= 0) return;

				// Start the timer
				TimeToDespawn -= Time.deltaTime;

				// check to see if the time has lapsed below 0 and despawn item/object
				if (TimeToDespawn <= 0)
				{
					TimeToDespawn = originalTimeToDespawn;
					// SPManager.instance.DisablePoolObject(gameObject.transform);
				}

			}
		}
	}
}