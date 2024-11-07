namespace ITS.QueueSample.Models;

public class SampleMessage
{
    public Product Product { get; set; }
    public DateTime CreationDate { get; set; }
    public CRUDAction Action { get; set; }
}

public enum CRUDAction
{
    Create,
    //Read,
    //ReadAll,
    Update,
    Delete
}
