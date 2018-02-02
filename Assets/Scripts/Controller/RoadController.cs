using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Controller;

public class RoadController : MonoBehaviour {

    public int maxRoadBlock;
    public GameObject roadBlock_Left_Prefab;
    public GameObject roadBlock_Right_Prefab;
    public GameObject mainMapPrefab;
    public GameObject []subMapPrefabs;
    public GameObject coinPrefab;
    public GameObject temp;

    public int leftTileColsFromPlayer;  // player의 좌측 tile columns 수
    public int rightTileColsFromPlayer; // player의 우측 tile columns수
    public int backTileRowsFromPlayer;  // player의 뒤쪽 tile row수
    public int frontTileRowsFromPlayer; // player의 앞쪽 tile row수


    List<RoadBlockProperty> roadBlocks = new List<RoadBlockProperty>();
    
    public RoadBlockProperty GetRoadBlockProperty(int index)
    {
        if (index < 0)
            return null;

        if (roadBlocks.Count > index)
            return roadBlocks[index];

        return null;
    }

    // road blocks를 제거한다.
    // game object도 제거한다.
    void ClearRoadBlocks()
    {
        foreach(var prop in roadBlocks)
        {
            prop.DeleteAllObjects();
            prop.DeleteCoin();
        }
        roadBlocks.Clear();
    }

    // map을 구성한다.
    public void MakeMap()
    {
        ClearRoadBlocks();

         // 시작할때 roadblock을 랜덤하게 만든다.
        GenerateRoadBlockList();

        // block을 생성한다.
        MakeRoadBlocks();

        // coin을 생성한다.
        MakeCoins();

        // map block을 생성한다.
        MakeMapBlocks();
    }

    void OnEnable()
    {
       
    }


	// Use this for initialization
	void Start () {
	}

    // road block 목록을 만든다.
    // 랜덤하게 만든다.(추후에 알고리즘 적용할 필요가 있다)
    void GenerateRoadBlockList()
    {
        roadBlocks.Clear();
        RoadBlockProperty prop;
        for (int ix = 0; ix < maxRoadBlock; ++ix)
        {
            prop = new RoadBlockProperty();

            // 시작은 항상 우측으로 가도록 한다.
            if (ix == 0)
                prop.Left = false;
            else
                prop.Left = Random.Range(0, 2) == 0 ? false : true;

            // 10개 전에는 항상 코인이 없다
            if (ix < 10)
                prop.CoinNums = 0;
            else
                prop.CoinNums = Random.Range(0, 3) == 0 ? Random.Range(1, 3) : 0;


            roadBlocks.Add(prop);
        }
    
    }

    // player 위치 좌우로 map block game object를 만든다.
    // 보이는 것만 만든다.
    public void MakeMapBlockRoadSideByPlayerPosition(int index, int playerPosition)
    {
        // player 기준으로 보이지 않는 경우에는 만들지 않는다.
        if (!IsInVisibleRangeByPlayerPosition(index, playerPosition))
            return;

        if(index < 0  || index >= roadBlocks.Count)
            return;

        RoadBlockProperty rb = roadBlocks[index];

        Quaternion rotation = Quaternion.identity;
        Vector3 pos;

        GameObject obj;
        // road 측면에 main map block을 만든다.
        {
            int count = 2;

            // 좌
            for (int ix = 0; ix < count; ++ix)
            {
                pos = rb.Position;
                pos.x -= (ix + 1);
                obj = Instantiate(mainMapPrefab, pos, rotation);
                obj.transform.parent = temp.transform;
                rb.AddObject(obj);
            }

            // 우
            for (int ix = 0; ix < count; ++ix)
            {
                pos = rb.Position;
                pos.x += (ix + 1);
                obj = Instantiate(mainMapPrefab, pos, rotation);
                obj.transform.parent = temp.transform;
                rb.AddObject(obj);
            }
        }

        // road 측면을 제외한 위치에 sub map block을 랜덤하게 만든다.
        {
            int count = 10;

            // 좌
            for (int ix = 0; ix < count; ++ix)
            {
                pos = rb.Position;
                pos.x -= (ix + 3);

                int idx = Random.Range(0, subMapPrefabs.Length);
                obj = Instantiate(subMapPrefabs[idx], pos, rotation);
                obj.transform.parent = temp.transform;
                rb.AddObject(obj);
            }

            // 우
            for (int ix = 0; ix < count; ++ix)
            {
                pos = rb.Position;
                pos.x += (ix + 3);

                int idx = Random.Range(0, subMapPrefabs.Length);
                obj = Instantiate(subMapPrefabs[idx], pos, rotation);
                obj.transform.parent = temp.transform;
                rb.AddObject(obj);
            }
        }
        
    }

    // 시작위치의 아래에 기본 맵을 채운다.
    void MakeMapBlocks_Base()
    {
        Quaternion rotation = Quaternion.identity;
        Vector3 pos;

        GameObject obj;
        int count = 10;
        int countSide = 5;
        for (int jx = 0; jx < count; ++jx)
        {
            Vector3 posOrg = roadBlocks[0].Position;
            posOrg.z -= (jx + 1);

            // 좌
            for (int ix = 0; ix < countSide; ++ix)
            {

                pos = posOrg;
                pos.x -= (ix + 0);
                obj = Instantiate(mainMapPrefab, pos, rotation);
                obj.transform.parent = temp.transform;
            }

            // 우
            for (int ix = 0; ix < countSide; ++ix)
            {
                pos = posOrg;
                pos.x += (ix + 1);
                obj = Instantiate(mainMapPrefab, pos, rotation);
                obj.transform.parent = temp.transform;
            }
        }
    }

    // road 좌우로 생성되는 일반 map block을 만든다.
    void MakeMapBlocks_RoadSide()
    {
        for (int ix = 0; ix < roadBlocks.Count; ++ix)
        {
            MakeMapBlockRoadSideByPlayerPosition(ix, 0);
        }
    }

    // map block gameobject를 만든다.
    void MakeMapBlocks()
    {
        Quaternion rotation = Quaternion.identity;
        Vector3 pos;

        // 시작위치의 에서 아래에 기본 맵을 채운다.
        {
            MakeMapBlocks_Base();
        }

        // road의 좌우 3개씩 일반 맵을 랜덤하게 추가한다.
        {
            MakeMapBlocks_RoadSide();
        }
    }

    // player의 위치범위에 포함되는 인덱스인지?
    // row만 비교한다.
    public bool IsInVisibleRangeByPlayerPosition(int index, int playerPosition)
    {
        if (index < playerPosition - backTileRowsFromPlayer)
            return false;
        if (index > playerPosition + frontTileRowsFromPlayer)
            return false;

        return true;
    }

    // road block gameobject 1개를 만든다.
    public void MakeRoadBlock(int index, Vector3 pos, int playerPosition)
    {
        if (IsInVisibleRangeByPlayerPosition(index, playerPosition))
        {
            // 방향
            bool left = roadBlocks[index].Left;

            // 회전
            Quaternion rotation = Quaternion.identity;

            // prefab
            GameObject prefab = left == true ? roadBlock_Left_Prefab : roadBlock_Right_Prefab;

            // road block 생성
            GameObject rb = Instantiate(prefab, pos, rotation);
            rb.transform.parent = temp.transform;

            // object 보관
            roadBlocks[index].AddObject(rb);

        }
        
        // 위치 보관
        roadBlocks[index].Position = pos;
    }

    // road block gameobject를 만든다.
    // player를 기준으로 범위내 블럭만 gameobject를 만들고, 나머지는 데이터만 생성한다.
    void MakeRoadBlocks()
    {
        Vector3 lastRoadBlockPos = new Vector3(0, 0, 0);
        Vector3 pos = lastRoadBlockPos;
        for (int ix = 0; ix < roadBlocks.Count; ++ix)
        {
            MakeRoadBlock(ix, pos, 0);
           
            // 마지막 위치 보관
            lastRoadBlockPos = pos;

            // 다음 위치 갱신(이번 블럭의 방향에 따라 다음블럭의 위치가 정해진다)
            {
                pos.z += 1.0f;
                if (roadBlocks[ix].Left)
                    pos.x += 1.0f;
                else
                    pos.x -= 1.0f;
            }
        }
    }

    // coin gameobject를 만든다.
    // playerPosition 기준으로 보이는 것만 만든다.
    public void MakeCoin(int index, Vector3 blockPosition, int playerPosition)
    {
        if (IsInVisibleRangeByPlayerPosition(index, playerPosition))
        {
            Quaternion rotation = Quaternion.Euler(45.0f, 0.0f, 0.0f);

            Vector3 pos = blockPosition;
            pos.y = 0.5f;

            // coin 생성
            GameObject coin = Instantiate(coinPrefab, pos, rotation);
            coin.transform.parent = temp.transform;

            // 코인 object를 생성하는데, 갯수에 따라 크기를 달리한다.
            coin.transform.localScale *= roadBlocks[index].CoinNums;

            // game object 보관
            roadBlocks[index].SetCoin(coin);
        }
    }

    // coins gameobject를 만든다.
    void MakeCoins()
    {
        for (int ix = 0; ix < roadBlocks.Count; ++ix)
        {
            MakeCoin(ix, roadBlocks[ix].Position, 0);
        }
    }

	// Update is called once per frame
	void Update () {
	}
}
