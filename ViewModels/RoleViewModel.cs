using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Display(Name="Role Name")]
        public string Name { get; set; }
        public RoleViewModel()
        {
            Id= Guid.NewGuid().ToString();
        }
    }
}
