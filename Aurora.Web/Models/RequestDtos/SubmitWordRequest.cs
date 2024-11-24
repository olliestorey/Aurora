namespace Aurora.Web.Models.RequestDtos
{
    public class SubmitWordRequest
    {
        public string RoomCode { get; set; }
        public Guid PlayerKey { get; set; }
        public string Word { get; set; }
        public bool WordWasSkipped { get; set; }
    }
}
