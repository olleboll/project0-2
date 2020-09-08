
public struct PuzzleData {
	public bool solved;
}

public interface IPuzzleData
{

	PuzzleData GetPuzzleData(string name);
	void SetPuzzleData(PuzzleData data);
}
