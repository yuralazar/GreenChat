namespace GreenChat.DAL.Models
{
    public class Friend
    {
        //Primary key
        public int ID { get; set; }

        //Foreign key
        public string Friend1ID { get; set; }
        public string Friend2ID { get; set; }

        //Navigation properties
        public ApplicationUser Friend1 { get; set; }
        public ApplicationUser Friend2 { get; set; }
    }
}