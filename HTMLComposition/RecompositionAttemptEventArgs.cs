using System;

namespace HTMLComposition
{
    public class RecompositionAttemptEventArgs :EventArgs
    {
        internal RecompositionAttemptEventArgs(bool success, RecompositionEventReason reason,string failureDetails)
        {
            Success = success;
            Reason = reason;
            FailureDetails = failureDetails;
        }
        public bool Success { get; private set; }
        public RecompositionEventReason Reason { get; private set; }
        public string FailureDetails { get; private set; }

    }
}