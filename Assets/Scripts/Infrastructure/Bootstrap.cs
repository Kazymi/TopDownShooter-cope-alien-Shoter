using System;
using System.Collections;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    public SaveData saveData;
    private LoadSaveController loadSaveController;

    public event Action OnSave;
    public event Action OnLoad;

    private void Awake()
    {
        Initialize();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Save();
        }
    }

    private void Initialize()
    {
        loadSaveController = new LoadSaveController();
        saveData = Load();
        var wallet = new Wallet(saveData);
        ServiceLocator.Subscribe<Wallet>(wallet);
        ServiceLocator.Subscribe<SaveData>(saveData);
        ServiceLocator.Subscribe<Bootstrap>(this);
    }

    private IEnumerator PreLoad()
    {
       yield return new WaitForSeconds(1f);
        OnLoad?.Invoke();
    }

    private void OnDestroy()
    {
        ServiceLocator.UnSubscribe<Wallet>();
    }

    private SaveData Load()
    {
        var returnValue = loadSaveController.Load();
        StartCoroutine(PreLoad());
        return returnValue;
    }

    public void Save()
    {
        OnSave?.Invoke();
        loadSaveController.Save(saveData);
        Debug.Log("Save completed");
    }
}