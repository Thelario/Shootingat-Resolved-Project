using System.Collections.Generic;
using System.Linq;
using PabloLario.Managers;
using UnityEngine;

namespace PabloLario.DungeonGeneration
{
    public class DungeonInstantiator : MonoBehaviour
    {
        [SerializeField] private DungeonProceduralGeneration dungeonGenerator;
        [SerializeField] private RoomSelector roomSelector;
        [SerializeField] private GameObject initialRoom;
        
        private const float RoomSizeX = 40f;
        private const float RoomSizeY = 25f;

        private void Start()
        {
            GenerateDungeon();
        }

        private void GenerateDungeon()
        {
            List<RoomPos> rooms = dungeonGenerator.GenerateValidRoomsPos();

            Instantiate(initialRoom, Vector3.zero, Quaternion.identity, transform);

            foreach (RoomPos room in rooms.Skip(1))
            {
                RoomType rt = room.RoomDoorsType.ToRoomType();
                
                GameObject roomToBeCreated = room.RoomType switch
                {
                    RoomTypeOld.NormalRoom => roomSelector.GetRoomFromVariants(rt),
                    RoomTypeOld.TreasureRoom => roomSelector.GetTreasureRoomFromVariants(rt),
                    RoomTypeOld.BossRoom => roomSelector.GetBossRoomFromVariants(rt), 
                    _ => roomSelector.GetRoomFromVariants(rt) // Impossible to reach
                };
                
                Debug.Log(roomToBeCreated);
                Instantiate(roomToBeCreated, GlobalCoordinateOf(room.Pos), Quaternion.identity, transform);
            }
            
            GameManager.InvokeDelegateDungeonGeneration();
        }

        private Vector3 GlobalCoordinateOf(Vector2Int coordinate)
        {
            return new Vector3(coordinate.x * RoomSizeX, coordinate.y * RoomSizeY, 0);
        }
    }
}