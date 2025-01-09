public interface INode
{
    float randomedValue { get; set; }
    bool AlreadyGenerate { get; set; }
    int storyID { get; set; }
    string heading { get; set; }
    string storyText { get; set; }
    string outcome { get; set; }
}