using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace FMODUnity
{
    [AddComponentMenu("FMOD Studio/FMOD Studio Global Parameter Trigger")]
    public class StudioGlobalParameterTrigger: EventHandler
    {
        [ParamRef]
        [FormerlySerializedAs("parameter")]
        public string Parameter;

        public EmitterGameEvent TriggerEvent;

        [FormerlySerializedAs("value")]
        public float Value;

        private FmodStudioEventEmitter.Studio.PARAMETER_DESCRIPTION parameterDescription;
        public FmodStudioEventEmitter.Studio.PARAMETER_DESCRIPTION ParameterDescription { get { return parameterDescription; } }

        protected override void HandleGameEvent(EmitterGameEvent gameEvent)
        {
            if (TriggerEvent == gameEvent)
            {
                TriggerParameters();
            }
        }

        public void TriggerParameters()
        {
            bool paramNameSpecified = !string.IsNullOrEmpty(Parameter);
            if (paramNameSpecified)
            {
                FmodStudioEventEmitter.RESULT result = FmodStudioEventEmitter.RESULT.OK;
                bool paramIDNeedsLookup = string.IsNullOrEmpty(parameterDescription.name);
                if (paramIDNeedsLookup)
                {
                    result = RuntimeManager.StudioSystem.getParameterDescriptionByName(Parameter, out parameterDescription);
                    if (result != FmodStudioEventEmitter.RESULT.OK)
                    {
                        RuntimeUtils.DebugLogError(string.Format(("[FMOD] StudioGlobalParameterTrigger failed to lookup parameter {0} : result = {1}"), Parameter, result));
                        return;
                    }
                }

                result = RuntimeManager.StudioSystem.setParameterByID(parameterDescription.id, Value);
                if (result != FmodStudioEventEmitter.RESULT.OK)
                {
                    RuntimeUtils.DebugLogError(string.Format(("[FMOD] StudioGlobalParameterTrigger failed to set parameter {0} : result = {1}"), Parameter, result));
                    return;
                }
            }
        }
    }
}
