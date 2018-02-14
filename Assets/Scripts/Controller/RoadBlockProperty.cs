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
        // 좌우
        bool left;
        public bool Left
        { get; set; }
        

        // coin 개수(0이상)
        int coinNums;
        public int CoinNums
        { get; set; }

        // coin object
        GameObject coin;
        public void SetCoin(GameObject newCoin)
        {
            coin = newCoin;
        }

        // 코인 object를 모두 삭제한다.
        public void DeleteCoin()
        {
            Mem.DestroyObject(coin);
            coin = null;
        }

        // road block과 tile object들(나중에 삭제를 위해서 등록해둔다)
        List<GameObject> objects    = new List<GameObject>();
        public void AddObject(GameObject obj)
        {
            objects.Add(obj);
        }

        // coin을 제외한 모든 object들을 삭제한다.
        public void DeleteAllObjects()
        {
            foreach(var obj in objects)
            {
                Mem.DestroyObject(obj);
            }
            objects.Clear();
        }

        // 좌표
        Vector3 position;
        public Vector3 Position
        { get; set; }

        // 장애물


    }
}
