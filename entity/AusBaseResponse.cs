using System;
namespace aus_backend_core.entity
{
    public class AusBaseResponse
    {
        public AusBaseResponse()
        {
        }

        public int responseCode { get; set; }
        public Object responseBody { get; set; }

    }
}
