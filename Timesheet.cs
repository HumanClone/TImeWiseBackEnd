namespace TimeWise
{
    public class Timesheet
    {
        public string? TimesheetId { get; set; }
        public string? CategoryId { get; set; }
        public string? PictureId { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}