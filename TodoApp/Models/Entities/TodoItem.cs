using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TodoApp.Models.Entities
{
    public class TodoItem
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }

        [Required]
        [JsonInclude]
        public string TodoName { get; set; }

        [JsonInclude]
        public string? TodoDescription { get; set; }

        [JsonInclude]
        public DateTime? RemindMeDate { get; set; }

        [Required]
        [JsonInclude]
        public int TodoPriority { get; set; }

        [Required]
        public string TaskOwner { get; set; }

        public DateTime? CreatedDate { get; set; } 
        public DateTime? LastModifiedDate { get; set; } 

        [JsonInclude]
        public List<TodoItemComment>? ItemComments { get; set; }
    }
}
