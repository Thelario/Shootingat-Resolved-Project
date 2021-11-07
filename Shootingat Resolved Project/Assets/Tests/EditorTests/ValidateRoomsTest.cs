using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using PabloLario.DungeonGeneration;
using UnityEditor;
using UnityEngine;

namespace PabloLario.Tests.EditorTests
{
    public class ValidateRoomsTest
    {
        // private RoomTypeOld GetFieldValue(this object obj, string name)
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
                    RoomTypeOld roomType = (RoomTypeOld) (roomField?.GetValue(room));

                    var spawnPointsfield = room.GetType().GetField("spawnPoints", bindingFlags);
                    List<Transform> spawnPoints = (List<Transform>) (spawnPointsfield?.GetValue(room));

                    if (roomType == RoomTypeOld.NormalRoom && !prefab.name.EndsWith("nitial"))
                    {
                        Assert.Greater(spawnPoints.Count, 0, $"Room in path {prefabPath} doesn't have any spawnPoints");
                    }
                }
            }
        }

        // The next tests are going to be programmed because although right now all the rooms are correctly setup,
        // I am planning in the future to have more rooms of each type, which means that I can probably miss some
        // objects or smth when creating them. I also want to learn how to make tests, because I have never done 
        // them before. I am mainly copying Carlos' Tests (above), but changing the thing I am testing. Instead of
        // only testing the spawnpoints, I want to test the rest.

        [Test]
        public void ValidateRoomsDoors()
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

                    var doorsFields = room.GetType().GetField("roomDoors", bindingFlags);
                    List<Door> doors = (List<Door>) (doorsFields?.GetValue(room));

                    if (!prefab.name.EndsWith("nitial"))
                    {
                        Assert.Greater(doors.Count, 0, $"Room in path {prefabPath} doesn't have any doors.");
                    }
                }
            }
        }

        [Test]
        public void ValidateRoomsSpriteMasks()
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
                    Transform spriteMasksObject = prefab.transform.Find("SpriteMasks");

                    if (spriteMasksObject != null)
                        Assert.Greater(spriteMasksObject.childCount, 0,
                            $"Room in path {prefabPath} doesn't have any spriteMasks.");
                }
            }
        }

        [Test]
        public void ValidateSpriteMasksObjectName()
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
                    Transform spriteMasksObject = prefab.transform.Find("SpriteMasks");

                    Assert.NotNull(spriteMasksObject, $"Room in path {prefabPath} doesn't have a SpriteMasks object.");
                }
            }
        }

        [Test]
        public void ValidateRoomsPoints()
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

                    var pointsFields = room.GetType().GetField("points", bindingFlags);
                    List<Transform> points = (List<Transform>) (pointsFields?.GetValue(room));

                    Assert.Greater(points.Count, 0, $"Room in path {prefabPath} doesn't have any doors.");
                }
            }
        }
    }
}