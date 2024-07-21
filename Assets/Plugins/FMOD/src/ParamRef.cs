using System;

namespace FMODUnity
{
    [Serializable]
    public class ParamRef
    {
        public string Name;
        public float Value;
        public FmodStudioEventEmitter.Studio.PARAMETER_ID ID;
    }
}
