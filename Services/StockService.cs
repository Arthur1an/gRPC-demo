using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using LeaveDemo;
using Microsoft.Extensions.Logging;
using StockServices.Protos;

namespace StockServices.Services
{
    public class StockService : Stock.StockBase
    {

        private static List<Product> _allProducts = new List<Product>();
        private readonly ILogger<StockService> _logger;
        private readonly GrpcGreeter.Entity.LeaveDbContext _dbContext;
        public StockService(GrpcGreeter.Entity.LeaveDbContext dbContext, ILogger<StockService> logger)
        {
            _logger = logger;
            _dbContext = dbContext;
        }


        public override Task<Result> AddProduct(Product request, ServerCallContext context)
        {

            if (string.IsNullOrEmpty(request.Name))
                return Task.FromResult<Result>(new Result { Msg = "Product Name Can't be nulled", Status = false });

            if (_allProducts.FirstOrDefault(f => f.Code == request.Code) != null)
                return Task.FromResult<Result>(new Result { Msg = "Product is already Added", Status = false });

            GrpcGreeter.Entity.Stock s = new GrpcGreeter.Entity.Stock();
            s.Name = request.Name;
            s.Code =Convert.ToInt32(request.Code);
            _dbContext.Stock.Add(s);
            _dbContext.SaveChanges();
            var model = _dbContext.Stock.ToList();
            int count = model.Count();

            _allProducts.Add(request);
            return Task.FromResult<Result>(new Result { Msg = "Added. Total Products: " + model.Count.ToString(), Status = true });

        }


        public override async Task GetAllProducts(Empty request, IServerStreamWriter<Product> responseStream, ServerCallContext context)
        {
            var model = _dbContext.Stock.ToList();
            List<Product> list = new List<Product>();
            foreach (var item in model)
            {
                Product pt = new Product();
                pt.Name = item.Name;
                pt.Code =Convert.ToUInt32(item.Code);
                list.Add(pt);
            }
            foreach (var each in list)
            {
                await responseStream.WriteAsync(each);
            }

        }


    }
}