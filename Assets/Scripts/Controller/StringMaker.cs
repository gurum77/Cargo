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
            
            // 모드별로 다르게 표시한다.
            GameModeController.GameMode curGameMode = GameController.Instance.gameModeController.GetCurGameMode();
            if (curGameMode == GameModeController.GameMode.eEnergyBarMode)
            {
                sb.Append(LocalizationText.GetText("Best "));
                sb.Append(GameController.Instance.Player.GameData.EnergyBarModeBestScore.ToString());
            }
            else if (curGameMode == GameModeController.GameMode.e100MMode)
            {
                sb.Append(LocalizationText.GetText("Best "));
                sb.Append(GameMode_100M.TimeToString(GameController.Instance.Player.GameData.HundredMBestTime));
            }
            else if(curGameMode == GameModeController.GameMode.eMathMode)
            {
                sb.Append(LocalizationText.GetText("Best "));
                sb.Append(GameController.Instance.Player.GameData.MathModeBestScore.ToString());
            }
            else if (curGameMode == GameModeController.GameMode.eFlagMode)
            {
                return GetFlagModeLevelString();
            }
            
            return sb.ToString();
        }

        // coin text 리턴
        public static string GetCoinsString()
        {
            return GameController.Instance.Player.GameData.Coins.ToString();
        }

        // diamond text 리턴
        public static string GetDiamondsString()
        {
            return GameController.Instance.Player.GameData.Diamonds.ToString();
        }

        // collected coin text 리턴
        public static string GetCollectedCoinsString()
        {
            return GameController.Instance.Player.CollectedCoins.ToString();
        }

        // collected diamond text 리턴
        public static string GetCollectedDiamondsString()
        {
            return GameController.Instance.Player.CollectedDiamonds.ToString();
        }
        

        // flag 모드 레벨 문자열
        public static string GetFlagModeLevelString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Stage ");

            GameModeController.GameMode curGameMode = GameController.Instance.gameModeController.GetCurGameMode();
            if (curGameMode == GameModeController.GameMode.eFlagMode)
            {
                sb.Append(GameController.Instance.Player.GameData.FlagModeLevel.ToString());
            }

            return sb.ToString();
        }

        // player flag 개수 문자열
        public static string GetPlayerFlagCountString()
        {
            StringBuilder sb = new StringBuilder();
            GameModeController.GameMode curGameMode = GameController.Instance.gameModeController.GetCurGameMode();
            if(curGameMode == GameModeController.GameMode.eFlagMode)
            {
                sb.Append(GameController.Instance.Player.FlagCount.ToString());
            }

            return sb.ToString();
        }

        // com flag 개수 문자열
        public static string GetComFlagCountString()
        {
            StringBuilder sb = new StringBuilder();
            GameModeController.GameMode curGameMode = GameController.Instance.gameModeController.GetCurGameMode();
            if (curGameMode == GameModeController.GameMode.eFlagMode)
            {
                GameMode_Flag mode = GameController.Instance.gameModeController.gameModes[(int)curGameMode].GetComponent<GameMode_Flag>();
                if(mode)
                {
                    sb.Append(mode.Com.FlagCount.ToString());
                }
            }

            return sb.ToString();
        }

        // score text
        public static string GetScoreString()
        {
            // 모드별로 다르게 표시한다.
            GameModeController.GameMode curGameMode = GameController.Instance.gameModeController.GetCurGameMode();
            if (curGameMode == GameModeController.GameMode.eEnergyBarMode ||
                curGameMode == GameModeController.GameMode.eFlagMode ||
                curGameMode == GameModeController.GameMode.eMathMode)
            {
                return GameController.Instance.Player.Score.ToString();
            }
            else if (curGameMode == GameModeController.GameMode.e100MMode)
            {
                GameMode_100M gameMode100M = GameController.Instance.gameModeController.gameModes[(int)curGameMode].GetComponent<GameMode_100M>();
                if(gameMode100M)
                    return GameMode_100M.TimeToString(gameMode100M.Time100M);
            }

            return "";
        }

        //  실제 기본 하트 문자열
        public static string GetRealDefaultLifeString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("+");
            sb.Append(GameController.Instance.player.GetRealDefaultLife().ToString());

          

            return sb.ToString();
        }

        //  실제 하트 문자열
        public static string GetRealLifeString()
        {
            return GameController.Instance.player.Life.ToString();

        }

        //  실제 코인 획득률 문자열 
        public static string GetRealCoinRateString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("+");
            sb.Append(((GameController.Instance.player.GetRealCoinRate() - 1.0f) * 100.0f + (GameController.Instance.player.GetTransformLevel() * 100)).ToString());
            sb.Append("%");
            
            return sb.ToString();
        }

        //  실제 파워 문자열 
        public static string GetRealPowerString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("+");
            sb.Append(GameController.Instance.player.GetRealPower().ToString());

            return sb.ToString();
        }

        //  실제 스피드 문자열
        public static string GetRealSpeedString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("+");
            sb.Append(GameController.Instance.player.GetRealSpeed().ToString());

            return sb.ToString();
        }

        // level 문자열
        public static string GetLevelString()
        {
            return GameController.Instance.Player.Level.ToString();
        }

        // epx 문자열
        public static string GetExpString()
        {
            // 현재 경험치
            int curExp  = GameController.Instance.Player.Exp;

            // 다음 레벨을 위한 경험치
            int nextExp = GameController.Instance.player.ExpForNextLevel;
            StringBuilder sb    = new StringBuilder();
            sb.Append(curExp.ToString());
            sb.Append("/");
            sb.Append(nextExp.ToString());
            
            return sb.ToString();
        }
    }
}
