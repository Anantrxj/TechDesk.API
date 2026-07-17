namespace TechDesk.API.DTOs.Dashboard
{
    public class DashboardResponseDto
    {
        public int TotalUsers { get; set; }

        public int TotalEngineers { get; set; }

        public int TotalDepartments { get; set; }

        public int TotalCategories { get; set; }

        public int TotalTickets { get; set; }

        public int OpenTickets { get; set; }

        public int AssignedTickets { get; set; }

        public int InProgressTickets { get; set; }

        public int ResolvedTickets { get; set; }

        public int ClosedTickets { get; set; }
    }
}