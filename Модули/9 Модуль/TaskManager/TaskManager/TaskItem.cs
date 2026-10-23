namespace TaskManager;
// Модель для хранения информации об одной задаче.
public class TaskItem
{
    public string Title { get; set; } = "";
    public string Priority { get; set; } = "Средний";
    public DateTime DueDate { get; set; } = DateTime.Today;
    public string Status { get; set; } = "Не начата";
    public int Progress { get; set; }
}