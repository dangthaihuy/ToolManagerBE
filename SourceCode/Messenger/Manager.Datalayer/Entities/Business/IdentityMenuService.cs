using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.DataLayer.Entities.Business
{
    [Serializable]
    public class IdentityMenuService : CommonIdentity
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public int Status { get; set; }

        public List<IdentityWorkFlow> WorkFlows { get; set; }

        public IdentityMenuService()
        {
            WorkFlows = new List<IdentityWorkFlow>();
        }
    }

    [Serializable]
    public class IdentityMenuWorkFlow
    {
        public int Id { get; set; }
        public int MenuId { get; set; }
        public int WorkFlowId { get; set; }
        public int SortOrder { get; set; }
    }
}
