
public interface IDialogueNodeVisitor
{
    void Visit(BasicDialogueNode node);
    void Visit(ChoiceDialogueNode node);
}