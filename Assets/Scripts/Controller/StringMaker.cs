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
            sb.Append(LocalizationText.GetText("Best "));
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
            else if(curGameMode == GameModeController.GameMode.eMathMode)
            {
                sb.Append(GameController.Me.Player.GameData.MathModeBestScore.ToString());
            }
            
            return sb.ToString();
        }

        // coin text 리턴
        public static string GetCoinsString()
        {
            return GameController.Me.Player.GameData.Coins.ToString();
        }

        // diamond text 리턴
        public static string GetDiamondsString()
        {
            return GameController.Me.Player.GameData.Diamonds.ToString();
        }

        // collected coin text 리턴
        public static string GetCollectedCoinsString()
        {
            return GameController.Me.Player.CollectedCoins.ToString();
        }

        // collected diamond text 리턴
        public static string GetCollectedDiamondsString()
        {
            return GameController.Me.Player.CollectedDiamonds.ToString();
        }
        

        // flag 모드 레벨 문자열
        public static string GetFlagModeLevelString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("LEVEL ");

            GameModeController.GameMode curGameMode = GameController.Me.gameModeController.GetCurGameMode();
            if (curGameMode == GameModeController.GameMode.eFlagMode)
            {
                sb.Append(GameController.Me.Player.GameData.FlagModeLevel.ToString());
            }

            return sb.ToString();
        }

        // player flag 개수 문자열
        public static string GetPlayerFlagCountString()
        {
            StringBuilder sb = new StringBuilder();
            GameModeController.GameMode curGameMode = GameController.Me.gameModeController.GetCurGameMode();
            if(curGameMode == GameModeController.GameMode.eFlagMode)
            {
                sb.Append(GameController.Me.Player.FlagCount.ToString());
            }

            return sb.ToString();
        }

        // com flag 개수 문자열
        public static string GetComFlagCountString()
        {
            StringBuilder sb = new StringBuilder();
            GameModeController.GameMode curGameMode = GameController.Me.gameModeController.GetCurGameMode();
            if (curGameMode == GameModeController.GameMode.eFlagMode)
            {
                GameMode_Flag mode = GameController.Me.gameModeController.gameModes[(int)curGameMode].GetComponent<GameMode_Flag>();
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
            GameModeController.GameMode curGameMode = GameController.Me.gameModeController.GetCurGameMode();
            if (curGameMode == GameModeController.GameMode.eEnergyBarMode ||
                curGameMode == GameModeController.GameMode.eFlagMode ||
                curGameMode == GameModeController.GameMode.eMathMode)
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

        //  실제 기본 하트 문자열
        public static string GetRealDefaultLifeString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("+");
            sb.Append(GameController.Me.player.GetRealDefaultLife().ToString());

          

            return sb.ToString();
        }

        //  실제 하트 문자열
        public static string GetRealLifeString()
        {
            return GameController.Me.player.Life.ToString();

        }

        //  실제 코인 획득률 문자열 
        public static string GetRealCoinRateString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("+");
            sb.Append(((GameController.Me.player.GetRealCoinRate() - 1.0f) * 100.0f).ToString());
            sb.Append("%");
            if (GameController.Me.player.GetLevel() > 0)
            {
                sb.Append(" +");
                sb.Append((GameController.Me.player.GetLevel() * 100).ToString());
                sb.Append("%");
            }
            return sb.ToString();
        }

        //  실제 파워 문자열 
        public static string GetRealPowerString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("+");
            sb.Append(GameController.Me.player.GetRealPower().ToString());

            return sb.ToString();
        }

        //  실제 스피드 문자열
        public static string GetRealSpeedString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("+");
            sb.Append(GameController.Me.player.GetRealSpeed().ToString());

            return sb.ToString();
        }
    }
}
