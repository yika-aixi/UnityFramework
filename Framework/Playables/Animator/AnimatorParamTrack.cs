using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Framework
{
	namespace Playables
	{
		[TrackColor(180f / 255f, 115f / 255f, 215f / 255f)]
		[TrackBindingType(typeof(Animator))]
		[TrackClipType(typeof(AnimatorParamClipAsset))]
		public class AnimatorParamTrack : AnimatedTrack, IParentBindableTrack
		{
			public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
			{
				ScriptPlayable<AnimatorParamTrackMixer> playable = TimelineUtils.CreateTrackMixer<AnimatorParamTrackMixer>(this, graph, go, inputCount);

				ParentBindableTrack.OnCreateTrackMixer(this, playable.GetBehaviour(), graph);

				return playable;
			}
		}
	}
}