using Shared.LogData.ELK.Abstract;
using Shared.LogData.ELK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.LogData.ELK.Concrete
{
    public class ElasticsearchLogService : IElasticsearchLogService
    {
        public Task<LogDetail> AddAsync(LogDetail logDetail)
        {
            throw new NotImplementedException();
        }

        public Task<List<LogDetail>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<LogDetail>> GetAllByRiskAsync(byte risk)
        {
            throw new NotImplementedException();
        }

        public Task<List<LogDetail>> GetMatchingByMethodNameAsync(string methodName)
        {
            throw new NotImplementedException();
        }

        public Task<List<LogDetail>> SearchAsync(string keyword)
        {
            throw new NotImplementedException();
        }

        public Task<List<LogDetail>> SearchPagingAsync(string keyword, int page, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}
