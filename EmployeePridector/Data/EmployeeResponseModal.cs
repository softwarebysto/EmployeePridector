namespace EmployeePridector.Data
{
    public class EmployeeResponseModal
    {
        public PredictedData? predicted_data { get; set; }
    }
    public class PredictedData
    {
        public List<double>? accuracy { get; set; }
        public List<double>? precision { get; set; }
        public List<double>? recall { get; set; }
        public List<double>? predicted { get; set; }
    }
}
