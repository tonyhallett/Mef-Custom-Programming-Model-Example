using System;

namespace DomDocumentTest
{
    public class RecompositionAttemptEventArgs :EventArgs
    {
        public bool Success { get; set; }
        public RecompositionEventReason Reason { get; set; }
        public string FailureDetails { get; set; }

    }
}