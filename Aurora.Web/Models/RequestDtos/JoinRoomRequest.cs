namespace Aurora.Web.Models.RequestDtos
{
    public class JoinRoomRequest
    {
        public string RoomCode { get; set; }

        public string PlayerName { get; set; }
        public string PlayerEmail { get; set; }
    }
}
