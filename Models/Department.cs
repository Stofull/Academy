namespace asp_project.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Head { get; set; } // Руководитель кафедры
        public List<Teacher> Teachers { get; set; } = new List<Teacher>();
    }
}
