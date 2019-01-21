using System;
using System.Collections.Generic;

namespace PipelineFramework.Core.Examples
{
    public class ExamplePipelinePayload
    {
        public ExamplePipelinePayload()
        {
            Messages = new List<string>();
        }

        public Guid FooKey { get; set; }
        public List<string> Messages { get; }
        public int Result { get; set; }
    }
}