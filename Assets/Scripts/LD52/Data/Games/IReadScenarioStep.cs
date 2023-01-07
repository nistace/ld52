namespace LD52.Data.Games {
	public interface IReadScenarioStep {
		bool               equipmentStep { get; }
		ScenarioStepReward rewards       { get; }
	}
}