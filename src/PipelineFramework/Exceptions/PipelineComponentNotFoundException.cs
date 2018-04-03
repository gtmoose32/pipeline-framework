using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace PipelineFramework.Exceptions
{

    [Serializable]
    public class PipelineComponentNotFoundException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //
        public PipelineComponentNotFoundException(string message) : base(message)
        {
        }

        [ExcludeFromCodeCoverage]
        protected PipelineComponentNotFoundException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        { }
    }
}
