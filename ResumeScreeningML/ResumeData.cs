using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML.Data;

namespace ResumeScreeningML
{


    public class ResumeData
    {
        [LoadColumn(0)]
        public string ResumeText { get; set; }

        [LoadColumn(1)]
        public string JobRole { get; set; }
    }

    public class ResumePrediction
    {
        [ColumnName("PredictedLabel")]
        public string PredictedJobRole { get; set; }
    }

}
