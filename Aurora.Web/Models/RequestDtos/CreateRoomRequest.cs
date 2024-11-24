namespace Aurora.Web.Models.RequestDtos
{
    public class CreateRoomRequest
    {
        public string RoomCode { get; set; }
        public int NumberOfWordsInGame { get; set; }
        public string? WordList { get; set; }
    }
}
