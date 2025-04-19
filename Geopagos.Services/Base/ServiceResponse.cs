namespace Geopagos.Services.Base
{
    public class ServiceResponse<T>
    {
        public int ReturnValue { get; set; }
        public string ReturnCode { get; set; }
        public List<ServiceError> Errors { get; set; } = [];
        public T Data { get; set; }

        public bool Status => Errors.Count == 0;

        public void AddError(Exception ex)
        {
            Errors.Add(new ServiceError(ex));
        }
        public void AddError(string errorMessage)
        {
            Errors.Add(new ServiceError(errorMessage));
        }
        public void AddError(string errorCode, string errorMessage)
        {
            Errors.Add(new ServiceError(errorCode, errorMessage));
        }
        public void AddError(string errorCode, string errorMessage, ServiceErrorLevel errorLevel)
        {
            Errors.Add(new ServiceError(errorCode, errorMessage, errorLevel));
        }
        public void AddError(ServiceError serviceError)
        {
            Errors.Add(serviceError);
        }
        public void AddErrors(List<ServiceError> serviceErrorList)
        {
            foreach (var e in serviceErrorList)
                Errors.Add(e);
        }
    }

    public class ServiceResponse : ServiceResponse<object> { }

}
