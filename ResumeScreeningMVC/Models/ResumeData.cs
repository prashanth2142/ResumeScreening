using Microsoft.ML.Data;

namespace ResumeScreeningMVC.Models
{
    public class ResumeData
    {
        public string ResumeText { get; set; }
        public string JobRole { get; set; }
    }
    public class ResumePrediction
    {
        [ColumnName("PredictedLabel")]
        public string PredictedJobRole { get; set; }
    }
}
