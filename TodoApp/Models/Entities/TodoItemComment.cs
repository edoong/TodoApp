using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TodoApp.Models.Entities
{
    public class TodoItemComment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [MaxLength(255)]
        [JsonInclude]
        public virtual string CommentText { get; set; }
        
        [Required]
        [JsonInclude]
        public virtual string UserId { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual DateTime? LastModifiedDate { get; set; }
        [JsonInclude]
        public virtual int TodoItemId { get; set; }

        [NotMapped]
        [JsonInclude]
        public virtual string? UserName { get; set; }
    }
}
