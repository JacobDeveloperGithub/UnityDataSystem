# UnityDataSystem
Simple unity save/load system with optional encryption.

Its very simple to use, uploading here for easy access to myself later.

Here is a simple usecase example:

[x] Step one: Create Serializable POD (plain old data) structure.

```cs
  [System.Serializable]
  public class SpawnedObjectData{
      public bool isActive;
      public Vector3 position;
      public Color color;
  }
```

[x] Step two: Save a load that POD into a GameObject using a uniquely assigned key to that object (its name if its in the scene for example, or if its a system, just a string of your choice)
```cs
  public class SpawnedObject : MonoBehaviour 
  {
      private string key = name; //transform.name
      public void Start(){
          if(SaveData.HasData(name))
              Load();
      }
  
      public void Update(){
          if(Input.GetKeyDown(KeyCode.Space)){
              Save();
          }
      }
  }
```

[x] Write save and load functions (Can use an ISave interface or something, these will generally look the same)

```cs
    public void Save(){
        SaveData.StoreData(key, new SpawnedObjectData(){
            isActive = isActiveAndEnabled,
            position = transform.position,
            color = GetComponent<SpriteRenderer>().color
        });
    }

    public void Load(){
        var data = SaveData.ReadData<SpawnedObjectData>(key);
        transform.position = data.position;
        gameObject.SetActive(data.isActive);
        GetComponent<SpriteRenderer>().color = data.color;
    }
```

Future to maybe do:
[] May add in the future GUID support and generation functions
