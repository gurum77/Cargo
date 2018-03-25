using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Define
{
    public class Trigger
    {
        static public readonly string Jump  = "Car_Jump";
        static public readonly string Base  = "Car_Base";
        static public readonly string Level1    = "Car_Level1";
        static public readonly string Destroy   = "Car_Destroy";
        static public readonly string Shake     = "shake";
        static public readonly string Rock_Damage   = "Rock_Damage";
        static public readonly string Groggy = "Groggy";
    }

    public class Tag
    {
        static public readonly string Character = "Character";
    }

    public class Scene
    {
        static public readonly string Playground = "Playground";
        static public readonly string GameModeSelection = "GameModeSelection";
        static public readonly string MapSelection = "MapSelection";
        static public readonly string PlayerSelection = "PlayerSelection";
    }

    public class Message
    {
        static public readonly string Clash = "Clash";
    }
}
