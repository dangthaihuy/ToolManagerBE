using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.DataLayer.Entities.Business
{
    [Serializable]
    public class IdentityForm : CommonIdentity
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }    
        public string ShortDescription { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public int Status { get; set; }

        public List<IdentityFormField> Fields { get; set; }
        public IdentityForm()
        {
            Fields = new List<IdentityFormField>();
        }
    }

    [Serializable]
    public class IdentityFormField
    {
        public int Id { get; set; }
        public int FormId { get; set; }
        public string Template { get; set; }
        public IdentityFormFieldTemplate TemplateInfo { get; set; }
    }

    [Serializable]
    public class EmployeeFillFore
    {
        public int Id { get; set; }
        public int FormId { get; set; }
        public int FormFieldId { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        public int? EmployeeId { get; set; }
    }

    public class IdentityFormFieldTemplate
    {        
        public int FieldId { get; set; }
        public int FormId { get; set; }
        public string label { get; set; }
        public string labelPosition { get; set; }
        public int labelWidth { get; set; }
        public int labelMargin { get; set; }
        public string placeholder { get; set; }
        public string description { get; set; }
        public int rows { get; set; }
        public string prefix { get; set; }
        public string suffix { get; set; }
        public bool tableView { get; set; }
        public string type { get; set; }
        public string key { get; set; }
        public bool input { get; set; }
    }
}
