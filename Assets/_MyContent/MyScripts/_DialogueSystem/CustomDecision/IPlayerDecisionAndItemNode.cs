using Devdog.QuestSystemPro.Dialogue;

public interface IPlayerDecisionAndItemNode {

	int playerDecisionIndex { get; }
	PlayerDecisionAndItem[] playerDecisions { get; }

	/// <summary>
	/// Set the player's decision index. Note that this will move to the next node automatically.
	/// </summary>
	void SetPlayerDecisionAndMoveToNextNode(int index);
}
