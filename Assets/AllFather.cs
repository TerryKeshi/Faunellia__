using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class AllFather : MonoBehaviour
{
	Dictionary<string, Save> _theSave;
	Dictionary<string, Save> _theSaveOfTheSave;
	public Camera _camera;
	public Canvas _canvas;
	public Inventory _inventory;
	public AudioManager _audioManager;
	public string _savedSceneName;
	public GameObject _playerObject;
	private PlayerStorage _playerStorage;
	private PlayerMovement _playerMovement;
	private GameObject _playerHub;
	private PlayerCamScript _playerCamScript;
	private Vector3 _savedPlayerPosition;
	public float _savedPlayerXRot;
	public float _savedPlayerYRot;
	public List<string> _savedLoadedScenesNames;

	public int _enemyBulletSparklesCount;

	void Start()
	{
		_theSave = new Dictionary<string, Save>();
		
		_playerHub = _playerObject.transform.parent.gameObject;
		_playerStorage = _playerHub.GetComponent<PlayerStorage>();
		_playerMovement = _playerHub.GetComponent<PlayerMovement>();
		_playerCamScript = Camera.main.GetComponent<PlayerCamScript>();
	}

	public Dictionary<string, Save> DeepCopyDictionary(Dictionary<string, Save> original)
	{
		Dictionary<string, Save> copy = new Dictionary<string, Save>();

		foreach (var entry in original)
		{
			string key = entry.Key;
			Save originalValue = entry.Value;

			// Сериализуем объект в JSON строку
			string json = JsonUtility.ToJson(originalValue);

			// Десериализуем JSON строку обратно в новый объект
			Save copyValue = JsonUtility.FromJson<Save>(json);

			copy.Add(key, copyValue);
		}

		return copy;
	}

	public void SaveTheSave()
	{
		_theSaveOfTheSave = DeepCopyDictionary(_theSave);
		_savedPlayerXRot = _playerCamScript.xRotation;
		_savedPlayerYRot = _playerCamScript.yRotation;
		_savedPlayerPosition = _playerHub.transform.position;
		_savedSceneName = _playerStorage._currentSceneName;

		_savedLoadedScenesNames.Clear();
		for (int i = 0; i < SceneManager.sceneCount; i++)
			if (SceneManager.GetSceneAt(i).name != "Start")
				_savedLoadedScenesNames.Add(SceneManager.GetSceneAt(i).name);

		_inventory.SaveTheSave();
	}

	public void LoadTheSave()
	{
		_playerHub.transform.position = _savedPlayerPosition;
		_theSave = DeepCopyDictionary(_theSaveOfTheSave);
		_playerStorage.Heal(1000000);
		_playerStorage._currentSceneName = _savedSceneName;

		for (int i = 0; i < SceneManager.sceneCount; i++)
		{
			Scene scene0 = SceneManager.GetSceneAt(i);

			if (scene0.name != "Start")
				SceneManager.UnloadSceneAsync(scene0);
		}

		StartCoroutine(LoadSavedScene());
	}

	private IEnumerator LoadSavedScene()
	{
		_inventory.LoadTheSave();

		foreach (string sceneName in _savedLoadedScenesNames)
		{
			yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
		}

		yield return new WaitUntil(() => SceneManager.GetSceneByName(_savedSceneName).isLoaded);

		_playerHub.transform.position = _savedPlayerPosition;
		_playerCamScript.StaticRotate(_savedPlayerXRot, _savedPlayerYRot);

		Debug.Log("SAVE LOADED!!!");
	}

	public void Save(string key, Save save)
	{
		if (_theSave.ContainsKey(key))
			_theSave[key] = save;
		else
			_theSave.Add(key, save);
	}

	public Save Load(string key)
	{
		if (_theSave.ContainsKey(key))
			return _theSave[key];
		else
			return new Save();
	}

	public bool Contains(string key)
	{
		return _theSave.ContainsKey(key);
	}
}