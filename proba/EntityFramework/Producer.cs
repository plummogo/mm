//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace proba.EntityFramework
{
    using System;
    using System.Collections.Generic;
    
    public partial class Producer
    {
        public Producer()
        {
            this.CarsProducers = new HashSet<CarsProducers>();
        }
    
        public int id { get; set; }
        public string producer1 { get; set; }
        public int cat_id { get; set; }
    
        public virtual ICollection<CarsProducers> CarsProducers { get; set; }
        public virtual Category Category { get; set; }
    }
}
