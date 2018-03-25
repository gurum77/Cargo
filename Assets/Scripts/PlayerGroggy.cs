using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class PlayerGroggy
    {
        // 최대 그로기 시간
        public float MaxGroggyTime
        { get; set; }

        // 지금 그로기 상태인지?
        public bool IsGroggy
        { get; set; }

        // 그로기 상태로 있었던 시간
        float timeInGroggy;

        public void StartGroggy(float maxGroggyTime=1)
        {
            IsGroggy = true;
            timeInGroggy = 0;
            MaxGroggyTime = maxGroggyTime;
        }

        public void EndGroggy()
        {
            IsGroggy = false;
            timeInGroggy = 0;
        }

        // 그로기 상태 시간을 추가한다.
        public void AddTimeInGroggy(float time)
        {
            if (!IsGroggy)
                return;

            timeInGroggy += time;

            // 그로기 상태로 있었던 시간이 최대 그로기 시간보다 길어지면 그로기를 푼다.
            if(MaxGroggyTime < timeInGroggy)
            {
                IsGroggy = false;
            }
        }
    }
}
