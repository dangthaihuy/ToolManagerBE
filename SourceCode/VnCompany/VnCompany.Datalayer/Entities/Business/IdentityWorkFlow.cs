using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VnCompany.DataLayer.Entities.Business
{
    [Serializable]
    public class IdentityWorkFlow : CommonIdentity
    {
        public int Id { get; set; }
       
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set;}
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public int Status { get; set; }

        public List<IdentityForm> Forms { get; set; }
        public int? WorkFlowFormId { get; set; }

        public IdentityWorkFlow()
        {
            Forms = new List<IdentityForm>();
        }
    }

    [Serializable]
    public class IdentityWorkFlowForm
    {
        public int Id { get; set; }
        public int WorkFlowId { get; set; }
        public int FormId { get; set; }
        public int? SortOrder { get; set; }
    }
}
