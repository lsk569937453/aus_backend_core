using System;
namespace netcoreTest.entity
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
