using UnityEngine;
using System.Collections;

namespace GameBase
{
    public static class ChannelInterface
    {
        private static string _ChannelID = "0";
        public static string GetChannelID()
        {
            return _ChannelID;
        }
    }
}
