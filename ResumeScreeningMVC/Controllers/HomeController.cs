using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;
using ResumeScreeningMVC.Models;
using DocumentFormat.OpenXml.Packaging;
using System.IO;

namespace ResumeScreeningMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly MLContext _mlContext;
        private readonly ITransformer _model;

        public HomeController()
        {
            _mlContext = new MLContext();
            _model = _mlContext.Model.Load("C:\\ProjectRepos\\ResumeScreeningMVC\\MLModel\\ResumeModel.zip", out _);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UploadResume(Microsoft.AspNetCore.Http.IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ViewBag.Message = "Please upload a valid file.";
                return View("Index");
            }

            string resumeText = ExtractTextFromFile(file);

            if (string.IsNullOrWhiteSpace(resumeText))
            {
                ViewBag.Message = "Could not extract text from the uploaded file.";
                return View("Index");
            }

            // Make prediction
            var prediction = PredictJobRole(resumeText);

            var result = new ResumeData
            {
                ResumeText = resumeText,
                JobRole = prediction
            };

            return View("Result", result);
        }

        private string ExtractTextFromFile(Microsoft.AspNetCore.Http.IFormFile file)
        {
            using var stream = file.OpenReadStream();
            if (file.FileName.EndsWith(".docx"))
            {
                using var wordDoc = WordprocessingDocument.Open(stream, false);
                return wordDoc.MainDocumentPart.Document.Body.InnerText;
            }
            else if (file.FileName.EndsWith(".txt"))
            {
                using var reader = new StreamReader(stream);
                return reader.ReadToEnd();
            }
            else
            {
                throw new InvalidDataException("Unsupported file format. Please upload a .docx or .txt file.");
            }
        }

        private string PredictJobRole(string resumeText)
        {
            var predEngine = _mlContext.Model.CreatePredictionEngine<ResumeData, ResumePrediction>(_model);
            var prediction = predEngine.Predict(new ResumeData { ResumeText = resumeText });
            return prediction.PredictedJobRole;
        }
    }
}
