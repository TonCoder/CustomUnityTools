using UnityEngine;
using UnityEngine.Playables;

namespace Creative_Vein_Studio.Tools.Timeline_Scripts
{
    public class TimelineTimeMachineBehaviour : PlayableBehaviour
    {
        public TimeMachineAction action;
        public Condition condition;
        public string markerToJumpTo, markerLabel;
        public float timeToJumpTo;
        public GameObject objectToCheck;

        [HideInInspector] public bool clipExecuted = false; //the user shouldn't author this, the Mixer does

        public bool ConditionMet()
        {
            switch (condition)
            {
                case Condition.Always:
                    return true;

                case Condition.PlatoonIsAlive:
                    //The Timeline will jump to the label or time if object is active
                    if (objectToCheck != null)
                    {
                        return objectToCheck.activeSelf;
                    }
                    else
                    {
                        return false;
                    }

                case Condition.Never:
                default:
                    return false;
            }
        }

        public enum TimeMachineAction
        {
            Marker,
            JumpToTime,
            JumpToMarker,
            Pause,
        }

        public enum Condition
        {
            Always,
            Never,
            PlatoonIsAlive,
        }
    }
}