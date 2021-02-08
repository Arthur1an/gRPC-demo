using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcGreeter
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        private readonly GrpcGreeter.Entity.LeaveDbContext  _dbContext;
        public GreeterService(GrpcGreeter.Entity.LeaveDbContext dbContext, ILogger<GreeterService> logger)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            var test=_dbContext.Employees.FirstOrDefault(x => x.Id == 1);
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }

        public override Task<HelloReply> Saybyby(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "byby " + request.Name
            });
        }
    }
}
