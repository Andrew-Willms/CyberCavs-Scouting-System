using System;
using System.Collections.Generic;

namespace UtilitiesLibrary.Validation;



public class ValidationEvent {

	private List<Action> Delegates { get; } = new();

	public void Subscribe(Action action) {
		Delegates.Add(action);
	}

	public void UnSubscribe(Action action) {
		Delegates.Remove(action);
	}

	public void SubscribeTo(ValidationEvent validationEvent) {
		validationEvent.Subscribe(Invoke);
	}

	public void UnsubscribeFrom(ValidationEvent validationEvent) {
		validationEvent.UnSubscribe(Invoke);
	}

	public void Invoke() {

		foreach (Action function in Delegates) {
			function.Invoke();
		}
	}

	//public void EventHandler(object? sender, EventArgs e) {
	//	Invoke();
	//}
}