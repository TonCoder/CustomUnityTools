using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Creative_Vein_Studio.Tools.Timeline_Scripts
{
    [System.Serializable]
    public class TimelineTimeMachineAsset : PlayableAsset, ITimelineClipAsset
    {
        [HideInInspector] public TimelineTimeMachineBehaviour template = new TimelineTimeMachineBehaviour();

        public TimelineTimeMachineBehaviour.TimeMachineAction action;
        public TimelineTimeMachineBehaviour.Condition condition;
        public string markerToJumpTo = "", markerLabel = "";
        public float timeToJumpTo = 0f;

        public ExposedReference<GameObject> platoon;

        public ClipCaps clipCaps
        {
            get { return ClipCaps.None; }
        }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<TimelineTimeMachineBehaviour>.Create(graph, template);
            TimelineTimeMachineBehaviour clone = playable.GetBehaviour();
            clone.objectToCheck = platoon.Resolve(graph.GetResolver());
            clone.markerToJumpTo = markerToJumpTo;
            clone.action = action;
            clone.condition = condition;
            clone.markerLabel = markerLabel;
            clone.timeToJumpTo = timeToJumpTo;

            return playable;
        }
    }
}