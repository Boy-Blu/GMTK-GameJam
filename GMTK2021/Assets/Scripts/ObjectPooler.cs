using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectPooler : MonoBehaviour {

    #region Singleton
    // Create a singleton
    private static ObjectPooler _instance;

    public static ObjectPooler Instance { get { return _instance; } }

    void Awake () {
        if (_instance != null && _instance != this) {
            Destroy (this.gameObject);
        } else {
            _instance = this;
        }
    }

    void OnDestroy () {
        if (this == _instance) {
            _instance = null;
        }
    }
    #endregion

    // Where to get the Object From\
    [System.Serializable]
    public class Pool {
        public string tag;
        public GameObject prefab;
        public int size;
        public bool isUI;
    }
    // Canavas Obj

    public Canvas canvas;
    // List of all pools we want
    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDict;

    void Start () {
        poolDict = new Dictionary<string, Queue<GameObject>> ();
        foreach (Pool pool in pools) {
            Queue<GameObject> oPool = new Queue<GameObject> ();

            // Set up pool with size GameObjects
            for (int i = 0; i < pool.size; i++) {
                GameObject obj = Instantiate (pool.prefab);
                obj.SetActive (false);
                oPool.Enqueue (obj);

                if (pool.isUI) {
                    obj.transform.SetParent (canvas.transform, false);
                }
            }
            poolDict.Add (pool.tag, oPool);
        }
    }

    /// <summary>
    ///  Spawns a GameObject given a directory at a given Position + rotation
    /// </summary>
    /// <param name="tag">Spawns in a Object of the same Tag</param>
    /// <param name="position"> Position to spawn at</param>
    /// <param name="tag"> Rotation of the object spawn</param>
    public GameObject SpawnFromPool (string tag, Vector3 position, Quaternion rot) {
        if (!poolDict.ContainsKey (tag)) {
            Debug.LogWarning ("Tag '" + tag + "' does not exist");
            return null;
        } else if (poolDict[tag].Count == 0) {
            // Change to instantiate when empty
            Debug.LogWarning ("Tag '" + tag + "' Object Pool has no more GameObjects in it");
            return null;
        }
        GameObject objectSpawn = poolDict[tag].Dequeue ();

        // Instatiate with Certain Attributes
        objectSpawn.SetActive (true);
        objectSpawn.transform.position = position;
        objectSpawn.transform.rotation = rot;

        return objectSpawn;
    }

    /// <summary> 
    /// Spawns a GameObject given a directory at a given position
    /// </summary>
    /// <param name="tag"> Spawns in a Object of the same Tag </param>
    /// <param name="position">Position to spawn at </param>
    public GameObject SpawnFromPool (string tag, Vector3 position) {
        return SpawnFromPool(tag, position, Quaternion.identity);
    }

    /// <summary> 
    /// Returns a GameObject to it's pool
    /// </summary>
    /// <param name="tag"> Tag of where the Obect belongs</param>
    /// <param name="GO"> the GameObject to return</param>
    public void ReturnToPool (string tag, GameObject GO) {
        if (!poolDict.ContainsKey (tag)) {
            Debug.LogWarning ("Tag '" + tag + "' does not exist");
            return;
        }
        GO.SetActive (false);
        poolDict[tag].Enqueue (GO);
    }

}