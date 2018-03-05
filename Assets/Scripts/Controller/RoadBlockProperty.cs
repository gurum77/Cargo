using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Controller
{
    // road block 속성
    public class MapBlockProperty
    {
        // item
        public enum ItemType
        {
            eNone = -1,
            eCoin,
            eBigCoin,
            eFlag,
            eCount
        };

        public MapBlockProperty()
        {
            Item = ItemType.eNone;
        }

        public ItemType Item
        {get;set;}

        // 좌우
        bool left;
        public bool Left
        { get; set; }
        
        // item object
        GameObject itemGameObject;
        public void SetItemGameObject(GameObject newItem)
        {
            itemGameObject = newItem;
        }

      
        // 코인의 개수를 리턴
        // 코인이 아니면 0개이다.
        public int GetCoinNums()
        {
            if (Item == ItemType.eCoin)
                return 100;
            else if (Item == ItemType.eBigCoin)
                return 300;

            return 0;
        }

        // 코인 object를 모두 삭제한다.
        public void DeleteItemGameObject()
        {
            Mem.DestroyGameObject(itemGameObject);
            itemGameObject = null;

            Item = ItemType.eNone;
        }


        // road block과 tile object들(나중에 삭제를 위해서 등록해둔다)
        List<GameObject> gameObjects    = new List<GameObject>();
        public void AddGameObject(GameObject obj)
        {
            gameObjects.Add(obj);
        }

        // coin을 제외한 모든 object들을 삭제한다.
        public void DeleteAllGameObjects()
        {
            foreach(var obj in gameObjects)
            {
                Mem.DestroyGameObject(obj);
            }
            gameObjects.Clear();
        }

        // 좌표
        Vector3 position;
        public Vector3 Position
        { get; set; }

        // 장애물


    }
}
