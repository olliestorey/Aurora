namespace Aurora.Web.Models.ResonseDtos
{
    public class SubmitWordDto
    {
        public bool Result { get; set; }
        public int? Position { get; set; }

        public SubmitWordDto(bool result, int? position)
        {
            Result = result;
            Position = position;
        }
    }
}
