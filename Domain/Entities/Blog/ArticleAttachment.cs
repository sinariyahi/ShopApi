using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Domain.Entities.Blog;
using Infrastructure.Common;

namespace Domain.Entities.Blog
{
    [Table("ArticleAttachments", Schema = "Blog")]
    public class ArticleAttachment
    {
        [Key]
        public Guid Id { get; set; }
        public int ArticleId { get; set; }
        public virtual Article Article { get; set; }

        [MaxLength(128)]
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string FileContentType { get; set; }
        public string FileSize { get; set; }
        public FileType FileType { get; set; }
        public string FilePath { get; set; }
        public DateTime CreateDate { get; set; }

    }
    public enum FriendlyColorsEnum
    {
        [Description("Blanched Almond Color")]
        BlanchedAlmond = 1,
        [Description("Dark Sea Green Color")]
        DarkSeaGreen = 2,
        [Description("Deep Sky Blue Color")]
        DeepSkyBlue = 3,
        [Description("Rosy Brown Color")]
        RosyBrown = 4
    }
    public enum SomeEnum
    {
        [Display(Name = "Some Name")]
        SomeValue=1,
        
    }
    public partial class PersonViewModel
    {
        public string Fname { get; set; }
    }

    [MetadataType(typeof(PersonViewModelMetaData))]
    public partial class PersonViewModel
    {
    }

    public class PersonViewModelMetaData
    {
        [Required(ErrorMessage = "First name is required.")]
        public string Fname { get; set; }
    }


    
}
//public EntityRef<Article> _Customer;
//// ...
//[Association(Name = "FK_Orders_Customers", Storage = "_Customer", ThisKey = "CustomerID", IsForeignKey = true)]
//public Customer Customer
//{
//    get
//    {
//        return this._Customer.Entity;
//    }
//    set
//    {
//        Customer previousValue = this._Customer.Entity;
//        if (((previousValue != value)
//                    || (this._Customer.HasLoadedOrAssignedValue == false)))
//        {
//            this.SendPropertyChanging();
//            if ((previousValue != null))
//            {
//                this._Customer.Entity = null;
//                previousValue.Orders.Remove(this);
//            }
//            this._Customer.Entity = value;
//            if ((value != null))
//            {
//                value.Orders.Add(this);
//                this._CustomerID = value.CustomerID;
//            }
//            else
//            {
//                this._CustomerID = default(string);
//            }
//            this.SendPropertyChanged("Customer");
//        }
//    }
//}


