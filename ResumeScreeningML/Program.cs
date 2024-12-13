using System;
using Microsoft.ML;
using Microsoft.ML.Data;
using ResumeScreeningML;

class Program
{
    public static  MLContext _mlContext;
    public static  ITransformer _model;
    static void Main(string[] args)
    {
        //var mlContext = new MLContext();

        //// Load data
        //string dataPath = "C:\\ProjectRepos\\ResumeScreeningML\\DataSet\\UpdatedResumeDataSet.csv";
        //IDataView dataView = mlContext.Data.LoadFromTextFile<ResumeData>(dataPath, separatorChar: ',', hasHeader: true);

        //// Split data into training and testing sets
        //var splitData = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
        //var trainingData = splitData.TrainSet;
        //var testData = splitData.TestSet;

        //// Define data processing and training pipeline
        //var pipeline = mlContext.Transforms.Text.FeaturizeText("Features", nameof(ResumeData.ResumeText))
        //    .Append(mlContext.Transforms.Conversion.MapValueToKey("Label", nameof(ResumeData.JobRole)))
        //    .Append(mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy())
        //    .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

        //// Train the model
        //var model = pipeline.Fit(trainingData);

        //// Evaluate the model
        //var testMetrics = mlContext.MulticlassClassification.Evaluate(model.Transform(testData));
        //Console.WriteLine($"MicroAccuracy: {testMetrics.MicroAccuracy}");
        //Console.WriteLine($"MacroAccuracy: {testMetrics.MacroAccuracy}");

        //// Save the model
        //mlContext.Model.Save(model, trainingData.Schema, "C:\\ProjectRepos\\ResumeScreeningML\\Models\\ResumeModel_1.zip");

        //Console.WriteLine("Model training complete.");

        _mlContext = new MLContext();
        _model = _mlContext.Model.Load("C:\\ProjectRepos\\ResumeScreeningML\\Models\\ResumeModel_1.zip", out _);


        Predict(_mlContext, "SKILLS C Basics, IOT, Python, MATLAB, Data Science, Machine Learning, HTML");
        Predict(_mlContext, "SKILLS C Basics, IOT, Python, MATLAB, Data Science, Machine Learning, HTML");

        Console.ReadKey();
    }

    static void Predict(MLContext mlContext, string resumeText)
    {
        // Load the model
        ITransformer trainedModel = mlContext.Model.Load("C:\\ProjectRepos\\ResumeScreeningML\\Models\\ResumeModel_1.zip", out var modelInputSchema);

        // Create a prediction engine
        var predEngine = mlContext.Model.CreatePredictionEngine<ResumeData, ResumePrediction>(trainedModel);

        // Predict job role
        var prediction = predEngine.Predict(new ResumeData { ResumeText = resumeText });
        Console.WriteLine($"Predicted Job Role: {prediction.PredictedJobRole}");
    }

}
