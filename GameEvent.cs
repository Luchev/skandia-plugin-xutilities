using Plugins;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xUtilities
{
    public class GameEvent
    {
        public string Name { get; set; }
        public uint Bait { get; set; }
        public uint Map { get; set; }
        public Vector2 Location2D { get; set; }
        public Vector3 Location3D { get; set; }
        public EventType Type { get; set; }
        public List<DayOfWeek> Days { get; set; }
        public List<TimeSpan> StartTimes { get; set; }
        public List<TimeSpan> EndTimes { get; set; }
        public string Tooltip { get; set; }
        public string Note { get; set; }
        
        public bool IsOnGoing()
        {
            var serverTime = ObjectManager.CurrentServerTime;
            bool IsToday = Days.Contains(serverTime.DayOfWeek);
            bool IsRunning = false;
            TimeSpan time = serverTime.TimeOfDay + TimeSpan.FromSeconds(DateTime.Now.Second);
            for (int i = 0; i < StartTimes.Count; i++)
            {
                if (StartTimes[i] <= time && time <= EndTimes[i])
                {
                    IsRunning = true;
                }
            }
            return IsToday && IsRunning;
        }
        public TimeSpan OnGoingEndTime()
        {
            var serverTime = ObjectManager.CurrentServerTime;
            bool IsToday = Days.Contains(serverTime.DayOfWeek);
            TimeSpan time = serverTime.TimeOfDay + TimeSpan.FromSeconds(DateTime.Now.Second);
            for (int i = 0; i < StartTimes.Count; i++)
            {
                if (StartTimes[i] <= time && time <= EndTimes[i])
                {
                    return EndTimes[i];
                }
            }
            return new TimeSpan();
        }

        public bool HasMoreEventsToday()
        {
            var serverTime = ObjectManager.CurrentServerTime;
            bool LeftEventsToday = true;
            if (serverTime.TimeOfDay >= EndTimes[EndTimes.Count-1])
            {
                LeftEventsToday = false;
            }
            return LeftEventsToday;
        }
    }

    public class GameEventSerializeable
    {
        public string Name { get; set; }
        public uint Bait { get; set; }
        public uint Map { get; set; }
        public Vector2 Location2D { get; set; }
        public Vector3 Location3D { get; set; }
        public EventType Type { get; set; }
        public List<DayOfWeek> Days { get; set; }
        public List<int> StartTimes { get; set; }
        public List<int> EndTimes { get; set; }
        public string Tooltip { get; set; }
        public string Note { get; set; }
    }

    public enum EventType
    {
        FishKing,
        WorldBoss,
        GuildBoss,
        Battlefield,
        DungeonReset,
        Raid,
        Quiz,
        CardRanger,
        Custom
    }
    
    public class GameEventOld
    {
        public string Name = "";
        public string Bait = "";
        public float X = 0;
        public float Y = 0;
        public string Type = "";
        public List<int> Days = new List<int>();
        public List<int> StartTimes = new List<int>();
        public List<int> EndTimes = new List<int>();
        public string Map = "";
        public string Tooltip = "";
    }
}
