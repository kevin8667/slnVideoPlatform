namespace VdbAPI.Member.ViewModel
{
    public class ReturnResult<T>
    {
        public bool IsSuccess { get; set; }
        public string AlertMsg { get; set; }
        public bool HasAlertMsg { get {return  !string.IsNullOrEmpty(AlertMsg); } }

        public T Data { get; set; }
        public List<T> Datas { get; set; }
    }

    public class ReturnResult
    {
        public bool IsSuccess { get; set; }
        public bool HasAlertMsg { get { return string.IsNullOrEmpty(AlertMsg); } }
        public string AlertMsg { get; set; }
     
    }

}
