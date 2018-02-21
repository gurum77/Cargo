using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Controller
{
    // 문자열을 만들어 내는 클래스
    public class StringMaker
    {
        // best score string 리턴
        public static string GetBestScoreString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Best ");
            // 모드별로 다르게 표시한다.
            GameModeController.GameMode curGameMode = GameController.Me.gameModeController.GetCurGameMode();
            if (curGameMode == GameModeController.GameMode.eEnergyBarMode)
            {
                sb.Append(GameController.Me.Player.GameData.EnergyBarModeBestScore.ToString());
            }
            else if (curGameMode == GameModeController.GameMode.e100MMode)
            {
                sb.Append(GameMode_100M.TimeToString(GameController.Me.Player.GameData.HundredMBestTime));
            }
            
            return sb.ToString();
        }

        // coin text 리턴
        public static string GetCoinString()
        {
            return GameController.Me.Player.GameData.Coins.ToString();
        }

        // score text
        public static string GetScoreString()
        {
            // 모드별로 다르게 표시한다.
            GameModeController.GameMode curGameMode = GameController.Me.gameModeController.GetCurGameMode();
            if (curGameMode == GameModeController.GameMode.eEnergyBarMode)
            {
                return GameController.Me.Player.Score.ToString();
            }
            else if (curGameMode == GameModeController.GameMode.e100MMode)
            {
                GameMode_100M gameMode100M = GameController.Me.gameModeController.gameModes[(int)curGameMode].GetComponent<GameMode_100M>();
                if(gameMode100M)
                    return GameMode_100M.TimeToString(gameMode100M.Time100M);
            }

            return "";
        }
        
    }
}
