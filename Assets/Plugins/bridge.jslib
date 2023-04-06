// This file is used to bridge Unity and Javascript, so that we can call Javascript functions from Unity.
mergeInto(LibraryManager.library, {

	// Add a message to the HTML chat window
	AddChatJS: function (msg, player) {
		console.log("Add HTML dialogue from Unity");
		var utfString = UTF8ToString(msg);
		var unityInstance = document.unityInstance;
		if (unityInstance) {
			AddToChat(utfString, player);
		}
	},

	// Show/hide the HTML input
	ShowInputJS: function (action) {
	    ShowInputAction(action);
	},

	// Show/hide chat chat UI
	SetChatActiveJS: function (active) {
		SetChatActive(active);
	},
});
