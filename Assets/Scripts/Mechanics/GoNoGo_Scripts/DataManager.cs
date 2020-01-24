using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(GonoGoData_Save))]
public class DataManager : MonoBehaviour
{
    GonoGoData_Save dataSave;

    [SerializeField] string file = "gonogoData.txt";

    string[] data;

    private void Start()
    {
        dataSave = GetComponent<GonoGoData_Save>();
    }

    //Guardamos el archivo en el documento de texto en lenguaje JSON
    public void Save()
    {
        /*data = dataSave.dataToSave();

        string json = JsonUtility.ToJson(data);*/
        string json = JsonUtility.ToJson(dataSave);

        WriteToFile(file, json);
        
    }
    public bool Load()
    {
        
        string path = GetFilePath(file);
        try
        {
        StreamReader reader = new StreamReader(path);
        string json = reader.ReadToEnd();
        reader.Close();

        JsonUtility.FromJsonOverwrite(json, dataSave);
            return true;
        }
        catch(FileNotFoundException e) { 
            Debug.LogWarning("No existe el fichero");
            Debug.LogWarning(e);
            return false;
        }
    }

    private void WriteToFile(string fileName, string json)
    {
        string path = GetFilePath(fileName);
        /*FileStream fileStream = new FileStream(path, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(json);
        }*/
        print(path);

        StreamWriter writer = new StreamWriter(path, false);
        writer.Write(json);
        writer.Close();
    }

    //Cogemos la ruta en la que dejar el archivo de texto
    private string GetFilePath(string fileName)
    {
        return Application.dataPath + "/" + fileName;
    }
}
