using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;

public class ChunkLoader : MonoBehaviour
{
    static float[] xBorders;
    static float[] zBorders;
    AllFather _allFather;

    static int _chunkX;
    static int _chunkZ;

    void Start()
    {
        _allFather = GameObject.Find("AllFather").GetComponent<AllFather>();

        xBorders = new float[100];
        zBorders = new float[100];

        xBorders[0] = -36756.35f;
        xBorders[1] = -36006.12f;
        zBorders[0] = 21532.69f;
        zBorders[1] = 20786.4f;

        _chunkX = 300;
        _chunkZ = 900;

        for (int i = 0; i < xBorders.Length; i++)
            if (xBorders[i] == 0)
                xBorders[i] = xBorders[i - 1] + 756;

        for (int i = 0; i < zBorders.Length; i++)
            if (zBorders[i] == 0)
                zBorders[i] = zBorders[i - 1] - 756;
    }

    void Update()
    {
        float x = _allFather._camera.transform.position.x;
        float y = _allFather._camera.transform.position.z;

        int chunkX = 0;
        int chunkZ = 0;

        for (int i = 0; i < xBorders.Length; i++)
            if (x < xBorders[i])
            {
                chunkX = i;
                break;
            }
        for (int i = 0; i < zBorders.Length; i++)
            if (y > zBorders[i]) ///////////////////////////////////////////////////
            {
                chunkZ = i;
                break;
            }

        chunkX += 300;
        chunkZ = 900 - chunkZ;

        //Debug.Log($"x {x} y {y} Chunk C_{chunkX}_{chunkZ}");

        if (chunkX > _chunkX)
        {
            _chunkX = chunkX;
            Load($"C_{chunkX + 1}_{chunkZ}");
            Load($"C_{chunkX + 1}_{chunkZ + 1}");
            Load($"C_{chunkX + 1}_{chunkZ - 1}");
            Unload($"C_{chunkX - 2}_{chunkZ}");
            Unload($"C_{chunkX - 2}_{chunkZ + 1}");
            Unload($"C_{chunkX - 2}_{chunkZ - 1}");
        }
        else if (chunkX < _chunkX)
        {
            _chunkX = chunkX;
            Load($"C_{chunkX - 1}_{chunkZ}");
            Load($"C_{chunkX - 1}_{chunkZ + 1}");
            Load($"C_{chunkX - 1}_{chunkZ - 1}");
            Unload($"C_{chunkX + 2}_{chunkZ}");
            Unload($"C_{chunkX + 2}_{chunkZ + 1}");
            Unload($"C_{chunkX + 2}_{chunkZ - 1}");
        }

        if (chunkZ > _chunkZ)
        {
            _chunkZ = chunkZ;
            Load($"C_{chunkX}_{chunkZ + 1}");
            Load($"C_{chunkX + 1}_{chunkZ + 1}");
            Load($"C_{chunkX - 1}_{chunkZ + 1}");
            Unload($"C_{chunkX}_{chunkZ - 2}");
            Unload($"C_{chunkX + 1}_{chunkZ - 2}");
            Unload($"C_{chunkX - 1}_{chunkZ - 2}");
        }
        else if (chunkZ < _chunkZ)
        {
            _chunkZ = chunkZ;
            Load($"C_{chunkX}_{chunkZ - 1}");
            Load($"C_{chunkX + 1}_{chunkZ - 1}");
            Load($"C_{chunkX - 1}_{chunkZ - 1}");
            Unload($"C_{chunkX}_{chunkZ + 2}");
            Unload($"C_{chunkX + 1}_{chunkZ + 2}");
            Unload($"C_{chunkX - 1}_{chunkZ + 2}");
        }

        void Load(string name)
        {
            Debug.Log($"Load {name} ({SceneExists(name)})");

            if (SceneExists(name))
                SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
        }

        void Unload(string name)
        {
            Debug.Log($"Unload {name} ({SceneExists(name)})");

            if (SceneExists(name))
                SceneManager.UnloadSceneAsync(name);
        }

        bool SceneExists(string sceneName)
        {
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                var sceneNameInBuildSetting = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
                if (sceneNameInBuildSetting.Equals(sceneName))
                    return true;
            }
            return false;
        }
    }
}
