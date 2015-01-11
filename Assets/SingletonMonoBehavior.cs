using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
		private static T instance;
		public static T Instance {
				get {
						if (instance == null) {
								instance = (T)FindObjectOfType (typeof(T));

								if (instance == null) {
										string script = null;
										if (typeof(T) == typeof(FixCamera)) {
												script = "FixCamera";
										} else if (typeof(T) == typeof(FixCoordinates)) {
												script = "FixCoordinates";
										}
										GameObject.Instantiate (Resources.Load (script, typeof(GameObject)));
								}
						}
			
						return instance;
				}
		}
	
}
