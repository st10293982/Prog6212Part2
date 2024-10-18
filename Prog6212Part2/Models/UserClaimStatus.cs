using System;

namespace Prog6212Part2.Models
{
    public class UserClaimStatus
    {
        public int Id { get; set; }

        // The status of the claim: Pending, Approved, Rejected
        public string Status { get; set; }

        // The progress of the claim as a percentage (0-100)
        public int Progress { get; set; }

        // Class to determine the color of the progress bar (e.g., success, warning, danger)
        public string StatusClass { get; set; }

        // A message to display based on the claim status
        public string StatusMessage { get; set; }
    }
}
