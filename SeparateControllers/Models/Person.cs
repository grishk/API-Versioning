namespace SeparateControllers.Models
{
    public class Person
    {
        public Person()
        {
        }

        public Person(int id)
        {
            Id = id;
        }

        public int Id { get; set; }

        public string Desc { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}