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
            eDiamond,
            eFlag,
            eExplosion, // 폭발
            eBlank, // 장애물(띄어 넘어야 한다)
            eClock, // energy bar 모드에서 시간을 늘려줌
            eLife,  // 모든 게임에서 생명을 1개 증가시킴
            eRock,  // 여러번을 두둘겨야 사라진다.
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

      

        // item의 health가 남아 있는지?
        public bool IsRemainHealth()
        {
            if (Item != ItemType.eRock)
                return false;

            if (itemGameObject)
            {
                Rock rock = itemGameObject.GetComponent<Rock>();
                if (rock)
                {
                    return rock.IsRemainHealth();
                }
            }

            return false;
        }

        // item에 damage를 준다.
        public void AddDamage(int power)
        {
            if (Item != ItemType.eRock)
                return;

            if(itemGameObject)
            {
                Rock rock = itemGameObject.GetComponent<Rock>();
                if(rock)
                {
                    rock.AddDamage(power);

                    if(GameController.Instance.Player.audioSourceRock)
                    {
                        GameController.Instance.Player.audioSourceRock.Play();
                    }
                }
            }
        }
        
        // item object
        GameObject itemGameObject;
        public GameObject ItemGameObject
        {
            get { return itemGameObject; }
            set { itemGameObject = value; }
            
        }

      
        // 코인의 개수를 리턴
        // 코인이 아니면 0개이다.
        public int GetCoinNums()
        {
            if (Item == ItemType.eCoin)
                return 1;
            else if (Item == ItemType.eBigCoin)
                return 3;

            return 0;
        }

        // Diamond의 개수를 리턴
        // Diamond가 아니면 0개이다.
        public int GetDiamondNums()
        {
            if (Item == ItemType.eDiamond)
                return 1;
            
            return 0;
        }

        // 코인 object를 모두 삭제한다.
        public void DeleteItemGameObject()
        {
            Mem.DestroyGameObject(itemGameObject);
            itemGameObject = null;
            if (Item != ItemType.eBlank)
            {
                Item = ItemType.eNone;
            }
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
