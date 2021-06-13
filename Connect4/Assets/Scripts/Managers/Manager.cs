using UnityEngine;


namespace Connect4.Managers
{
    public abstract class Manager<T> : MonoBehaviour where T : Manager<T>
    {
        public static T Instance {get; private set;}


        protected virtual void Awake()
        {
            if(Instance != null)
            {
                Destroy(gameObject);
            }

            Instance = (T)this;
        }
    }
}
