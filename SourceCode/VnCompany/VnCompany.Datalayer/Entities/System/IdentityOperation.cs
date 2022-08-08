using System.Collections.Generic;

namespace VnCompany.DataLayer.Entities
{
    public class IdentityOperation
    {
        public int Id { get; set; }
        public string OperationName { get; set; }
        public bool Enabled { get; set; }
        public string AccessId { get; set; }
        public string ActionName { get; set; }

        public string AccessName { get; set; }
        public string ControllerName { get; set; }
        public int IndexOrder { get; set; }
        public List<IdentityOperationLang> LangList { get; set; }

        public IdentityOperation()
        {
            LangList = new List<IdentityOperationLang>();
        }
    }
}
