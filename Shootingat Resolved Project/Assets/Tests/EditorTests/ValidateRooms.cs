using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;


public class ValidateRooms
{


    // private GetFieldValue(this object obj, string name)
    // {
    //     // Set the flags so that private and public fields from instances will be found
    //     var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
    //     var field = obj.GetType().GetField(name, bindingFlags);
    //     return (RoomTypeOld) field?.GetValue(obj);
    // }

    [Test]
    public void ValidateRoomsSpawnPoints()
    {

        string absolutePath = Path.Combine("Assets", "Prefabs", "Rooms", "Rooms").ToString();
        string[] roomPaths = AssetDatabase.GetSubFolders(absolutePath);

        foreach (string roomPath in roomPaths)
        {
            string[] filesPaths = Directory.GetFiles(roomPath);
            List<String> prefabFilesPaths = filesPaths.Where(file => file.EndsWith("prefab")).ToList();

            foreach (string prefabPath in prefabFilesPaths)
            {
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                Room room = prefab.GetComponent<Room>();


                var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
                var roomField = room.GetType().GetField("oldType", bindingFlags);
                RoomTypeOld roomType = (RoomTypeOld)(roomField?.GetValue(room));

                var spawnPointsfield = room.GetType().GetField("spawnPoints", bindingFlags);
                List<Transform> spawnPoints = (List<Transform>)(spawnPointsfield?.GetValue(room));

                if (roomType == RoomTypeOld.NormalRoom && !prefab.name.EndsWith("nitial"))
                {
                    Assert.Greater(spawnPoints.Count, 0, $"room in path {prefabPath} doesn't have enough spawn points");

                }

            }


        }




        // GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(path);





    }

}
