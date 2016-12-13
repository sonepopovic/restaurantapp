using System.ComponentModel.DataAnnotations;

namespace RestaurantFacultyApplication.Models
{
    public class ReservationParameters
    {
        
        public  int Id { get; set; }
        [Display(Name="Name")]
        public string Name { get; set; }
        [Display(Name = "Reservation time")]
        public System.DateTime ReservationTime { get; set; }
        [Display(Name = "Duration")]
        public short Duration { get; set; } 
    }
}