using System;
using System.Collections.Generic;

namespace UtilitiesLibrary;



public class Event {

	private List<Action> Delegates { get; } = new();

	public void Subscribe(Action action) {
		Delegates.Add(action);
	}

	public void UnSubscribe(Action action) {
		Delegates.Remove(action);
	}

	public void SubscribeTo(Event @event) {
		@event.Subscribe(Invoke);
	}

	public void UnsubscribeFrom(Event @event) {
		@event.UnSubscribe(Invoke);
	}

	public void Invoke() {
		Delegates.ForEach(x => x.Invoke());
	}

}