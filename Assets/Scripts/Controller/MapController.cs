using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Controller;
using MarchingBytes;

public class MapController : MonoBehaviour {

    static public string GetMapName(MapController.Map eMap)
    {
        InventoryGameData ig = new InventoryGameData();
        return ig.mapInfo[(int)eMap].Name;
    }

    // 맵의 종류
    public enum Map
    {
        eVillage, 
        eBasic,
        eSea,
        eDesert,
        eChristmas,
        eForeset,
        eCount
    };

    public int maxRoadBlock;    // 최대 road block 개수
    public int maxMainMapColumns;   // 최대 main map columns수(좌우 포함)
    
    public GameObject roadBlock_Left_Prefab;    // road block 왼쪽에 들어가는 prefab
    public GameObject roadBlock_Right_Prefab;   // road block 오른쪽에 들어가는 prefab
    public GameObject roadBlock_TurnToLeft_Prefab;    // road block 오른쪽에서 왼쪽으로 돌아가는 prefab
    public GameObject roadBlock_TurnToRight_Prefab;   // road block 왼쪽에서 오른쪽으로 돌아가는 prefab
    public GameObject mainMapPrefab;    // main map prefab
    public GameObject []subMapPrefabs;  // sub map prefab
    
    public GameObject temp;
    public GameObject finishPrefab;

    Vector3 mainMapSize;    // main map의 크기
    Vector3[] subMapSize;   // sub map의 크기
    Vector3 maxMapSize; // 가장 큰 map의 크기
    float lastMainMapBlockZ;    // 마지막 main map block의 Z 좌표
    bool[] enabledItem;
    public void EnableItem(MapBlockProperty.ItemType itemType, bool enable)
    {
        enabledItem[(int)itemType] = enable;
    }

    // item과 item의 발생 주기
    public GameObject[] itemPrefabs;
    public int[] itemGenerationCycle;   // 아이템 발생 주기
    public int GetItemPrefabIndex(GameObject prefab)
    {
        for(int ix = 0; ix < itemPrefabs.Length; ++ix)
        {
            if (itemPrefabs[ix] == prefab)
                return ix;
        }

        return -1;
    }
    
    #region 맵별 프리팹 정의
    enum MapPrefabIndex
    {
        eMain,
        eLeft,  // 왼쪽으로 가는 블럭
        eRight, // 오른쪽으로 가는 블럭
        eTurnToLeft,    // 오른쪽으로 가다가 왼쪽으로 회전하는 블럭
        eTurnToRight,   // 왼쪽으로 가다가 오른족으로 회전하는 블럭
        eSub,
        eCount
    };
    public GameObject[] basicMapPrefabs;    // 0 : mainMap prefab, 1 : road block 왼쪽, 2 : road block 오른쪽, 3 이후 : sub map prefab
    public GameObject[] seaMapPrefabs;      // 0 : mainMap prefab, 1 : road block 왼쪽, 2 : road block 오른쪽, 3 이후 : sub map prefab
    public GameObject[] desertMapPrefabs;      // 0 : mainMap prefab, 1 : road block 왼쪽, 2 : road block 오른쪽, 3 이후 : sub map prefab
    public GameObject[] christmasMapPrefabs;      // 0 : mainMap prefab, 1 : road block 왼쪽, 2 : road block 오른쪽, 3 이후 : sub map prefab
    public GameObject[] villageMapPrefabs;      // 0 : mainMap prefab, 1 : road block 왼쪽, 2 : road block 오른쪽, 3 이후 : sub map prefab
    public GameObject[] foresetMapPrefabs;      // 0 : mainMap prefab, 1 : road block 왼쪽, 2 : road block 오른쪽, 3 이후 : sub map prefab
    #endregion 
    
    public int leftTileColsFromPlayer;  // player의 좌측 tile columns 수
    public int rightTileColsFromPlayer; // player의 우측 tile columns수
    public int backTileRowsFromPlayer;  // player의 뒤쪽 tile row수
    public int frontTileRowsFromPlayer; // player의 앞쪽 tile row수
    
    List<MapBlockProperty> mapBlocks = new List<MapBlockProperty>();
    
    public MapBlockProperty GetMapBlockProperty(int index)
    {
        
        if (index < 0)
            return null;

        if (mapBlocks.Count > index)
            return mapBlocks[index];

        return null;
    }

    // main map의 크기
    public Vector3 MaxMapSize
    {
        get { return maxMapSize; }
    }

    
    

    // 맵의 크기를 계산한다.
    void CalcMapGameObjectSize()
    {
        maxMapSize  = new Vector3(0, 0, 0);

        if (mainMapPrefab)
        {
            mainMapSize = mainMapPrefab.GetComponentInChildren<Renderer>().bounds.size;
            maxMapSize  = mainMapSize;
        }
        System.Array.Resize<Vector3>(ref subMapSize, subMapPrefabs.Length);
        int idx = 0;
        foreach(var sub in subMapPrefabs)
        {
            if (sub)
            {
                subMapSize[idx] = sub.GetComponentInChildren<Renderer>().bounds.size;
            }
            else
            {
                subMapSize[idx] = new Vector3(0, 0, 0);
            }
            
            maxMapSize.x = Mathf.Max(maxMapSize.x, subMapSize[idx].x);
            maxMapSize.y = Mathf.Max(maxMapSize.x, subMapSize[idx].y);
            maxMapSize.z = Mathf.Max(maxMapSize.x, subMapSize[idx].z);
            idx++;
        }
    }

    // road blocks를 제거한다.
    // game object도 제거한다.
    void ClearMapBlocks()
    {
        foreach(var prop in mapBlocks)
        {
            prop.DeleteAllGameObjects();
            prop.DeleteItemGameObject();
        }
        mapBlocks.Clear();
    }

    // map을 구성한다.
    public void MakeMap()
    {
        lastMainMapBlockZ = -999;

        ClearMapBlocks();

         // 시작할때 roadblock을 랜덤하게 만든다.
        GenerateRoadBlockList();

        // road block object을 생성한다.
        MakeRoadBlockObjects();

        // coin object을 생성한다.
        MakeItemObjects();

        // map block object을 생성한다.
        MakeMapBlockObjects();

        // finish object를 생성한다.
        MakeFinishObject();
    }

    // 맵을 변경한다.
    public void ChangeMap(MapController.Map map)
    {
        GameObject[] mapPrefabs;
        if (map == Map.eBasic)
        {
            mapPrefabs = basicMapPrefabs;
        }
        else if (map == Map.eSea)
        {
            mapPrefabs = seaMapPrefabs;
        }
        else if(map == Map.eDesert)
        {
            mapPrefabs = desertMapPrefabs;
        }
        else if (map == Map.eChristmas)
        {
            mapPrefabs = christmasMapPrefabs;
        }
        else if(map == Map.eVillage)
        {
            mapPrefabs = villageMapPrefabs;
        }
        else if (map == Map.eForeset)
        {
            mapPrefabs = foresetMapPrefabs;
        }
        else
            return;

        // 최소한 sub prefab 1개까지는 정의가 되어 있어야 한다.
        // 안그러면 맵교체를 못한다.
        if(mapPrefabs.Length <= (int)MapPrefabIndex.eSub)
        {
            Debug.Assert(false);
            return;
        }

        mainMapPrefab = mapPrefabs[(int)MapPrefabIndex.eMain];
        roadBlock_Left_Prefab = mapPrefabs[(int)MapPrefabIndex.eLeft];
        roadBlock_Right_Prefab = mapPrefabs[(int)MapPrefabIndex.eRight];
        roadBlock_TurnToLeft_Prefab = mapPrefabs[(int)MapPrefabIndex.eTurnToLeft];
        roadBlock_TurnToRight_Prefab = mapPrefabs[(int)MapPrefabIndex.eTurnToRight];
        System.Array.Resize<GameObject>(ref subMapPrefabs, mapPrefabs.Length - (int)MapPrefabIndex.eSub);
        int idx = 0;
        for (int ix = (int)MapPrefabIndex.eSub; ix < mapPrefabs.Length; ++ix)
            subMapPrefabs[idx++] = mapPrefabs[ix];
    }

    void OnEnable()
    {
        // 데이타 체크
        CheckData();
        
    }

    void CheckData()
    { 
        foreach(var item in itemPrefabs)
        {
            if (item == null)
            {
                Debug.Assert(false);
            }
        }
    }

	// Use this for initialization
	void Start () {
        SetDefaultEnableItem();
	}

    // item 활성화를 기본값으로 설정한다.
    // 나와야 하는 것만 나오게 한다.
    public void SetDefaultEnableItem()
    {
        // enabled item 배열 초기화
        {
            System.Array.Resize<bool>(ref enabledItem, (int)MapBlockProperty.ItemType.eCount);
            for (int ix = 0; ix < enabledItem.Length; ++ix)
            {
                enabledItem[ix] = false;
            }

            // coin, diamond, life은 항상 활성화 한다.
            EnableItem(MapBlockProperty.ItemType.eCoin, true);
            EnableItem(MapBlockProperty.ItemType.eBigCoin, true);
            EnableItem(MapBlockProperty.ItemType.eDiamond, true);
            EnableItem(MapBlockProperty.ItemType.eLife, true);
        }
    }

    // road block 목록을 만든다.
    // 랜덤하게 만든다.(추후에 알고리즘 적용할 필요가 있다)
    void GenerateRoadBlockList()
    {
        mapBlocks.Clear();
        MapBlockProperty prop;
        for (int ix = 0; ix < maxRoadBlock; ++ix)
        {
            prop = new MapBlockProperty();

            // 시작은 항상 우측으로 가도록 한다.
            if (ix == 0)
                prop.Left = false;
            else
                prop.Left = Random.Range(0, 2) == 0 ? false : true;

            // 10개 전에는 항상 코인이 없다
            if (ix < 10)
                prop.Item = MapBlockProperty.ItemType.eNone;
            else
            {
                // 각 아이템을 발생주기에 따라 발생시킨다.
                for(int jx = 0; jx < itemGenerationCycle.Length; ++jx)
                {
                    if(enabledItem[jx] == false)
                        continue;

                    if (Random.Range(0, itemGenerationCycle[jx]) == 0)
                        prop.Item = (MapBlockProperty.ItemType)jx;
                }

              
            }

            mapBlocks.Add(prop);
        }


        // 장애물에 맞춰서 맵 블럭을 조정한다.
        AdjustMapBlockPropertyByBarrier();    
    }

    // 장애물에 맞춰서 맵 블럭을 조정한다.
    void AdjustMapBlockPropertyByBarrier()
    {
        MapBlockProperty prop;
        int lastAdjustIndex = -1;   // 마지막으로 조정된 index
        for (int ix = 2; ix < mapBlocks.Count-1; ++ix)
        {
            prop = mapBlocks[ix];

            if (prop.Item == MapBlockProperty.ItemType.eBlank)
            {
                // barrior은 연속될 수 없다.
                // 연속으로 나오면 이번것은 지운다
                // 방향이 다르다면 한칸 건너 띄어도 나올 수 없다.
                if (mapBlocks[ix - 1].Item == MapBlockProperty.ItemType.eBlank ||
                    lastAdjustIndex >= ix - 1)
                {
                    prop.Item = MapBlockProperty.ItemType.eNone;
                    continue;
                }

                // barrior은 앞 2칸과 다음칸이 무조건 같은 방향이어야 한다.
                mapBlocks[ix - 1].Left = prop.Left;
                mapBlocks[ix - 2].Left = prop.Left;
                mapBlocks[ix + 1].Left = prop.Left;

                lastAdjustIndex = ix + 1;
            }
        }
    }
    // main map block의 columns 수를 계산한다.
    // 기본 3개인데(좌,중간,우 포함), main map block의 크기에 따라서 달라진다.
    // 최소 1개는 들어가야한다.
    int GetMainMapBlockColumnsOnSide()
    {
        int count = Mathf.RoundToInt((float)maxMainMapColumns / mainMapSize.x);

        if (count <= 0)
            count = 1;

        return count;
    }

    // main map block이 깔려야 하는 x축 끝 좌표를 리턴한다.
    float GetXMinMainMapBlock(float x)
    {
        int left    = GetMainMapBlockColumnsOnSide() / 2;

        return x - left * mainMapSize.x;
    }

    // z를 포함하는 rectnalge의 중심 Z값을 리턴한다.
    float GetRectangleZHasZPos(float z, float rectangleHeight)
    {
        // 몇번재 rectangle인지?
        int index = (int)(z / rectangleHeight);
        if(z < 0)
        {
            index--;
        }

        return (rectangleHeight * index) + (rectangleHeight / 2.0f);
    }

    // main map block이 깔려야 하는 Z축 좌표
    // 이미 깔려 있다면 false를 리턴한다.
    bool GetZMainMapBlock(float roadBlockZ, ref float mainMapBlockZ)
    {
        // main map block의 중심 z좌표
        mainMapBlockZ = GetRectangleZHasZPos(roadBlockZ, mainMapSize.z);

        // 이미 그려져 있는 main block의 상단 좌표가 road block 중심 z좌표보다 크면 그리지 않는다.
        if (roadBlockZ < lastMainMapBlockZ + mainMapSize.z/2)
        {
            return false;
        }
        

        return true;
    }

    // player 위치 좌우로 map block game object를 만든다.
    // 보이는 것만 만든다.
    // index가 0보다 작으면 0 위치기준으로 앞쪽으로 만든다.
    // index가 최대크기보다 크면 마지막 위치를 기준으로 뒤쪽으로 만든다.
    public void MakeMapBlockRoadSideByPlayerPosition(int index, int playerPosition)
    {
        // player 기준으로 보이지 않는 경우에는 만들지 않는다.
        if (!IsInVisibleRangeByPlayerPosition(index, playerPosition))
            return;

        
        int mapBlocksIndex = index;
        if (mapBlocksIndex < 0)
            mapBlocksIndex = 0;
        if (mapBlocksIndex >= mapBlocks.Count)
            mapBlocksIndex = mapBlocks.Count - 1;

        MapBlockProperty rb = mapBlocks[mapBlocksIndex];
        Vector3 position = rb.Position;
        position.z += (index - mapBlocksIndex);

        Quaternion rotation = Quaternion.identity;
        Vector3 pos = new Vector3();
        GameObject obj;

        // road 측면에 main map block을 만든다.
        if (GetZMainMapBlock(position.z, ref pos.z))
        {
            lastMainMapBlockZ = pos.z;

            int count = GetMainMapBlockColumnsOnSide();
            
            // 좌 -> 우 로 main map block을 깐다.
            pos.x = GetXMinMainMapBlock(position.x);

            for (int ix = 0; ix < count; ++ix)
            {
                if(ix > 0)
                    pos.x += mainMapSize.x;

                pos.y = mainMapPrefab.transform.position.y;
                obj = Mem.Instantiate(mainMapPrefab, pos, rotation);
                obj.transform.parent = temp.transform;
                rb.AddGameObject(obj);
            }
        }

        // road 측면을 제외한 위치에 sub map block을 랜덤하게 만든다.
        {
            for (int left = 0; left < 2; ++left)
            {
                bool isLeft = left == 0 ? true : false;

                int subMapIndex = Random.Range(0, subMapPrefabs.Length);
                if (subMapPrefabs[subMapIndex] == null)
                    continue;

                // sub map의 크기
                Vector3 size = subMapSize[subMapIndex];

                // sub map의 위치
                pos = position;
                {
                    // road block에 폭과 위아래에 방향으로 겹치지 않도록 조정
                    if (isLeft)
                        pos.x -= (size.x / 2 + size.z / 2) + 1;
                    else
                        pos.x += (size.x / 2 + size.z / 2) + 1;

                    pos.y = subMapPrefabs[subMapIndex].transform.position.y;
                }


                obj = Mem.Instantiate(subMapPrefabs[subMapIndex], pos, subMapPrefabs[subMapIndex].transform.rotation);
                obj.transform.parent = temp.transform;
                rb.AddGameObject(obj);
            }
        }
        
    }

    
    // road 좌우로 생성되는 일반 map block을 만든다.
    void MakeMapBlocks_RoadSide()
    {
        for (int ix = -10; ix < mapBlocks.Count; ++ix)
        {
            MakeMapBlockRoadSideByPlayerPosition(ix, 0);
        }
    }

    // finish object를 만든다
    void MakeFinishObject()
    {
        if(finishPrefab == null)
        {
            Debug.Assert(false);
            return;
        }

        var property = mapBlocks[mapBlocks.Count - 1];
        GameObject obj = Mem.Instantiate(finishPrefab, property.Position, finishPrefab.transform.rotation);
        if(obj)
        {
            obj.transform.SetParent(temp.transform);
            property.AddGameObject(obj);
        }
    }

    // map block gameobject를 만든다.
    void MakeMapBlockObjects()
    {
        // map block game object의 크기 계산
        {
            CalcMapGameObjectSize();
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
        if (IsInVisibleRangeByPlayerPosition(index, playerPosition) && mapBlocks[index].Item != MapBlockProperty.ItemType.eBlank)
        {
            // 방향
            bool left = mapBlocks[index].Left;


            // prefab
            GameObject prefab = left == true ? roadBlock_Left_Prefab : roadBlock_Right_Prefab;
            // 회전이라면 상황이 달라짐
            if(index > 0 && mapBlocks[index-1].Left != left && index < mapBlocks.Count-1)
            {
                prefab = left == true ? roadBlock_TurnToLeft_Prefab : roadBlock_TurnToRight_Prefab;
            }

            pos.y = prefab.transform.position.y;

            // road block 생성
            GameObject rb = Mem.Instantiate(prefab, pos, prefab.transform.rotation);
            rb.transform.parent = temp.transform;

            // object 보관
            mapBlocks[index].AddGameObject(rb);

        }
        
        // 위치 보관
        mapBlocks[index].Position = pos;
    }

    // road block gameobject를 만든다.
    // player를 기준으로 범위내 블럭만 gameobject를 만들고, 나머지는 데이터만 생성한다.
    void MakeRoadBlockObjects()
    {
        Vector3 lastRoadBlockPos = new Vector3(0, 0, 0);
        Vector3 pos = lastRoadBlockPos;
        for (int ix = 0; ix < mapBlocks.Count; ++ix)
        {
            MakeRoadBlock(ix, pos, 0);
           
            // 마지막 위치 보관
            lastRoadBlockPos = pos;

            // 다음 위치 갱신(이번 블럭의 방향에 따라 다음블럭의 위치가 정해진다)
            {
                pos.z += 1.0f;
                if (mapBlocks[ix].Left)
                    pos.x -= 1.0f;
                else
                    pos.x += 1.0f;
            }
        }
    }

    // item gameobject를 만든다.
    // playerPosition 기준으로 보이는 것만 만든다.
    public void MakeItem(int index, Vector3 blockPosition, int playerPosition)
    {
        if (mapBlocks[index].Item == Assets.Scripts.Controller.MapBlockProperty.ItemType.eNone)
            return;

        if (IsInVisibleRangeByPlayerPosition(index, playerPosition))
        {
            Quaternion rotation = itemPrefabs[(int)mapBlocks[index].Item].transform.rotation;

            Vector3 pos = blockPosition;
            pos.y = 1.0f;

            // item 생성
            GameObject item = Mem.Instantiate(itemPrefabs[(int)mapBlocks[index].Item], pos, rotation);
            item.transform.parent = temp.transform;

            // game object 보관
            mapBlocks[index].SetItemGameObject(item);
        }
    }



    // item gameobject를 만든다.
    void MakeItemObjects()
    {
        for (int ix = 0; ix < mapBlocks.Count; ++ix)
        {
            MakeItem(ix, mapBlocks[ix].Position, 0);
        }
    }

    

	// Update is called once per frame
	void Update () {
	}
}
