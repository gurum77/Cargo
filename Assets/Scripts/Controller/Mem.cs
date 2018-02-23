using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarchingBytes;
using UnityEngine;

// 메모리 관리
namespace Assets.Scripts.Controller
{
    public class Mem
    {
        // pool을 통해서 game object를 생성한다.
        static public GameObject Instantiate(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            if (prefab == null)
                return null;

            GameObject obj  = EasyObjectPool.instance.GetObjectFromPool(prefab.name, position, rotation);

            // 등록이 안된 경우이다. 에러 방지를 위해서 등록해준다.
            if(obj == null)
            {
                EasyObjectPool.instance.AddPools(prefab.name, prefab, 10, false);
                obj = EasyObjectPool.instance.GetObjectFromPool(prefab.name, position, rotation);
            }

            return obj;
        }

        // pool을 통해서 game object를 제거한다.
        static public void DestroyGameObject(GameObject obj)
        {
            if (obj == null)
                return;

            EasyObjectPool.instance.ReturnObjectToPool(obj);
        }
    }
}
