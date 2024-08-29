using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class GeneralSettings
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        public Employee ?Employee { get; set; }
        [Required]
        //positive
 
        [Range(0, int.MaxValue, ErrorMessage = "Hour Con not be less than 0")]
        public decimal bonusPerHoure { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Hour Con not be less than 0")]
        public decimal rivalPerHour { get; set; }


        [Required]
        public string WeeklyHolidays { get; set; }


        [NotMapped]
        [Required]
        public List<Day> WeeklyHolidayList
        {
            get
            {
                return string.IsNullOrEmpty(WeeklyHolidays)
                    ? new List<Day>()
                    : WeeklyHolidays.Split(',')
                        .Select(day => (Day)Enum.Parse(typeof(Day), day))
                        .ToList();
            }
            set
            {
                WeeklyHolidays = string.Join(",", value.Select(day => day.ToString()));
            }
        }
    }
}
