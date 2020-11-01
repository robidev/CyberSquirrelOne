using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class StateSerializer : MonoBehaviour
{
    void Save(Player player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.fun";
        using(FileStream stream = new FileStream(path, FileMode.Create))
        {
            PlayerData data = new PlayerData(player);
            formatter.Serialize(stream, data); 
            //stream.Close();      
        }
    }

    PlayerData Load()
    {
        string path = Application.persistentDataPath + "/player.fun";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            PlayerData data = null;
            using(FileStream stream = new FileStream(path, FileMode.Open))
            {
                data = (PlayerData)formatter.Deserialize(stream);
                stream.Close();
            }
            return data;
        } else{
            Debug.Log("File not Found");
            return null;
        }
    }
}

[System.Serializable]
public class PlayerData
{
  public List<int> livingTargetPositions = new List<int>();
  public List<int> livingTargetsTypes = new List<int>();

  public int hits = 0;
  public int shots = 0;

  public PlayerData(Player player)
  {

  }
}

public class Player
{

}