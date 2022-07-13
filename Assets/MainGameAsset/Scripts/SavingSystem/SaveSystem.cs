using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem : MonoBehaviour
{
    static public SaveSystem instance;
    string filePath;

    private void Awake()
    {
       
        filePath = Application.persistentDataPath + "/save.data";
        #region singleton
        if(instance == null) 
        {
            instance = this;
        }
        else 
        {
            Destroy(gameObject);
        }
        #endregion
    }
    public void SaveGame(GameData saveData)
    {
        
        FileStream dataStream = new FileStream(filePath, FileMode.Create);//open up a file path we crated and prepare to create new file in there

        BinaryFormatter converter = new BinaryFormatter();
        converter.Serialize(dataStream, saveData); //convert the data to binary and save at the pathfile

        dataStream.Close();//stop the stream , so we are not going to change any other information
    }

    
    public GameData LoadGame() 
    {
        if (File.Exists(filePath) )
        {
            //file exists, so return it
            FileStream dataStream = new FileStream(filePath, FileMode.Open);

            BinaryFormatter converter = new BinaryFormatter();
            GameData saveData = converter.Deserialize(dataStream) as GameData;

            dataStream.Close();
            return saveData;

        }
        else 
        {
            //file does not exist,make a new one and return it
            Debug.LogError("Save file not found in " + filePath);
            return null;
        }
    
    }
}
